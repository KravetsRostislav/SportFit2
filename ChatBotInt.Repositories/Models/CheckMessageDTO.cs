using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotInt.Repositories.Models
{
    public class CheckMessageDTO
    {
        public string ConnectionId { get; set; }
        public string StatusConnectionID { get; set; }
        public bool IsNewMessage { get; set; }
        public Guid UserId { get; set; }
    }
}
