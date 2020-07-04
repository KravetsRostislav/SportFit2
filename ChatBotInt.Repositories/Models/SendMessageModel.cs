using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotInt.Repositories.Models
{
    public class SendMessageModel
    {
        public Guid SessionId { get; set; }
        public string MessageText { get; set; }
        public DateTime DateTime { get; set; }
        public Guid? UserId { get; set; }
        public string UserEmail { get; set; }
    }
}
