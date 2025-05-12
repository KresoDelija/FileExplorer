using FileExplorer.Domain.DTO.Folder;

namespace FileExplorer.Domain.Services
{
    public interface IFolderService
    {
        public Task<FolderDto> CreateFolder(CreateFolderDto fileDto);
        public Task<bool> DeleteFolder(DeleteFolderDto fileDto);
        public Task<IEnumerable<FolderDto>> ListFolders(int? folderId);
    }
}
