using Microsoft.AspNetCore.Mvc;
using XUnitApp.Interface;

namespace XUnitApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadFileController : Controller
    {
        IAzureBlobService _service;
        public UploadFileController(IAzureBlobService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile files)
        {
            if (files == null || files.FileName== "")
            {
                return BadRequest("File not found");
            }

            var response = await _service.UploadFiles(files);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> ReadFile(string files)
        {
            if (files == null || files == "")
            {
                return BadRequest("File not found");
            }

            var response = await _service.ReadFiles(files);
            if(response == null) 
            { 
                return BadRequest(); 
            }
            return Ok(files);
        }
    }
}