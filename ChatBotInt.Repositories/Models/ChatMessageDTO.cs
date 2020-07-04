using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotInt.Repositories.Models
{
    /// <summary>
    /// Model for chat inn
    /// </summary>
    public class ChatMessageDTO
    {
        public ulong ChatId { get; set; }
        public Guid SessionId { get; set; }
        public DateTime MessageDateTime { get; set; }
        public string MessageContent { get; set; }
        public bool IsRead { get; set; }
        public Guid SenderUserId { get; set; }
        public bool IsGroup { get; set; }
        public Guid GroupId { get; set; }
    }
}
