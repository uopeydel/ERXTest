using System;
using System.Collections.Generic;
using System.Text;

namespace ERXTest.Shared.Models
{
    public class LogDesc
    {
        public string Uuid { get; set; }
        public string RemoteIpAddress { get; set; }
        public string BaseApiUrl { get; set; }
        public string IpConnection { get; set; }
        public string Pid { get; set; }
        public string AppName { get; set; }
        public string Host { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
        public string RequestTime { get; set; }
        public string ResponseTime { get; set; }
        public string ThreadName { get; set; }  
        public string Jti { get; set; }
        public string ClassName { get; set; }
        public string IsFromClient { get; set; }
    }
}
