using System;
using System.Collections.Generic;
using System.Text;

namespace Lynx.Models
{
    // This class will search the requested items in the db
    public class FileItemPresenter
    {
        private readonly FileDatabase _db;
        private readonly int _recordIndex;

        public FileItemPresenter(FileDatabase db, int recordIndex)
        {
            _db = db;
            _recordIndex = recordIndex;
        }

        // Avalonia read from this
        // This will search the object on the array with the index
        public string FileName => _db.Files[_recordIndex].Name;
        public string FilePath => _db.Paths[_db.Files[_recordIndex].PathId];
    }
}
