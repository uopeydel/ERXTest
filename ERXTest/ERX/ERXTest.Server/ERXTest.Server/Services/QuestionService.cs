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
    public interface IQuestionService
    {
        Task<ERXTestResults<QuestionModel>> GetQuestionById(int id, Guid? respondentGuid);
        Task<ERXTestResults<bool>> Upsert(QuestionUpsertRequest data);
        Task<ERXTestResults<List<QuestionModel>>> GetQuestionList(Guid? respondentGuid);
        Task<ERXTestResults<bool>> Delete(int questionId);
    }
    public class QuestionService : IQuestionService
    {
        private readonly IMapper _mapper;
        public readonly DateTime now;
        private readonly IQuestionRepository _QuestionRepository;
        private readonly ILoggerService _logger;
        public QuestionService(
            IQuestionRepository QuestionRepository,
            ILoggerService logger,
             IMapper mapper
        )
        {
            _mapper = mapper;
            _QuestionRepository = QuestionRepository;
            _logger = logger;
            now = DateTime.UtcNow.AddHours(7);
        }


        public async Task<ERXTestResults<List<QuestionModel>>> GetQuestionList(Guid? respondentGuid)
        {
            try
            {
                var isEndResult =await _QuestionRepository.IsUserTouchEndAnswer(respondentGuid);
                if (isEndResult.Error)
                {
                    return ERXTestResponse.CreateErrorResponse<List<QuestionModel>>(isEndResult.Message);
                }
                if (isEndResult.Data)
                {
                    return ERXTestResponse.CreateSuccessResponse(new List<QuestionModel> { });
                }
                ERXTestResults<List<QuestionModel>> result = await _QuestionRepository.GetQuestionList(respondentGuid);
                return result;
            }
            catch (Exception ex)
            {
                return ERXTestResponse.CreateErrorResponse<List<QuestionModel>>("Error At Create", ex);
            }
        }

        public async Task<ERXTestResults<QuestionModel>> GetQuestionById(int id, Guid? respondentGuid)
        {
            try
            {
                ERXTestResults<QuestionModel> result = await _QuestionRepository.GetQuestionById(id, respondentGuid);
                return result;
            }
            catch (Exception ex)
            {
                return ERXTestResponse.CreateErrorResponse<QuestionModel>("Error At Create", ex);
            }
        }

        public async Task<ERXTestResults<bool>> Upsert(QuestionUpsertRequest data)
        {
            try
            {

                ERXTestResults<bool> result = await _QuestionRepository.Upsert(
                     _mapper.Map<Question>(data.question),
                     _mapper.Map<List<Answer>>(data.answer)
                    );
                return result;
            }
            catch (Exception ex)
            {
                return ERXTestResponse.CreateErrorResponse<bool>("Error At Create", ex);
            }
        }

        public async Task<ERXTestResults<bool>> Delete(int questionId)
        {
            try
            {

                ERXTestResults<bool> result = await _QuestionRepository.Delete(questionId);
                return result;
            }
            catch (Exception ex)
            {
                return ERXTestResponse.CreateErrorResponse<bool>("Error At delete", ex);
            }
        }


    }

}
