namespace FileExplorer.Domain.DTO.Folder
{
    public class FolderDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        
    }
}
