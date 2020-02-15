using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TimestampMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimestampController : ControllerBase
    {
        [Route("{dateParam}")]
        public IActionResult Get(string dateParam)
        {
            DateTimeOffset currentDate;
            if (string.IsNullOrEmpty(dateParam))
            {
                currentDate = DateTimeOffset.UtcNow;
            }
            else
            {
                if(long.TryParse(dateParam, out long sec))
                {
                    currentDate = DateTimeOffset.FromUnixTimeMilliseconds(sec);
                }
                else
                {
                    try
                    {
                        currentDate = new DateTimeOffset(DateTime.Parse(dateParam), DateTimeOffset.UtcNow.Offset);
                    }
                    catch (FormatException)
                    {
                        return Ok(new { error = "Invalid Date" });
                    }
                }
                
            }
            return Ok(new
            {
                unix = currentDate.ToUnixTimeMilliseconds(),
                utc = currentDate.ToString("ddd, dd MMM yyyy HH:mm:ss") + " GMT"
            });
        }
    }
}