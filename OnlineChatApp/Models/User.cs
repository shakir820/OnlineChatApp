using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineChatApp.Models
{
    public class User
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string NormalizedFirstName { get; set; }
        public string NormalizedLastName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public byte[] ProfilePic { get; set; }
        public string ConnectionId { get; set; }
    }
}
