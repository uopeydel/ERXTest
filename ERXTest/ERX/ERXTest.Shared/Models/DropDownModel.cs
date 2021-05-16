using System;
using System.Collections.Generic;
 

namespace ERXTest.Shared.Models
{
    public partial class DropDownModel
    {
        public DropDownModel()
        {
            Answers = new HashSet<AnswerModel>();
            //DropDownItems = new HashSet<DropDownItemModel>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AnswerModel> Answers { get; set; }
        public virtual List<DropDownItemModel> DropDownItems { get; set; }
    }
}
