using System;
using System.Collections.Generic;
 

namespace ERXTest.Shared.DBModels
{
    public partial class DropDownItem
    {
        public int Id { get; set; }
        public int DropDownId { get; set; }
        public string Name { get; set; }

        public virtual DropDown DropDown { get; set; }
    }
}
