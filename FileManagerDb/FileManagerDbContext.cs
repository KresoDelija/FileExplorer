using System.Collections.Generic;
using FileManager.Infrastructure.Model;
using Microsoft.EntityFrameworkCore;

namespace FileManager.Infrastructure
{
    public class FileManagerDbContext : DbContext
    {
        public FileManagerDbContext(DbContextOptions<FileManagerDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FolderItem>().HasData(
                new FolderItem
                {
                    Id = 1,
                    Name = "root",
                    ParentId = null
                },
                new FolderItem
                {
                    Id = 2,
                    Name = "documents",
                    ParentId = 1
                },
                new FolderItem
                {
                    Id = 3,
                    Name = "images",
                    ParentId = 1
                },
                new FolderItem
                {
                    Id = 4,
                    Name = "work",
                    ParentId = 2
                }
            );

            modelBuilder.Entity<FileItem>().HasData(
                new FileItem
                {
                    Id = 1,
                    Name = "work_cv.docx",
                    FolderId = 4
                },
                new FileItem
                {
                    Id = 2,
                    Name = "work_project_plan.xlsx",
                    FolderId = 4
                },
                new FileItem
                {
                    Id = 3,
                    Name = "vacation_beach.jpg",
                    FolderId = 3
                },
                new FileItem
                {
                    Id = 4,
                    Name = "Beach_FamilyPhoto.png",
                    FolderId = 3
                }
            );
        }

        public DbSet<FolderItem> Folders { get; set; }
        public DbSet<FileItem> Files { get; set; }

        

        public override int SaveChanges()
        {
            var rootFolder = ChangeTracker.Entries<FolderItem>()
                .FirstOrDefault(e => e.State == EntityState.Deleted && e.Entity.ParentId == null);

            if (rootFolder != null)
            {
                throw new InvalidOperationException("The root folder cannot be deleted.");
            }

            return base.SaveChanges();
        }


    }
}
