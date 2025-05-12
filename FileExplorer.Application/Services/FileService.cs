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

            var parent = await _dbContext.Folders.FindAsync(fileDto.FolderId);
            if (parent == null)
            {
                throw new ArgumentException($"Folder with ID {fileDto.FolderId} does not exist.");
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

            return files.Select(async f => new FileDto()
            {
                FolderId = f.FolderId,
                Id = f.Id,
                Name = f.Name,
                Path = await GetFullPath(f.FolderId, f.Name)
            }).Select(t => t.Result); // Fix: Await the Task<string> and return the result
        }

        private async Task<string> GetFullPath(int folderId, string fileName)
        {
            if (folderId == null)
            {
                return fileName;
            }

            var folder = await _dbContext.Folders.FindAsync(folderId);
            if (folder == null)
            {
                throw new ArgumentException($"Folder with ID {folderId} does not exist.");
            }

            var pathSegments = new List<string> { fileName };
            var currentFolder = folder;

            while (currentFolder != null)
            {
                pathSegments.Insert(0, currentFolder.Name);
                if (currentFolder.ParentId == null)
                {
                    break;
                }
                else
                {
                    currentFolder = await GetParentFolder(currentFolder.ParentId.Value);
                }
            }

            return string.Join("/", pathSegments);
        }

        private async Task<FolderItem?> GetParentFolder(int folderId)
        {
            var folder = await _dbContext.Folders.FindAsync(folderId);
            if (folder == null)
            {
                throw new ArgumentException($"Folder with ID {folderId} does not exist.");
            }
            return folder;
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

            var fileDtos = new List<FileDto>();
            foreach (var f in files)
            {
                fileDtos.Add(new FileDto()
                {
                    FolderId = f.FolderId,
                    Id = f.Id,
                    Name = f.Name,
                    Path = await GetFullPath(f.FolderId, f.Name) // Fix: Await the Task<string> here
                });
            }

            return fileDtos;
        }
    }
}
