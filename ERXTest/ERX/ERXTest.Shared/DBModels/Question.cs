using System;
using System.Collections.Generic;
 

namespace ERXTest.Shared.DBModels
{
    public partial class Question
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
            Respondents = new HashSet<Respondent>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<Respondent> Respondents { get; set; }
    }
}
