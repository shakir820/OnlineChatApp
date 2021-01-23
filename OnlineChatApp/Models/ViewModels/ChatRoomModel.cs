using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineChatApp.Models.ViewModels
{
    public class ChatRoomModel
    {
        public long id { get; set; }
        public bool is_group { get; set; } = false;
        public string name { get; set; }
        public DateTime created_date { get; set; }
        public List<ConversationModel> conversation_list { get; set; }
        public List<UserModel> member_list { get; set; }
        public ChatRoomType chat_room_type { get; set; } = ChatRoomType.OneToOne;
        public UserModel user { get; set; }
    }
}
