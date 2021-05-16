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
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Data.SqlClient;
using ERXTest.Shared.DBModels;

namespace ERXTest.Server.Repositories
{
    public interface IQuestionRepository
    {
        Task<ERXTestResults<List<QuestionModel>>> GetQuestionList(Guid? respondentGuid);
        Task<ERXTestResults<QuestionModel>> GetQuestionById(int id, Guid? respondentGuid);
        Task<ERXTestResults<bool>> Upsert(Question question, List<Answer> answer);
        Task<ERXTestResults<bool>> Delete(int questionId);
        Task<ERXTestResults<bool>> IsUserTouchEndAnswer(Guid? respondentGuid);
    }

    public class QuestionRepository : IQuestionRepository
    {
        private readonly ILoggerService _logger;
        private readonly ERXTestContext _db;
        public DateTime now;
        public QuestionRepository(ERXTestContext db, ILoggerService logger)
        {
            _logger = logger;
            _db = db;
            now = DateTime.UtcNow.AddHours(7);
        }

        public async Task<ERXTestResults<bool>> IsUserTouchEndAnswer(Guid? respondentGuid)
        {
            try
            {
                if (respondentGuid == null)
                {
                    return ERXTestResponse.CreateSuccessResponse(false);
                }

                var answ = await _db.Respondents.Where(w => w.Responder == respondentGuid)
                    .Where(w => !string.IsNullOrEmpty(w.AnswerNavigation.EndAnswer) && !string.IsNullOrEmpty(w.Answer))
                    .Select(s => new
                    {
                        EndAnswer = s.AnswerNavigation.EndAnswer,
                        Answer = s.Answer
                    }).ToListAsync();

                var isEndFlag = answ.Where(f =>
                {
                    var ends = f.EndAnswer.Split(",").ToList();
                    var isEnd = ends.Any(a => a.Trim().ToLower().Equals(f.Answer.Trim().ToLower()));

                    return isEnd;
                }).Any();

                return ERXTestResponse.CreateSuccessResponse(isEndFlag);
            }
            catch (Exception ex)
            {
                return ERXTestResponse.CreateErrorResponse<bool>("Error at check end answer", ex);
            }
        }

        public async Task<ERXTestResults<List<QuestionModel>>> GetQuestionList(Guid? respondentGuid)
        {
            try
            {
                var data = await _db.Questions
                    .Where(w => respondentGuid == null || !w.Respondents.Any(a => a.Responder == respondentGuid))
                    .Select(s => new QuestionModel
                    {
                        Id = s.Id,
                        Name = s.Name
                    }).ToListAsync();
                var result = ERXTestResponse.CreateSuccessResponse(data);

                return result;
            }
            catch (Exception ex)
            {
                return ERXTestResponse.CreateErrorResponse<List<QuestionModel>>("Error At GetQuestionList", ex);
            }
        }

        public async Task<ERXTestResults<QuestionModel>> GetQuestionById(int id, Guid? respondentGuid)
        {
            try
            {
                var data = await _db.Questions.Where(w => w.Id == id)
                    .Where(w => respondentGuid == null || !w.Respondents.Any(a => a.Responder == respondentGuid))
                    .Select(s => new QuestionModel
                    {
                        Id = s.Id,
                        Answers = s.Answers.Select(ss => new AnswerModel
                        {
                            AnswerType = (Enums.AnswerType)ss.AnswerType,
                            Id = ss.Id,
                            DropDownId = ss.DropDownId,
                            QuestionId = ss.QuestionId,
                            Name = ss.Name,
                            IsRequired = ss.IsRequired,
                            EndAnswer = ss.EndAnswer,
                            DropDown = ss.DropDown == null ? null : new DropDownModel
                            {
                                Id = ss.DropDown.Id,
                                Name = ss.DropDown.Name,
                                DropDownItems = ss.DropDown.DropDownItems.Select(sss => new DropDownItemModel
                                {
                                    DropDownId = sss.DropDownId,
                                    Id = sss.Id,
                                    Name = sss.Name
                                }).ToList()
                            }
                        }).ToList(),
                        Name = s.Name
                    }).FirstOrDefaultAsync();
                var result = ERXTestResponse.CreateSuccessResponse(data);

                return result;
            }
            catch (Exception ex)
            {
                return ERXTestResponse.CreateErrorResponse<QuestionModel>("Error At GetQuestionById", ex);
            }
        }

