using Blazored.Modal;
using Blazored.Modal.Services;
using ERXTest.BZ.Services;
using ERXTest.Shared.Models;
using ERXTest.Shared.Models.Request;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERXTest.BZ.Pages
{
    public partial class ModifyExamination
    {

        [CascadingParameter]
        BlazoredModalInstance ModalInstance { get; set; }

        [CascadingParameter]
        public IModalService Modal { get; set; }

        public List<QuestionModel> QuestionList { get; set; }
        public QuestionModel QuestionData { get; set; }
        public AnswerModel AnswerTemp { get; set; }
        public List<DropDownModel> DropDownList { get; set; }
        public int AnswerIndexIsModifing { get; set; }

        protected override async Task OnInitializedAsync()
        {
            AnswerIndexIsModifing = -1;
            AnswerTemp = new AnswerModel();
            QuestionList = new List<QuestionModel>();
            QuestionData = new QuestionModel();
            QuestionData.Answers = new List<AnswerModel>();
            await GetList();
            await GetListDropDown();
        }

        public async Task GetList()
        {
            var listResult = await QuestionService.GetQuestionList(null);
            if (listResult.Error)
            {
                var formModal = Modal.Show<AlertModal>(listResult.Message.FirstOrDefault());
                return;
            }

            QuestionList = listResult.Data;
        }

        public async Task Clear()
        {
            AnswerIndexIsModifing = -1;
            QuestionData = new QuestionModel();
            QuestionData.Answers = new List<AnswerModel>();
        }
        public async Task Upsert()
        {
            QuestionData.Answers.ForEach(f =>
            {
                if (f.AnswerType != Enums.AnswerType.DropDown)
                {
                    f.DropDownId = null;
                }
                f.DropDown = null;
            });

            QuestionUpsertRequest data = new QuestionUpsertRequest
            {
                question = QuestionData,
                answer = QuestionData.Answers,
            };
            var listResult = await QuestionService.Upsert(data);
            if (listResult.Error)
            {
                var formModal = Modal.Show<AlertModal>(listResult.Message.FirstOrDefault());
                return;
            }

            Modal.Show<AlertModal>("Save success.");
            await GetList();

            var IdToRefresh = 0;
            if (data.question.Id > 0)
            {
                IdToRefresh = data.question.Id;
            }
            else
            {
                IdToRefresh = QuestionList.OrderByDescending(o => o.Id).Select(s => s.Id).FirstOrDefault();
            }

            await GetQuestionById(IdToRefresh);
        }
        public async Task GetQuestionById(int questionId)
        {
            var itemResult = await QuestionService.GetQuestionById(questionId,null);
            if (itemResult.Error)
            {
                QuestionData = new QuestionModel();
                var formModal = Modal.Show<AlertModal>(itemResult.Message.FirstOrDefault());
                return;
            }

            QuestionData = itemResult.Data;
        }


        public async Task Delete(int questionId)
        {
            var itemResult = await QuestionService.Delete(questionId);
            if (itemResult.Error)
            {
                QuestionData = new QuestionModel();
                var formModal = Modal.Show<AlertModal>(itemResult.Message.FirstOrDefault());
                return;
            }
            await GetList();
        }


        public async Task AddToList()
        {
            if (AnswerIndexIsModifing > -1)
            {
                QuestionData.Answers[AnswerIndexIsModifing] = AnswerTemp;
            }
            else
            {
                QuestionData.Answers.Add(AnswerTemp);
            }
            AnswerTemp = new AnswerModel();


            AnswerIndexIsModifing = -1;
        }


        public async Task RemoveFromList(AnswerModel item)
        {
            QuestionData.Answers.Remove(item);
        }

        public async Task GetListDropDown()
        {
            var listResult = await DropDownService.List();
            if (listResult.Error)
            {
                var formModal = Modal.Show<AlertModal>(listResult.Message.FirstOrDefault());
                return;
            }

            DropDownList = listResult.Data;
        }

        public async Task ModifyThisAnswer(int index)
        {
            AnswerIndexIsModifing = index;
            AnswerTemp = QuestionData.Answers[index];
        }
    }


}
