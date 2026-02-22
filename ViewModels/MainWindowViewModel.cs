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
        public ObservableCollection<FileItem> SearchResults { get; } = new();



        // ------------------------------ //
        //            COMMANDS            //
        // ------------------------------ //
        // The search command in background
        [RelayCommand]
        private async Task PerformSearchAsync(string query)
        {
            // Set the searching flag true
            IsSearching = true;

            // Clean previous results
            SearchResults.Clear();

            // --------- SIMULATION ---------
            // Generate 10k fake files in a secondary thread
            await Task.Run(async () =>
            {
                await Task.Delay(150); // Simulates disc reading time
                var rand = new Random();
                for (int i = 0; i < 1000000; i++)
                {
                    SearchResults.Add(new FileItem
                    {
                        FileName = $"Archivo_Ultra_Rapido_{query}_{i}.txt",
                        FilePath = $@"C:\Datos\{query}\Subcarpeta\",
                        FileExtension = ".txt",
                        FileSize = rand.Next()
                    });
                }
            });

            // Set the searching flag false
            IsSearching = false;
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
                SearchResults.Clear();
                return;
            }

            // Start the search without blocking the UI
            PerformSearchCommand.Execute(value);
        }
    }
}
