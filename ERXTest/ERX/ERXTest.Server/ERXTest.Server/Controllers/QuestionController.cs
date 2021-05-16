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
    public class QuestionController : BaseController
    {
        private readonly QuestionFacade _QuestionFacade;
        private readonly ILoggerService _logger;
        public QuestionController(ILoggerService logger, QuestionFacade QuestionFacade)
        {
            _logger = logger;
            _QuestionFacade = QuestionFacade;
        }

        [HttpPost("v1/List")]
        [HttpPost("v1/List/{respondentGuid}")]
        public async Task<IActionResult> GetQuestionList([FromRoute] Guid? respondentGuid)
        {
            var results = await _QuestionFacade.GetQuestionList(respondentGuid);
            return Ok(results);
        }

        [HttpGet("v1/GetQuestionById/{Id}")]
        [HttpGet("v1/GetQuestionById/{Id}/{respondentGuid}")]
        public async Task<IActionResult> GetQuestionById([FromRoute] int Id, [FromRoute] Guid? respondentGuid)
        {
            var results = await _QuestionFacade.GetQuestionById(Id, respondentGuid);
            return Ok(results);
        }

        [HttpPost("v1/Upsert")]
        public async Task<IActionResult> Upsert([FromBody] QuestionUpsertRequest data)
        {
            var results = await _QuestionFacade.Upsert(data);
            return Ok(results);
        }


        [HttpPost("v1/Delete/{questionId}")]
        public async Task<IActionResult> Delete([FromRoute] int questionId)
        {
            var results = await _QuestionFacade.Delete(questionId);
            return Ok(results);
        }

    }
}
