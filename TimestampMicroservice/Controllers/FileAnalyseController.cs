using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TimestampMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileAnalyseController : ControllerBase
    {
        
        public IActionResult Post(IFormFile upfile)
        {
            return Ok(new { name = upfile.FileName, type = upfile.ContentType, size = upfile.Length });
        }

    }
}