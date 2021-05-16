using System;
using System.Collections.Generic;
using System.Text;

namespace ERXTest.Shared.Models
{
    public class XLSObject
    {
        public List<string> Phonenumber { get; set; }
        public int QuestionNameID { get; set; }
        public string QuestionName { get; set; }
        public string Message { get; set; }
        public DateTime? DateTimeSend { get; set; }

    }
}
