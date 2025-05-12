namespace FileExplorer.Domain.DTO.File
{
    public class CreateFileDto
    {
        public string Name { get; set; } = string.Empty;
        public int FolderId { get; set; }
    }
}
