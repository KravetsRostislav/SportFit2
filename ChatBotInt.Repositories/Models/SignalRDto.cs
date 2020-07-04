using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotInt.Repositories.Models
{
    public class SignalRDto
    {
        public Guid SignalRConnectionId { get; set; }
        public string ConnectionID { get; set; }
        public bool Connected { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public Guid UserId { get; set; }
        public string StatusConnectionID { get; set; }
    }
}
