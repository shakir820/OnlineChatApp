using Microsoft.EntityFrameworkCore;
using OnlineChatApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineChatApp.Data
{
    public class OnlineChatDbContext: DbContext
    {
        public OnlineChatDbContext(DbContextOptions<OnlineChatDbContext> options)
          : base(options)
        {

        }

        public DbSet<ConversationDelete> ConversationDeletes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<ChatRoomMember> ChatRoomMembers { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<ConversationSeen> ConversationSeens { get; set; }
    }
}
