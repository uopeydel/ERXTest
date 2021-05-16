using System;
using System.Collections.Generic;
 

namespace ERXTest.Shared.Models
{
    public partial class AnswerModel
    {
        public AnswerModel()
        {
            Respondents = new HashSet<RespondentModel>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public Enums.AnswerType AnswerType { get; set; }
        public int QuestionId { get; set; }
        public int? DropDownId { get; set; }
        public bool IsRequired { get; set; }
        public string EndAnswer { get; set; }

        public virtual DropDownModel DropDown { get; set; }
        public virtual QuestionModel Question { get; set; }
        public virtual ICollection<RespondentModel> Respondents { get; set; }
    }
}
