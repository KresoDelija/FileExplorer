using FileExplorer.Application.Services;
using FileExplorer.Domain.Services;
using FileManager.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FileExplorer.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<FileManagerDbContext>(options =>
                options.UseInMemoryDatabase("FileManagerDb"));

            builder.Services.AddScoped<IFolderService, FolderService>();
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Ensure the database is created (optional)
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<FileManagerDbContext>();
                dbContext.Database.EnsureCreated();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
