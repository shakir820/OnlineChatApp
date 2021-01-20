using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineChatApp.Models.ViewModels
{
    public class UserModel
    {
        public long id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public IFormFile profile_pic { get; set; }

        public ChatRoomType chat_room_type { get; set; } = ChatRoomType.OneToOne;
    }
}