        public async Task<ERXTestResults<bool>> Upsert(Question question, List<Answer> answer)
        {
            IDbContextTransaction tran = null;
            try
            {
                tran = await _db.Database.BeginTransactionAsync();
                if (question.Id > 0)
                {
                    await _db.Database.ExecuteSqlRawAsync(@" 
UPDATE Question SET Name = @qName WHERE Id = @qId",
                        new SqlParameter("@qId", question.Id),
                        new SqlParameter("@qName", question.Name));

                    var data = await _db.Questions.FromSqlRaw(@"
SELECT TOP 1 * FROM Question WHERE Id = @qId
",
        new SqlParameter("@qId", question.Id),
        new SqlParameter("@qName", question.Name)).FirstOrDefaultAsync();

                    if (data == null)
                    {
                        return ERXTestResponse.CreateErrorResponse<bool>("Error At Upsert, While update name");
                    }
                    if (data.Name != question.Name)
                    {
                        return ERXTestResponse.CreateErrorResponse<bool>("Error At Upsert, Name not update");
                    }
                }
                else
                {
                    await _db.Questions.AddAsync(question);
                    await _db.SaveChangesAsync();
                }
                await _db.Database.ExecuteSqlRawAsync(@" 
DELETE Answer WHERE QuestionId = @qId",
                    new SqlParameter("@qId", question.Id));

                var deleted = await _db.Answers.FromSqlRaw(@" 
SELECT TOP 1 * FROM Answer WHERE QuestionId = @qId
",
                new SqlParameter("@qId", question.Id)).FirstOrDefaultAsync();
                if (deleted != null)
                {
                    await tran.RollbackAsync();
                    return ERXTestResponse.CreateErrorResponse<bool>("Error At Upsert, While refresh Answers");
                }
                await _db.SaveChangesAsync();

                answer.ForEach(f => { f.QuestionId = question.Id; f.Id = 0; f.DropDown = null; });
                await _db.Answers.AddRangeAsync(answer);

                await _db.SaveChangesAsync();

                await tran.CommitAsync();

                return ERXTestResponse.CreateSuccessResponse(true);
            }
            catch (Exception ex)
            {
                await tran.RollbackAsync();
                return ERXTestResponse.CreateErrorResponse<bool>("Error At Upsert", ex);
            }
        }


        public async Task<ERXTestResults<bool>> Delete(int questionId)
        {
            IDbContextTransaction tran = null;
            try
            {
                tran = await _db.Database.BeginTransactionAsync();

                await _db.Database.ExecuteSqlRawAsync(@" 
DELETE Answer WHERE QuestionId = @qId
DELETE Question WHERE Id = @qId",
                    new SqlParameter("@qId", questionId));


                var deletedQuestion = await _db.Questions.FromSqlRaw(@" 
SELECT TOP 1 * FROM Question WHERE Id = @qId",
                new SqlParameter("@qId", questionId)).FirstOrDefaultAsync();
                if (deletedQuestion != null)
                {
                    await tran.RollbackAsync();
                    return ERXTestResponse.CreateErrorResponse<bool>("Error At delete question");
                }

                var deletedAnswer = await _db.Answers.FromSqlRaw(@" 
SELECT TOP 1 * FROM Answer WHERE QuestionId = @qId",
new SqlParameter("@qId", questionId)).FirstOrDefaultAsync();
                if (deletedAnswer != null)
                {
                    await tran.RollbackAsync();
                    return ERXTestResponse.CreateErrorResponse<bool>("Error At delete Answers");
                }
                await _db.SaveChangesAsync();

                await tran.CommitAsync();

                return ERXTestResponse.CreateSuccessResponse(true);
            }
            catch (Exception ex)
            {
                await tran.RollbackAsync();
                return ERXTestResponse.CreateErrorResponse<bool>("Error At Upsert", ex);
            }
        }


    }
}
