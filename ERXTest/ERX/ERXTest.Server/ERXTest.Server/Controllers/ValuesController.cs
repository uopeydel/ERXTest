
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using ERXTest.Shared.Helpers;
using ERXTest.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERXTest.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [Obsolete]
        private readonly IHostingEnvironment host;

        [Obsolete]
        public ValuesController(IHostingEnvironment _host)
        {
            host = _host;
        }

        [HttpGet("Test")]
        [Obsolete]
        public IActionResult Test()
        {
            return Ok(host.EnvironmentName);
        }

        //[HttpPost("Base64ToPhone")]
        //public async Task<IActionResult> Base64ToPhone([FromBody] XLSObject data)
        //{ 
        //    var byteArray = ReadXLS.Base64ToByteArray(data.Message);
        //    var stream = ReadXLS.ByteToStream(byteArray);
        //    var phone = ReadXLS.ReadToPhone(stream);
        //    XLSObject resultObj = new XLSObject { };
        //    return Ok(ERXTestResponse.CreateSuccessResponse(phone));
        //}


    }
}
