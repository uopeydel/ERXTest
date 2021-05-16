using System;
using System.Collections.Generic;
 

namespace ERXTest.Shared.DBModels
{
    public partial class Answer
    {
        public Answer()
        {
            Respondents = new HashSet<Respondent>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int AnswerType { get; set; }
        public int QuestionId { get; set; }
        public int? DropDownId { get; set; }
        public bool IsRequired { get; set; }
        public string EndAnswer { get; set; }

        public virtual DropDown DropDown { get; set; }
        public virtual Question Question { get; set; }
        public virtual ICollection<Respondent> Respondents { get; set; }
    }
}
