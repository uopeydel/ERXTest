using ERXTest.Server.Services;
using ERXTest.Shared.DBModels;
using ERXTest.Shared.Helpers;
using ERXTest.Shared.Models;
using ERXTest.Shared.Models.Request;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks; 
namespace ERXTest.Server.Facade
{
    public class RespondentFacade
    {
        private readonly IRespondentService _RespondentService; 
        public RespondentFacade( 
              IRespondentService RespondentService)
        {
            _RespondentService = RespondentService; 
        }

        public async Task<ERXTestResults<List<Guid>>> RespondentsList()
        {
            try
            {
                return await _RespondentService.RespondentsList();
            }
            catch (Exception ex)
            {
                return ERXTestResponse.CreateErrorResponse<List<Guid>>("Error At RespondentsList facade", ex);
            }
        }
        public async Task<ERXTestResults<bool>> Create(RespondentCreateRequest data)
        {
            try
            {
                ERXTestResults<bool> result = await _RespondentService.Create(data); 
                return result;
            }
            catch (Exception ex)
            {
                return ERXTestResponse.CreateErrorResponse<bool>("Error At Create", ex);
            }
        }

        public async Task<ERXTestResults<QuestionModel>> ReportList(int questionId)
        {
            try
            {
                ERXTestResults<QuestionModel> result = await _RespondentService.ReportList(questionId); 
                return result;
            }
            catch (Exception ex)
            {
                return ERXTestResponse.CreateErrorResponse<QuestionModel>("Error At ReportList facade", ex);
            }
        }

        public async Task<ERXTestResults<List<RespondentModel>>> Report(int questionId, Guid responder)
        {
            try
            {
                var result = await _RespondentService.Report(questionId ,responder); 
                return result;
            }
            catch (Exception ex)
            {
                return ERXTestResponse.CreateErrorResponse<List<RespondentModel>>("Error At Create", ex);
            }
        }
    }
}
