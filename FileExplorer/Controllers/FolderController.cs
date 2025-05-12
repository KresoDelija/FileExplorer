using FileExplorer.Domain.DTO.Folder;
using FileExplorer.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileExplorer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FolderController : ControllerBase
    {

        private readonly ILogger<FolderController> _logger;
        private readonly IFolderService _service;
        public FolderController(IFolderService service, ILogger<FolderController> logger)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet()]
        public async Task<IActionResult> Get([FromQuery] int? folderId)
        {
            IEnumerable<FolderDto> retVal;

            retVal = await _service.ListFolders(folderId);            

            return Ok(retVal); 
        }

        [HttpPost()]
        public async Task<IActionResult> CreateFolder([FromBody] CreateFolderDto folder)
        {
            var retVal = await _service.CreateFolder(folder);
            return CreatedAtAction(nameof(CreateFolder), new { id = retVal.Id }, retVal);
        }

        [HttpDelete()]
        public async Task<IActionResult> DeleteFolder([FromBody] DeleteFolderDto folder)
        {
            var retVal = await _service.DeleteFolder(folder);
            if (retVal)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

    }
}
