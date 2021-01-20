using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineChatApp.Models
{
    public class ChatRoom
    {
        public long Id { get; set; }
        public bool IsGroup { get; set; } = false;
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class ChatRoomMember
    {
        public long Id { get; set; }
        public long ChatRoomId { get; set; }
        public long MemeberId { get; set; }
    }

    public class Conversation
    {
        public long Id { get; set; }
        public long ChatRoomId { get; set; }
        public long SenderId { get; set; }

        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
    }


    public class ConversationDelete
    {
        public long Id { get; set; }
        public long ChatRoomId { get; set; }
        public long ConversationId { get; set; }
        public long DeletedBy { get; set; }
        public bool ForAll { get; set; } = false;
    }

    public class ConversationSeen
    {
        public long Id { get; set; }
        public long ConversationId { get; set; }
        public long SeenUserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }



}
