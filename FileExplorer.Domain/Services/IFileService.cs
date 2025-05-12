using FileExplorer.Domain.DTO.File;

namespace FileExplorer.Domain.Services
{
    public interface IFileService
    {
        public Task<FileDto> CreateFile(CreateFileDto fileDto);
        public Task<bool> DeleteFile(DeleteFileDto fileDto);
        public Task<IEnumerable<FileDto>> ListFiles(int folderId);
        public Task<IEnumerable<FileDto>> SearchFiles(string query, int? folderId);
    }
}
