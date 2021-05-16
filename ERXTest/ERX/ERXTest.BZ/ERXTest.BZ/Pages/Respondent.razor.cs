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
    public partial class Respondent
    {
        [CascadingParameter]
        BlazoredModalInstance ModalInstance { get; set; }

        [CascadingParameter]
        public IModalService Modal { get; set; }

        private List<RespondentModel> dataToDownload { get; set; }
        public List<Guid> responders { get; set; }
        protected override async Task OnInitializedAsync()
        {
            dataToDownload = new List<RespondentModel>();
            responders = new List<Guid>();
            await RespondentsList();
        }

        public async Task RequestReport(Guid respondentGuid)
        {
            var dataResult = await RespondentService.Report(0, respondentGuid);
            if (dataResult.Error)
            {
                Modal.Show<AlertModal>(dataResult.Message.FirstOrDefault());
                return;
            }
            dataToDownload = dataResult.Data;
            if (dataToDownload == null)
            { dataToDownload = new List<RespondentModel>(); }

            await DownloadFile(respondentGuid);
        }

        private string ReplaceForCSV(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                return word;
            }
            return word.Replace(",", "").Replace("'", "").Replace("\"", "");
        }

        public async Task DownloadFile(Guid respondentGuid)
        {
            var text = "QuestionName,AnswerName,Answer,Responder\n";

            for (int i = 0; i < dataToDownload.Count; i++)
            {
                var QuestionName = ReplaceForCSV(dataToDownload[i].QuestionName);
                var AnswerName = ReplaceForCSV(dataToDownload[i].AnswerName);
                var Answer = ReplaceForCSV(dataToDownload[i].Answer);
                var Responder = ReplaceForCSV(dataToDownload[i].Responder.ToString());
                text += $"{QuestionName} , {AnswerName} , {Answer} , {Responder} \n";

            }
            var bytes = System.Text.Encoding.UTF8.GetBytes(text);
            await FileUtil.SaveAs(js, $"{respondentGuid.ToString()}.csv", bytes);
        }

        public async Task RespondentsList()
        {
            var listOfRespResult = await RespondentService.RespondentsList();
            if (listOfRespResult.Error)
            {
                Modal.Show<AlertModal>(listOfRespResult.Message.FirstOrDefault());
                return;
            }
            responders = listOfRespResult.Data;
        }

    }
}
