using System;
using System.Net;
using System.Net.Sockets;
using Microsoft.AspNetCore.Mvc;

namespace TimestampMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShortUrlController : ControllerBase
    {
        private static readonly int URL_ID = 1;

        private static string _requestUrl = "http://www.google.com";

        [HttpPost("new")]
        public IActionResult NewUrl([FromBody]NewShortUrlRequestModel request)
        {
            try
            {
                Dns.GetHostEntry(new Uri(request.Url).DnsSafeHost);
            }
            catch (Exception)
            {
                return Ok(new { error = "invalid URL" });
            }
            _requestUrl = request.Url;
            return Ok(new { original_url = request.Url, short_url = URL_ID });
        }


        [Route("{id}")]
        public IActionResult Get(int id)
        {
            return Redirect(_requestUrl);
        }

    }

    

    public class NewShortUrlRequestModel
    {
        public string Url { get; set; }
    }
}

