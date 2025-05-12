using FileExplorer.Domain.DTO.Folder;
using FileExplorer.Domain.Services;
using FileManager.Infrastructure;
using FileManager.Infrastructure.Model;
using Microsoft.EntityFrameworkCore;

namespace FileExplorer.Application.Services
{
    public class FolderService : IFolderService
    {
        private readonly FileManagerDbContext _dbContext;

        public FolderService(FileManagerDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<FolderDto> CreateFolder(CreateFolderDto newFolderDto)
        {
            if ((newFolderDto.ParentId.HasValue))
            {
                var parent = await _dbContext.Folders.FindAsync(newFolderDto.ParentId);
                if (parent == null)
                {
                    throw new ArgumentException($"Parent folder with ID {newFolderDto.ParentId} does not exist.");
                }
            }

            var duplicate = await _dbContext.Folders
                .Where(f => f.Name == newFolderDto.Name && f.ParentId == newFolderDto.ParentId)
                .FirstOrDefaultAsync();
            if (duplicate != null)
            {
                throw new ArgumentException($"Folder with name {newFolderDto.Name} already exists in the specified folder.");
            }

            var newFolder = new FolderItem() {
                Name = newFolderDto.Name,
                ParentId = newFolderDto.ParentId
            };
            _dbContext.Folders.Add(newFolder);
            await _dbContext.SaveChangesAsync();
            return new FolderDto()
            {
                Id = newFolder.Id,
                Name = newFolder.Name,
                ParentId = newFolder.ParentId
            };
        }

        public async Task<bool> DeleteFolder(DeleteFolderDto folderDto)
        {
            var folder = await _dbContext.Folders.FindAsync(folderDto.Id);
            if (folder == null)
            {
                throw new ArgumentException($"Folder with ID {folderDto.Id} does not exist.");
            }

            var children = await _dbContext.Folders
                .Where(f => f.ParentId == folder.Id)
                .ToListAsync();
            if (children.Any())
            {
                throw new ArgumentException($"Folder with ID {folder.Id} has subfolders.");

            }

            var files = await _dbContext.Files
                .Where(f => f.FolderId == folder.Id)
                .ToListAsync();
            if (files.Any())
            {
                throw new ArgumentException($"Folder with ID {folder.Id} has files.");

            }

            _dbContext.Folders.Remove(folder);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<FolderDto>> ListFolders(int? folderId)
        {
            var folders = await _dbContext.Folders
                .Where(f => f.ParentId == folderId)
                .Select(f => new FolderDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    ParentId = f.ParentId
                })
                .ToListAsync();

            return folders;
        }
    }
}
