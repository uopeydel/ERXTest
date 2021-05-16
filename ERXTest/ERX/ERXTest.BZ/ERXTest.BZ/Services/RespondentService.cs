using ERXTest.Shared.DBModels;
using ERXTest.Shared.Helpers;
using ERXTest.Shared.Models;
using ERXTest.Shared.Models.Request;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERXTest.BZ.Services
{
    public interface IRespondentService
    {
        Task<ERXTestResults<bool>> Create(RespondentCreateRequest data);
        Task<ERXTestResults<QuestionModel>> ReportList(int questionId);
        Task<ERXTestResults<List<RespondentModel>>> Report(int questionId, Guid responder);
        Task ValidRespondentGuid();
        Task<ERXTestResults<List<Guid>>> RespondentsList();
    }

    public class RespondentService : IRespondentService
    {
        private IHttpService _httpService;
        private ILocalStorageService _LocalStorageService;
        public RespondentService(IHttpService httpService, ILocalStorageService LocalStorageService)
        {
            _httpService = httpService; _LocalStorageService = LocalStorageService;
        }

        public async Task ValidRespondentGuid()
        {
            var respondentGuid = await _LocalStorageService.GetItem<string>("RespondentGuid");
            if (string.IsNullOrEmpty(respondentGuid))
            {
                await _LocalStorageService.SetItem<string>("RespondentGuid", Guid.NewGuid().ToString());
            }
        }

        public async Task<ERXTestResults<bool>> Create(RespondentCreateRequest data)
        {
            ERXTestResults<bool> response = await _httpService.Post<ERXTestResults<bool>>(
                "/api/Respondent/v1/Create", data);
            return response;
        }

        public async Task<ERXTestResults<QuestionModel>> ReportList(int questionId)
        {
            ERXTestResults<QuestionModel> response = await _httpService.Post<ERXTestResults<QuestionModel>>(
                $"/api/Respondent/v1/ReportList/{questionId}" ,default);
            return response;
        }

        public async Task<ERXTestResults<List<RespondentModel>>> Report(int questionId, Guid responder)
        {
            ERXTestResults<List<RespondentModel>> response = await _httpService.Post<ERXTestResults<List<RespondentModel>>>(
                $"/api/Respondent/v1/Report/{questionId}/{responder}", default);
            return response;
        }

        public async Task<ERXTestResults<List<Guid>>> RespondentsList()
        {
            ERXTestResults<List<Guid>> response = await _httpService.Post<ERXTestResults<List<Guid>>>(
              $"/api/Respondent/v1/RespondentsList", default);
            return response;
        }

    }
}