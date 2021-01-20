using OnlineChatApp.Models;
using OnlineChatApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineChatApp.Helper
{
    public class ModelBindingResolver
    {
        public static UserModel ResolveUser(User db_user)
        {
            if(db_user == null)
            {
                return null;
            }
            var user = new UserModel();
            user.email = db_user.Email;
            user.first_name = db_user.FirstName;
            user.id = db_user.Id;
            user.last_name = db_user.LastName;

            return user;
        }




        public static ChatRoomModel ResolveChatRoom(ChatRoom chatRoom)
        {
            var cr = new ChatRoomModel();
            cr.created_date = chatRoom.CreatedDate;
            cr.id = chatRoom.Id;
            cr.is_group = chatRoom.IsGroup;
            cr.name = chatRoom.Name;

            return cr;
        }
    }
}
