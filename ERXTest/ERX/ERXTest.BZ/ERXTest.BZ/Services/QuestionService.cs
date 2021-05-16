using ERXTest.BZ.Services;
using ERXTest.Shared.Helpers;
using ERXTest.Shared.Models;
using ERXTest.Shared.Models.Request;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERXTest.BZ.Services
{
    public interface IQuestionService
    {
        Task<ERXTestResults<QuestionModel>> GetQuestionById(int id, Guid? respondentGuid);
        Task<ERXTestResults<bool>> Upsert(QuestionUpsertRequest data);
        Task<ERXTestResults<List<QuestionModel>>> GetQuestionList(Guid? respondentGuid);
        Task<ERXTestResults<bool>> Delete(int questionId);
    }

    public class QuestionService : IQuestionService
    {
        private IHttpService _httpService;

        public QuestionService(IHttpService httpService)
        {
            _httpService = httpService;
        }



        public async Task<ERXTestResults<List<QuestionModel>>> GetQuestionList(Guid? respondentGuid)
        {
            ERXTestResults<List<QuestionModel>> response = await _httpService.Post<ERXTestResults<List<QuestionModel>>>(
               $"/api/Question/v1/List/{respondentGuid}", default);
            return response;
        }

        public async Task<ERXTestResults<QuestionModel>> GetQuestionById(int id, Guid? respondentGuid)
        {
            ERXTestResults<QuestionModel> response = await _httpService.Get<ERXTestResults<QuestionModel>>(
                $"/api/Question/v1/GetQuestionById/{id}/{respondentGuid}");
            return response;
        }

        public async Task<ERXTestResults<bool>> Upsert(QuestionUpsertRequest data)
        {
            ERXTestResults<bool> response = await _httpService.Post<ERXTestResults<bool>>(
               "/api/Question/v1/Upsert", data);
            return response;
        }


        public async Task<ERXTestResults<bool>> Delete(int questionId)
        {
            ERXTestResults<bool> response = await _httpService.Post<ERXTestResults<bool>>(
               $"/api/Question/v1/Delete/{questionId}", default);
            return response;
        }


    }
}