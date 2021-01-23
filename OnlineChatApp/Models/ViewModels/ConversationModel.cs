using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineChatApp.Models.ViewModels
{
    public class ConversationModel
    {
        public long id { get; set; }
        public long chat_room_id { get; set; }
        public UserModel sender { get; set; }
        public string message { get; set; }
        public DateTime created_date { get; set; }
        public bool is_seen { get; set; }
    }
}
