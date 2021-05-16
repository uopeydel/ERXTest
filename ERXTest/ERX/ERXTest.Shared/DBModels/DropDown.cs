using System;
using System.Collections.Generic;
 

namespace ERXTest.Shared.DBModels
{
    public partial class DropDown
    {
        public DropDown()
        {
            Answers = new HashSet<Answer>();
            DropDownItems = new HashSet<DropDownItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<DropDownItem> DropDownItems { get; set; }
    }
}
