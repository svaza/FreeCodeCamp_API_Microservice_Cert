using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TimestampMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhoamiController : ControllerBase
    {
        [Route("")]
        public IActionResult Get()
        {
            return Ok(new { ipaddress = Request.HttpContext.Connection.RemoteIpAddress.ToString(), language = Request.Headers["Accept-Language"], software = Request.Headers["User-Agent"] });
        }
    }
}