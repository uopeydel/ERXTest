using System;
using System.Collections.Generic;
 

namespace ERXTest.Shared.DBModels
{
    public partial class Respondent
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string QuestionName { get; set; }
        public int AnswerId { get; set; }
        public string AnswerName { get; set; }
        public string Answer { get; set; }
        public Guid Responder { get; set; }

        public virtual Answer AnswerNavigation { get; set; }
        public virtual Question Question { get; set; }
    }
}
