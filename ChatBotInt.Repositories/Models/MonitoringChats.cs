using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotInt.Repositories.Models
{
    public class MonitoringChats
    {
        public Guid SessionId { get; set; }
        public string Client { get; set; }
        public string Channel { get; set; }
        public string Status { get; set; }
        public int WaitingTime { get; set; }
        public int Duration { get; set; }
        public DateTime SessionStart { get; set; }
    }
}
