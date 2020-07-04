using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotInt.Repositories.Models
{
    public class ChatMessageDto
    {
        public Guid SessionId { get; set; }
        public string FullName { get; set; }
        public string OperatorName { get; set; }
        public string MessageText { get; set; }
        public bool Direction { get; set; }
        public bool IsVip { get; set; }
        public DateTime DateTime { get; set; }
        public string Channel { get; set; }
    }
}
