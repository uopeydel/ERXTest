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
    public class RespondentController : BaseController
    {
        private readonly RespondentFacade _RespondentFacade;
        private readonly ILoggerService _logger;
        public RespondentController(ILoggerService logger, RespondentFacade RespondentFacade)
        {
            _logger = logger;
            _RespondentFacade = RespondentFacade;
        }
         
        [HttpPost("v1/Create")]
        public async Task<IActionResult> Create([FromBody] RespondentCreateRequest data)
        {
            var results = await _RespondentFacade.Create(data);
            return Ok(results);
        }

        [HttpPost("v1/ReportList/{QuestionId}")]
        public async Task<IActionResult> ReportList([FromRoute] int QuestionId)
        {
            var results = await _RespondentFacade.ReportList(QuestionId);
            return Ok(results);
        }

        [HttpPost("v1/Report/{QuestionId}/{Responder}")]
        public async Task<IActionResult> Report([FromRoute] int QuestionId, [FromRoute] Guid Responder)
        {
            var results = await _RespondentFacade.Report(QuestionId, Responder);
            return Ok(results);
        }

        [HttpPost("v1/RespondentsList")]
        public async Task<IActionResult> RespondentsList( )
        {
            var results = await _RespondentFacade.RespondentsList();
            return Ok(results);
        }
        


    }
}
