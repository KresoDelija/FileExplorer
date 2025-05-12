using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Domain.DTO.Folder
{
    public class DeleteFolderDto
    {
        public int Id { get; set; }

        public DeleteFolderDto(int id)
        {
            Id = id;
        }
    }
}
