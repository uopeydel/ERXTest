using System;
using System.Collections.Generic;
 

namespace ERXTest.Shared.Models
{
    public partial class RespondentModel
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string QuestionName { get; set; }
        public int AnswerId { get; set; }
        public string AnswerName { get; set; }
        public string Answer { get; set; }
        public DateTime? dtPicker { get; set; }
        public Guid Responder { get; set; }

        public virtual AnswerModel AnswerNavigation { get; set; }
        public virtual QuestionModel Question { get; set; }
    }
}
