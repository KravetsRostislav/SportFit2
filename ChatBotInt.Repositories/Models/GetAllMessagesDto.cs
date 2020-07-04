using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotInt.Repositories.Models
{
    public class GetAllMessagesDto
    {
        public DateTime DateTime { get; set; }
        public Guid SessionId { get; set; }
        public string MessageText { get; set; }
        public string Direction { get; set; }
    }
}
