using Microsoft.EntityFrameworkCore;
using ERXTest.Server.DataAccess;
using ERXTest.Server.Helper;
using ERXTest.Server.Services;
using ERXTest.Shared.Helpers;
using ERXTest.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERXTest.Shared.Models.Request;
using ERXTest.Shared.DBModels;

namespace ERXTest.Server.Repositories
{
    public interface IRespondentRepository
    {
        Task<ERXTestResults<List<Guid>>> RespondentsList();
        Task<ERXTestResults<bool>> Create(List<Respondent> data);
        Task<ERXTestResults<QuestionModel>> ReportList(int questionId);
        Task<ERXTestResults<List<Respondent>>> Report(int questionId, Guid responder);
    }

    public class RespondentRepository : IRespondentRepository
    {
        private readonly ILoggerService _logger;
        private readonly ERXTestContext _db;
        public DateTime now;
        public RespondentRepository(ERXTestContext db, ILoggerService logger)
        {
            _logger = logger;
            _db = db;
            now = DateTime.UtcNow.AddHours(7);
        }

        public async Task<ERXTestResults<bool>> Create(List<Respondent> data)
        {
            try
            {
                await _db.Respondents.AddRangeAsync(data);
                await _db.SaveChangesAsync();
                return ERXTestResponse.CreateSuccessResponse(true);
            }
            catch (Exception ex)
            {
                return ERXTestResponse.CreateErrorResponse<bool>("Error At Create", ex);
            }
        }

        public async Task<ERXTestResults<List<Guid>>> RespondentsList()
        {
            try
            {
                var data = await _db.Respondents
                    .Select(s => s.Responder).Distinct().ToListAsync();

                return ERXTestResponse.CreateSuccessResponse(data);
            }
            catch (Exception ex)
            {
                return ERXTestResponse.CreateErrorResponse<List<Guid>>("Error At RespondentsList", ex);
            }
        }

        public async Task<ERXTestResults<QuestionModel>> ReportList(int questionId)
        {
            try
            {
                var data = await _db.Questions.Where(w => w.Id == questionId)
                    .Select(s => new QuestionModel
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Respondents = s.Respondents.Select(ss => new RespondentModel
                        {
                            Responder = ss.Responder
                        }
                        ).Distinct().ToList()
                    }).FirstOrDefaultAsync();

                return ERXTestResponse.CreateSuccessResponse(data);
            }
            catch (Exception ex)
            {
                return ERXTestResponse.CreateErrorResponse<QuestionModel>("Error At ReportList", ex);
            }
        }

        public async Task<ERXTestResults<List<Respondent>>> Report(int questionId, Guid responder)
        {
            try
            {
                var data = await _db.Respondents
                    .Where(w => questionId < 1 || w.QuestionId == questionId)
                    .Where(w => w.Responder == responder)
                    //.Select(s=> new RespondentModel
                    //{ 

                    //})
                    .AsNoTracking()
                    .ToListAsync();
                var result = ERXTestResponse.CreateSuccessResponse(data);

                return result;
            }
            catch (Exception ex)
            {
                return ERXTestResponse.CreateErrorResponse<List<Respondent>>("Error At Report", ex);
            }
        }

    }
}
