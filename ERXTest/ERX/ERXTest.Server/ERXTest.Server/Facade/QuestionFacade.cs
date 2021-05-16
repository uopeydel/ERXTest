using ERXTest.Server.Services;
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
    public class QuestionFacade
    {
        private readonly IQuestionService _QuestionService;
        public QuestionFacade(
              IQuestionService QuestionService)
        {
            _QuestionService = QuestionService;
        }

        public async Task<ERXTestResults<List<QuestionModel>>> GetQuestionList(Guid? respondentGuid)
        {
            try
            {
                ERXTestResults<List<QuestionModel>> result = await _QuestionService.GetQuestionList(respondentGuid);
                return result;
            }
            catch (Exception ex)
            {
                return ERXTestResponse.CreateErrorResponse<List<QuestionModel>>("Error At GetQuestionList facade", ex);
            }
        }

        public async Task<ERXTestResults<QuestionModel>> GetQuestionById(int id, Guid? respondentGuid)
        {
            try
            {
                ERXTestResults<QuestionModel> result = await _QuestionService.GetQuestionById(id, respondentGuid);
                return result;
            }
            catch (Exception ex)
            {
                return ERXTestResponse.CreateErrorResponse<QuestionModel>("Error At GetQuestionById facade", ex);
            }
        }

        public async Task<ERXTestResults<bool>> Upsert(QuestionUpsertRequest data)
        {
            try
            {
                ERXTestResults<bool> result = await _QuestionService.Upsert(data);
                return result;
            }
            catch (Exception ex)
            {
                return ERXTestResponse.CreateErrorResponse<bool>("Error At Upsert facade", ex);
            }
        }


        public async Task<ERXTestResults<bool>> Delete(int questionId)
        {
            try
            {

                ERXTestResults<bool> result = await _QuestionService.Delete(questionId);
                return result;
            }
            catch (Exception ex)
            {
                return ERXTestResponse.CreateErrorResponse<bool>("Error At delete", ex);
            }
        }

    }
}
