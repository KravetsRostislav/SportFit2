using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotInt.Repositories.Models
{
    public class MessageForSessionModel
    {
        public string SessionId { get; set; }
        public string FullName { get; set; }
        public string MessageText { get; set; }
        public bool? Direction { get; set; }
        public bool? IsVip { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime StartOperatorSession { get; set; }
        public string Chanel { get; set; }
    }
}
