using System;
using System.Collections.Generic;
 

namespace ERXTest.Shared.Models
{
    public partial class QuestionModel
    {
        public QuestionModel()
        {
            //Answers = new HashSet<AnswerModel>();
            Respondents = new HashSet<RespondentModel>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual List<AnswerModel> Answers { get; set; }
        public virtual ICollection<RespondentModel> Respondents { get; set; }
    }
}
