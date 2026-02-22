using Avalonia.Controls.Shapes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Lynx.Models;
using System.Collections.ObjectModel;

namespace Lynx.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        // ------------------------------ //
        //           ATTRIBUTES           //
        // ------------------------------ //
        // User input to search
        [ObservableProperty]
        private string _searchQuery = string.Empty;
        // Flag to show when it is searching
        [ObservableProperty]
        private bool _isSearching;
        // The list that will be drawn by the UI
        public VirtualResultList SearchResults { get; } = new();
        // The "DB" in RAM
        private readonly FileDatabase _database = new();
        // Pre-take the memory instance
        private int[] _searchBuffer = Array.Empty<int>();



        // ------------------------------ //
        //          CONSTRUCTOR           //
        // ------------------------------ //
        public MainWindowViewModel()
        {
            // --- SIMULATION ---
            // Initialize the app by creating 4 million fake files
            GenerateFakeDataBase();
        }



        // ------------------------------ //
        //            COMMANDS            //
        // ------------------------------ //
        // The search command in background
        [RelayCommand]
        private async Task PerformSearchAsync(string query)
        {
            // Set the searching flag true
            IsSearching = true;

            // Background search
            var matchedCount = await Task.Run(() =>
            {
                int count = 0;

                // Iterate over the 4 million fake files in struct
                // As it is stored adjacently in memory, the CPU iterates fast
                for (int i = 0; i < _database.Files.Length; i++)
                {
                    if (_database.Files[i].Name?.Contains(query, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        _searchBuffer[count] = i;
                        count++;
                    }
                }
                return count;
            });

            // Update the UI
            Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                SearchResults.UpdateResults(_database, _searchBuffer, matchedCount);

                // Set the searching flag false
                IsSearching = false;
            });
            
        }



        // ------------------------------ //
        //            METHODS             //
        // ------------------------------ //
        // Will be automatically executed when the user writes anything in the UI's search field
        partial void OnSearchQueryChanged(string value)
        {
            // If there is no text in the field, clean the list
            if (string.IsNullOrWhiteSpace(value))
            {
                SearchResults.UpdateResults(_database, _searchBuffer, 0);
                return;
            }

            // Start the search without blocking the UI
            PerformSearchCommand.Execute(value);
        }

        private void GenerateFakeDataBase()
        {
            int totalFakeFiles = 1_000_000;
            _database.Files = new FileRecord[totalFakeFiles];

            // Duplicate paths
            _database.Paths[0] = @"C:\Windows\System32\";
            _database.Paths[1] = @"C:\Users\Oscar\Documents\";
            _database.Paths[2] = @"C:\Users\Oscar\Documents\";
            _database.Paths[3] = @"C:\Users\Oscar\Documents\";
            _database.Paths[4] = @"C:\Juegos\Steam\";
            _database.Paths[5] = @"C:\Juegos\EpicGames\";
            _database.Paths[6] = @"D:\Proyectos\";

            var rand = new Random();

            for (int i = 0; i < totalFakeFiles; i++)
            {
                // Shuffle the files between the 7 possible paths
                int pathId = i % 7;
                // Easy name
                _database.Files[i] = new FileRecord($"archivo_{i}.txt", pathId, rand.Next());
            }

            _searchBuffer = new int[_database.Files.Length];
        }
    }
}
