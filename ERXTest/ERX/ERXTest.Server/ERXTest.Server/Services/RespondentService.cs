using AutoMapper;
using ERXTest.Server.Helper;
using ERXTest.Server.Repositories;
using ERXTest.Shared.DBModels;
using ERXTest.Shared.Helpers;
using ERXTest.Shared.Models;
using ERXTest.Shared.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERXTest.Server.Services
{
    public interface IRespondentService
    {
        Task<ERXTestResults<List<Guid>>> RespondentsList();
        Task<ERXTestResults<bool>> Create(RespondentCreateRequest data);
        Task<ERXTestResults<QuestionModel>> ReportList(int questionId);
        Task<ERXTestResults<List<RespondentModel>>> Report(int questionId, Guid responder);
    }
    public class RespondentService : IRespondentService
    {
        private readonly IMapper _mapper;
        public readonly DateTime now;
        private readonly IRespondentRepository _RespondentRepository;
        private readonly ILoggerService _logger;
        public RespondentService(
            IRespondentRepository RespondentRepository,
            ILoggerService logger,
            IMapper mapper
        )
        {
            _mapper = mapper;
            _RespondentRepository = RespondentRepository;
            _logger = logger;
            now = DateTime.UtcNow.AddHours(7);
        }


        public async Task<ERXTestResults<List<Guid>>> RespondentsList()
        {
            try
            {
                return await _RespondentRepository.RespondentsList();
            }
            catch (Exception ex)
            {
                return ERXTestResponse.CreateErrorResponse<List<Guid>>("Error At RespondentsList service", ex);
            }
        }


        public async Task<ERXTestResults<bool>> Create(RespondentCreateRequest data)
        {
            try
            {
                ERXTestResults<bool> result = await _RespondentRepository.Create(_mapper.Map<List<Respondent>>(data.respondent));
                return result;
            }
            catch (Exception ex)
            {
                return ERXTestResponse.CreateErrorResponse<bool>("Error At Create service", ex);
            }
        }

        public async Task<ERXTestResults<QuestionModel>> ReportList(int questionId)
        {
            try
            {
                ERXTestResults<QuestionModel> result = await _RespondentRepository.ReportList(questionId);
                return result;
            }
            catch (Exception ex)
            {
                return ERXTestResponse.CreateErrorResponse<QuestionModel>("Error At ReportList service", ex);
            }
        }

        //gen xls
        public async Task<ERXTestResults<List<RespondentModel>>> Report(int questionId, Guid responder)
        {
            try
            {
                ERXTestResults<List<Respondent>> dataresult = await _RespondentRepository.Report(questionId, responder);
                if (dataresult.Error)
                {
                    return ERXTestResponse.CreateErrorResponse<List<RespondentModel>>(dataresult.Message);
                }
                var resultMapped = _mapper.Map<List<RespondentModel>>(dataresult.Data);
                var result = ERXTestResponse.CreateSuccessResponse(resultMapped);
                return result;
            }
            catch (Exception ex)
            {
                return ERXTestResponse.CreateErrorResponse<List<RespondentModel>>("Error At Report service", ex);
            }
        }
    }

}
