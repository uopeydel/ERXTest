using ERXTest.Shared.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERXTest.Shared.Models.Request
{
    public class DropDownUpsertRequest
    {
        public DropDownModel dropDown { get; set; }

        public List<DropDownItemModel> dropDownItem { get; set; }
    }
}
