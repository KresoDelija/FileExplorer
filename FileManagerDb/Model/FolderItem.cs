using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Infrastructure.Model
{
    public class FolderItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; } // Nullable for root folders
        public List<FileItem> Files { get; set; } = new List<FileItem>();
        public List<FolderItem> SubFolders { get; set; } = new List<FolderItem>();

    }
}
