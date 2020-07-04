using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotInt.Repositories.Models
{
    public class CrmClientDto
    {
        public string Source { get; set; }
        public string FullName { get; set; }
        public string Segment { get; set; }
        public string ClientNumber { get; set; }
        public string Branch { get; set; }
        public string Phone { get; set; }
        public bool FinPhone { get; set; }
        public string LastService { get; set; }
        public string RBS_num { get; set; }
        public DateTime Date_start { get; set; }

        public DateTime Date_operator_start { get; set; }
        public bool IsVip { get; set; }
        public string Queue { get; set; }
    }
}
