using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotInt.Repositories.Models
{
    public class ReturnBrancheITRModel
    {
        public Guid SessionId { get; set; }
        public Guid MenuItemId { get; set; }
        public Guid? UserId { get; set; }
        public string AutoText { get; set; }
    }
}
