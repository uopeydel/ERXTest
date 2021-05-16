using Blazored.Modal;
using Blazored.Modal.Services;
using ERXTest.BZ.Helpers;
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
    public partial class Index
    {
        [CascadingParameter]
        BlazoredModalInstance ModalInstance { get; set; }

        [CascadingParameter]
        public IModalService Modal { get; set; }

        public List<QuestionModel> questions { get; set; }
        protected override async Task OnInitializedAsync()
        { 
            questions = new List<QuestionModel>();
            await RespondentService.ValidRespondentGuid();

            var respondentGuid = await LocalStorageService.GetItem<string>("RespondentGuid");

            var questionResults = await QuestionService.GetQuestionList(Guid.Parse(respondentGuid));
            if (questionResults.Error)
            {
                Modal.Show<AlertModal>("Error while load question list.");
            }
            else
            {
                questions = questionResults.Data;
            }
        }

       

        public async Task DoExamination(int questionId)
        {
            NavigationManager.NavigateTo($"/Examination/{questionId}", false); 
        }
    }
}
