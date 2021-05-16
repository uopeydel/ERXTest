using Blazored.Modal;
using Blazored.Modal.Services;
using ERXTest.BZ.Services;
using ERXTest.Shared.Models;
using ERXTest.Shared.Models.Request;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ERXTest.BZ.Pages
{
    public partial class Examination
    {
        [CascadingParameter]
        BlazoredModalInstance ModalInstance { get; set; }

        [CascadingParameter]
        public IModalService Modal { get; set; }
        [Parameter]
        public string questionId { get; set; }

        public QuestionModel question { get; set; }

        public List<RespondentModel> answeredTemp { get; set; }

        string respondentGuid { get; set; }

        protected override async Task OnInitializedAsync()
        {
            question = new QuestionModel();
            question.Answers = new List<AnswerModel>();
            await RespondentService.ValidRespondentGuid();
            respondentGuid = await LocalStorageService.GetItem<string>("RespondentGuid");
            var loadQuestionSuccess = await LoadQuestionById();
            if (loadQuestionSuccess)
            {
                await LoadAnsweredTemp();
            }
        }



        public async Task<bool> LoadQuestionById()
        {
            var isInt = int.TryParse(questionId, out int qId);
            if (!isInt)
            {
                question = new QuestionModel();
                question.Answers = new List<AnswerModel>();
                Modal.Show<AlertModal>("Invalid question");
                return false;
            }
            var questionResult = await QuestionService.GetQuestionById(qId, Guid.Parse(respondentGuid));
            if (questionResult.Error)
            {
                question = new QuestionModel();
                question.Answers = new List<AnswerModel>();
                Modal.Show<AlertModal>(questionResult.Message.FirstOrDefault());
                return false;
            }

            question = questionResult.Data;
            if (question == null)
            {
                Modal.Show<AlertModal>("Have no question to answer.");
                NavigationManager.NavigateTo($"/", false);
                return false;
            }
            return true;
        }

        public async Task LoadAnsweredTemp()
        {
            int.TryParse(questionId, out int qId);
            answeredTemp = await LocalStorageService.GetItem<List<RespondentModel>>($"AnsweredTemp{questionId}");
            if (answeredTemp == null || !answeredTemp.Any() || answeredTemp.Count != question.Answers.Count)
            {
                answeredTemp = question.Answers.OrderBy(o => o.Id).Select(s => new RespondentModel
                {
                    AnswerId = s.Id,
                    AnswerName = s.Name,
                    QuestionId = qId,
                    QuestionName = question.Name,
                    Responder = Guid.Parse(respondentGuid)
                }).ToList();
            }
            await StartCountdown();
        }

        public async Task SaveAnsweredTemp()
        {
            await LocalStorageService.SetItem<List<RespondentModel>>($"AnsweredTemp{questionId}", answeredTemp);
        }

        public async Task SaveAnswered()
        {
            var message = "Answer saved.";
            var endFlgAnswer = question
                .Answers
                .Where(w => !string.IsNullOrEmpty(w.EndAnswer))
                .Select(s => new { EndAns = s.EndAnswer.Split(",").ToList(), s.Id }).ToList();

            answeredTemp.ForEach(f =>
            {
                var ansType = question.Answers.Where(w => w.Id == f.AnswerId).Select(s => s.AnswerType).FirstOrDefault();
                if (ansType == Enums.AnswerType.DateTime)
                {
                    f.Answer = f.dtPicker.ToString();
                }
                f.Responder = Guid.Parse(respondentGuid);

                var isEnd = endFlgAnswer.Where(w => w.Id == f.AnswerId)
                .Where(w => w.EndAns.Any(a => a.Trim().ToLower().Equals(f.Answer.Trim().ToLower()))).Any();
                if (isEnd)
                {
                    message = "Questionnaire is done.";
                }
            });
            var answ = new RespondentCreateRequest
            {
                respondent = answeredTemp,
            };
            var saveResult = await RespondentService.Create(answ);
            if (saveResult.Error)
            {
                Modal.Show<AlertModal>(saveResult.Message.FirstOrDefault());
            }
            else
            {
                await LocalStorageService.RemoveItem($"AnsweredTemp{questionId}");
                Modal.Show<AlertModal>(message);
                NavigationManager.NavigateTo($"/", false);
            }
        }

        void OnDatetimePickerChange(DateTime? value, string name, string format)
        {
            int.TryParse(name, out int answerId);
            //answeredTemp.Where(f => f.AnswerId == answerId).FirstOrDefault().Answer = value?.ToString(format);
            Console.WriteLine($"{name} value changed to {value?.ToString(format)}");
        }


        private async Task StartCountdown()
        {
            var timer = new Timer(new TimerCallback(async _ =>
            {
                await SaveAnsweredTemp();
            }), null, 10000, 10000);
        }
    }
}
