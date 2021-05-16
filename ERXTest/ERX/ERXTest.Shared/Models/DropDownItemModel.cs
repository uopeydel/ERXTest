using System;
using System.Collections.Generic;
 

namespace ERXTest.Shared.Models
{
    public partial class DropDownItemModel
    {
        public int Id { get; set; }
        public int DropDownId { get; set; }
        public string Name { get; set; }

        public virtual DropDownModel DropDown { get; set; }
    }
}
