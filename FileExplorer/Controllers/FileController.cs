using FileExplorer.Domain.DTO.File;
using FileExplorer.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileExplorer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {

        private readonly ILogger<FileController> _logger;
        private readonly IFileService _service;
        public FileController(IFileService service, ILogger<FileController> logger)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet()]
        public async Task<IActionResult> Get([FromQuery] int folderId)
        {
            IEnumerable<FileDto> retVal;

            retVal = await _service.ListFiles(folderId);

            return Ok(retVal);
        }

        [HttpPost()]
        public async Task<IActionResult> CreateFile([FromBody] CreateFileDto file)
        {
            var retVal = await _service.CreateFile(file);
            return CreatedAtAction(nameof(CreateFile), new { id = retVal.Id }, retVal);
        }

        [HttpDelete()]
        public async Task<IActionResult> DeleteFile([FromBody] DeleteFileDto file)
        {
            var retVal = await _service.DeleteFile(file);
            if (retVal)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchFiles(string query, int? folderId)
        {
            IEnumerable<FileDto> retVal;
            retVal = await _service.SearchFiles(query, folderId);
            return Ok(retVal);

        }
    }
}
