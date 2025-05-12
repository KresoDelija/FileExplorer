using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Domain.DTO.Folder
{
    public class CreateFolderDto
    {
        public string Name { get; set; } = string.Empty;
        public int? ParentId { get; set; }
        public CreateFolderDto(string name, int? parentId)
        {
            Name = name;
            ParentId = parentId;
        }
    }
}
