using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Infrastructure.Model
{
    public class FileItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FolderId { get; set; }
        public FolderItem Folder { get; set; }

    }
}
