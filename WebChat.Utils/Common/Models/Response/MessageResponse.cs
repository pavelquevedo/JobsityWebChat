using System;
using WebChat.Utils.Common.Enum;

namespace WebChat.Utils.Common.Models.Response
{
    public class MessageResponse
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int UserID { get; set; }
        public string UserFullName { get; set; }
        public DateTime CreationDate { get; set; }

        public MessageType MessageType { get; set; } 
    }
}
