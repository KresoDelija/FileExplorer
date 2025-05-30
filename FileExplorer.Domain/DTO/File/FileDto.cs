﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Domain.DTO.File
{
    public class FileDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? FolderId { get; set; }
        public string Path { get; set; }
    }
}
