using System;
using System.Collections.Generic;
using System.Text;

namespace ERXTest.Shared.Models.Request
{
    public class QuestionUpsertRequest
    {
        public QuestionModel question { get; set; }
        public List<AnswerModel> answer { get; set; }
    }
}
