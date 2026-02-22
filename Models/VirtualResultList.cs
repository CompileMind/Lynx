using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Lynx.Models
{
    /* This class tells Avalonia how many items exists, but does not store objects,
     * only the files' indexes matching with the search
     */
    public class VirtualResultList : IReadOnlyList<FileItemPresenter>, IList, INotifyCollectionChanged
    {
        private FileDatabase? _db;
        private int[] _matchesIndexes = Array.Empty<int>();
        private int _matchCount = 0;


        
        // --- Implementation from 'INotifyCollectionChanged' ---
        // Event that Avalonia will use to know when to update the screen
        public event NotifyCollectionChangedEventHandler? CollectionChanged;



        // --- PUBLIC METHODS (Modern C# and strongly-typed) ---
        public int Count => _matchCount;

        // Strongly-typed indexer
        public FileItemPresenter this[int index] => new FileItemPresenter(_db!, _matchesIndexes[index]);

        // Strongly-typed enumerator
        public IEnumerator<FileItemPresenter> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        // --- COMPATIBILITY METHODS FOR AVALONIA (IList) ---
        object? IList.this[int index]
        {
            get => this[index]; // Calls the initializer typed above
            set => throw new NotSupportedException();
        }

        bool IList.IsFixedSize => true;
        bool IList.IsReadOnly => true;
        bool ICollection.IsSynchronized => false;
        object ICollection.SyncRoot => this;

        // Redirect possible Avalonia's call from the non generic enumerator to the generic one
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        // As the list is readonly, don't support these methods
        void ICollection.CopyTo(Array array, int index) => throw new NotSupportedException();
        int IList.Add(object? value) => throw new NotSupportedException();
        void IList.Clear() => throw new NotSupportedException();
        bool IList.Contains(object? value) => throw new NotSupportedException();
        int IList.IndexOf(object? value) => throw new NotSupportedException();
        void IList.Insert(int index, object? value) => throw new NotSupportedException();
        void IList.Remove(object? value) => throw new NotSupportedException();
        void IList.RemoveAt(int index) => throw new NotSupportedException();



        // --- Own methods ---
        public void UpdateResults(FileDatabase db, int[] matchedIndexes, int matchCount)
        {
            _db = db;
            _matchesIndexes = matchedIndexes;
            _matchCount = matchCount;

            // Execute a "Reset" to redraw the list
            CollectionChanged?.Invoke(
                this,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)
            );
        }
    }
}
