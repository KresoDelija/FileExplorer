using FileExplorer.Domain.DTO.File;
using FileExplorer.Domain.Services;
using FileManager.Infrastructure;
using FileManager.Infrastructure.Model;
using Microsoft.EntityFrameworkCore;

namespace FileExplorer.Application.Services
{
    public class FileService : IFileService
    {
        private readonly FileManagerDbContext _dbContext;

        public FileService(FileManagerDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public async Task<FileDto> CreateFile(CreateFileDto fileDto)
        {
            if ((fileDto.FolderId.HasValue))
            {
                var parent = await _dbContext.Folders.FindAsync(fileDto.FolderId);
                if (parent == null)
                {
                    throw new ArgumentException($"Folder with ID {fileDto.FolderId} does not exist.");
                }
            }

            var duplicate = await _dbContext.Files
                .Where(f => f.Name == fileDto.Name && f.FolderId == fileDto.FolderId)
                .FirstOrDefaultAsync(); 
            if (duplicate != null)
            {
                throw new ArgumentException($"File with name {fileDto.Name} already exists in the specified folder.");
            }

            var retVal = new FileItem()
            {
                Name = fileDto.Name,
                FolderId = fileDto.FolderId
            };
            _dbContext.Files.Add(retVal);
            await _dbContext.SaveChangesAsync();
            return new FileDto()
            {
                Id = retVal.Id,
                Name = retVal.Name,
                FolderId = retVal.FolderId
            };
        }

        public async Task<bool> DeleteFile(DeleteFileDto fileDto)
        {
            var file = await _dbContext.Files.FindAsync(fileDto.Id);
            if (file == null)
            {
                throw new ArgumentException($"File with ID {fileDto.Id} does not exist.");
            }
            _dbContext.Files.Remove(file);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<FileDto>> ListFiles(int folderId)
        {
            var files = await _dbContext.Files
                .Where(f => f.FolderId == folderId)
                .ToListAsync();

            return files.Select(f => new FileDto() { FolderId = f.FolderId, Id = f.Id, Name = f.Name});
        }

        public async Task<IEnumerable<FileDto>> SearchFiles(string query, int? folderId)
        {
            var filesQuery = _dbContext.Files.AsQueryable();

            if (folderId.HasValue)
            {
                filesQuery = filesQuery.Where(f => f.FolderId == folderId.Value);
            }

            filesQuery = filesQuery.Where(f => f.Name.ToLowerInvariant().StartsWith(query.ToLowerInvariant()));

            var files = await filesQuery
                .Take(10)
                .ToListAsync();

            return files.Select(f => new FileDto() { FolderId = f.FolderId, Id = f.Id, Name = f.Name });
        }
    }
}
