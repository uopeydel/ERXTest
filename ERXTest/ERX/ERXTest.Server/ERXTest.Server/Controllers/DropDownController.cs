using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ERXTest.Server.Facade;
using ERXTest.Server.Services;
using ERXTest.Shared.Helpers;
using ERXTest.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERXTest.Shared.Models.Request;

namespace ERXTest.Server.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class DropDownController : BaseController
    {
        private readonly DropDownFacade _DropDownFacade;
        private readonly ILoggerService _logger;
        public DropDownController(ILoggerService logger, DropDownFacade DropDownFacade)
        {
            _logger = logger;
            _DropDownFacade = DropDownFacade;
        }

        [HttpPost("v1/Upsert")]
        public async Task<IActionResult> Upsert([FromBody] DropDownUpsertRequest data)
        {
            var results = await _DropDownFacade.Upsert(data);
            return Ok(results);
        }

        [HttpPost("v1/DropDownItem/{DropDownId}")]
        public async Task<IActionResult> DropDownItem([FromRoute] int DropDownId)
        {
            var results = await _DropDownFacade.DropDownItem(DropDownId);
            return Ok(results);
        }

        [HttpPost("v1/List")]
        public async Task<IActionResult> List()
        {
            var results = await _DropDownFacade.List();
            return Ok(results);
        }
    }
}
