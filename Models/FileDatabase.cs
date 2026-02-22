using System;
using System.Collections.Generic;
using System.Text;

namespace Lynx.Models
{
    public class FileDatabase
    {
        // Contiguous array
        public FileRecord[] Files = [];

        // Dictionary for not duplicating paths
        public Dictionary<int, string> Paths = new();
    }
}
