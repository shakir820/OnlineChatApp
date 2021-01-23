using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OnlineChatApp.Data;
using OnlineChatApp.Helper;
using OnlineChatApp.Hubs;
using OnlineChatApp.Models;
using OnlineChatApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineChatApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        OnlineChatDbContext _context;
        IHubContext<ChatHub> ChatHub;

        public MessageController(OnlineChatDbContext context, IHubContext<ChatHub> hub)
        {
            _context = context;
            ChatHub = hub;
        }




        public async Task<IActionResult> GetAllChatRooms(long user_id)
        {
            try
            {

                var db_chat_rooms = await _context.ChatRooms.Join(_context.ChatRoomMembers, o => o.Id, i => i.ChatRoomId, 
                    (o, i) => new
                {
                    cr = o,
                    m = i
                }).AsNoTracking().Where(a => a.m.MemeberId == user_id).ToListAsync();


                var chat_room_list = new List<ChatRoomModel>();

                foreach (var item in db_chat_rooms)
                {
                    var db_chat_memebers = await _context.ChatRoomMembers.Join(_context.Users, o => o.MemeberId, i => i.Id, (o, i) => new
                    {
                        crm = o,
                        u = i
                    }).AsNoTracking().Where(a => a.crm.ChatRoomId == item.cr.Id ).ToListAsync();

                    var cr = item.cr;

                    

                    var chat_room = ModelBindingResolver.ResolveChatRoom(cr);
                 
                    if (cr.ChatRoomType == ChatRoomType.OneToOne)
                    {
                        var receiver = db_chat_memebers.FirstOrDefault(a => a.u.Id != user_id).u;
                        chat_room.name = receiver.FirstName + " " + receiver.LastName;
                    }

                    chat_room.member_list = new List<UserModel>();
                    chat_room.conversation_list = new List<ConversationModel>();

                    foreach(var u_item in db_chat_memebers)
                    {
                        var user = ModelBindingResolver.ResolveUser(u_item.u);
                        chat_room.member_list.Add(user);
                    }


                    chat_room_list.Add(chat_room);
                }

                return new JsonResult(new
                {
                    success = true,
                    error = false,
                    chat_room_list
                });
            }
            catch(Exception ex)
            {
                return new JsonResult(new
                {
                    success = false,
                    error = true,
                    error_msg = ex.Message
                });
            }

           
        }




        public async Task<IActionResult> GetChatRoom(long sender_id, long receiver_id)
        {

            try
            {
                var db_s_crms = await _context.ChatRoomMembers.AsNoTracking().Where(a => a.MemeberId == sender_id).ToListAsync();
                var db_r_crms = await _context.ChatRoomMembers.AsNoTracking().Where(a => a.MemeberId == receiver_id).ToListAsync();

                if(db_r_crms.Count == 0 || db_s_crms.Count == 0)
                {
                    return new JsonResult(new
                    {
                        success = true,
                        error = false,

                    });
                }


                long? chatRoomId = null;
                foreach (var item in db_s_crms)
                {
                    var db_r_crm = db_r_crms.FirstOrDefault(a => a.ChatRoomId == item.ChatRoomId);
                    if (db_r_crm != null)
                    {
                        var cr = await _context.ChatRooms.FirstOrDefaultAsync(a => a.Id == db_r_crm.ChatRoomId);
                        if (cr.ChatRoomType == ChatRoomType.OneToOne)
                        {
                            chatRoomId = db_r_crm.ChatRoomId;
                            break;
                        }

                    }
                }

                if (chatRoomId != null)
                {
                    var db_chat_room = await _context.ChatRooms.AsNoTracking().FirstOrDefaultAsync(a => a.Id == chatRoomId);
                    var chat_room = ModelBindingResolver.ResolveChatRoom(db_chat_room);
                    chat_room.member_list = new List<UserModel>();

                    var db_crmu_lists = await _context.ChatRoomMembers.Join(_context.Users, o => o.MemeberId, i => i.Id, (o, i) => new { member = o, user = i }).AsNoTracking().
                        Where(a => a.member.ChatRoomId == chat_room.id).ToListAsync();

                    foreach(var item in db_crmu_lists)
                    {
                        var u = ModelBindingResolver.ResolveUser(item.user);
                        chat_room.member_list.Add(u);
                    }

                    
                    return new JsonResult(new
                    {
                        success = true,
                        error = false,
                        chat_room
                    });
                }
                else
                {
                    //using (var transaction = await _context.Database.BeginTransactionAsync())
                    //{
                    //    try
                    //    {
                    //        //create a new chat room
                    //        var cr = new ChatRoom();
                    //        cr.CreatedDate = DateTime.Now;
                    //        cr.IsGroup = false;
                    //        cr.Name = "";
                    //        _context.ChatRooms.Add(cr);
                    //        await _context.SaveChangesAsync();


                    //        //sender
                    //        var crm = new ChatRoomMember();
                    //        crm.ChatRoomId = cr.Id;
                    //        crm.MemeberId = sender_id;

                    //        _context.ChatRoomMembers.Add(crm);
                    //        await _context.SaveChangesAsync();


                    //        //receiver
                    //        crm = new ChatRoomMember();
                    //        crm.ChatRoomId = cr.Id;
                    //        crm.MemeberId = receiver_id;

                    //        _context.ChatRoomMembers.Add(crm);
                    //        await _context.SaveChangesAsync();


                    //        await transaction.CommitAsync();

                    //        var chat_room = ModelBindingResolver.ResolveChatRoom(cr);

                    //        return new JsonResult(new
                    //        {
                    //            success = true,
                    //            error = false,
                    //            chat_room
                    //        });
                    //    }
                    //    catch (Exception ex2)
                    //    {
                    //        await transaction.RollbackAsync();

                    //        return new JsonResult(new
                    //        {
                    //            success = false,
                    //            error = true,
                    //            error_msg = ex2.Message
                    //        });
                    //    }
                    //}


                    return new JsonResult(new
                    {
                        success = true,
                        error = false
                    });

                }
            }
            catch(Exception ex)
            {
                return new JsonResult(new
                {
                    success = false,
                    error = true,
                    error_msg = ex.Message
                });
            }

        }










        public async Task<IActionResult> GetConversationListByChatRoom(long chat_room_id, long user_id)
        {
            try
            {
                var db_cl = await _context.Conversations.AsNoTracking().Where(a => a.ChatRoomId == chat_room_id).ToListAsync();
                var conversation_list = new List<ConversationModel>();

                foreach(var item in db_cl)
                {
                    var dc = await _context.ConversationDeletes.AsNoTracking().FirstOrDefaultAsync(a => a.ChatRoomId == chat_room_id && a.DeletedBy == user_id && a.ConversationId == item.Id);
                    if(dc == null)
                    {
                        var c = ModelBindingResolver.ResolveConversation(item);
                        var db_sender = await _context.Users.AsNoTracking().FirstOrDefaultAsync(a => a.Id == item.SenderId);
                        c.sender = ModelBindingResolver.ResolveUser(db_sender);
                        conversation_list.Add(c);
                    }
                }

                return new JsonResult(new
                {
                    success = true,
                    error = false,
                    conversation_list
                });
            }
            catch(Exception ex)
            {
                return new JsonResult(new
                {
                    success = false,
                    error = true,
                    error_msg = ex.Message
                });
            }
        }







        [HttpPost]
        public async Task<IActionResult> SendMessage(PostMethodData post_data)
        {
            using(var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var cr_model = JsonConvert.DeserializeObject<ChatRoomModel>(post_data.json_data);

                    ChatRoom db_chat_room = await _context.ChatRooms.FirstOrDefaultAsync(a => a.Id == cr_model.id);


                    //create new chat room if doesn't exist
                    if(db_chat_room == null)
                    {
                        db_chat_room = new ChatRoom();
                        db_chat_room.ChatRoomType = ChatRoomType.OneToOne;
                        db_chat_room.CreatedDate = DateTime.Now;
                        db_chat_room.IsGroup = false;

                        _context.ChatRooms.Add(db_chat_room);
                        await _context.SaveChangesAsync();

                        foreach(var item in cr_model.member_list)
                        {
                            var m = new ChatRoomMember();
                            m.ChatRoomId = db_chat_room.Id;
                            m.MemeberId = item.id;

                            _context.ChatRoomMembers.Add(m);
                            await _context.SaveChangesAsync();
                        }
                    }


                    var conv = new Conversation();
                    conv.ChatRoomId = db_chat_room.Id;
                    conv.CreatedDate = DateTime.Now;
                    conv.Message = cr_model.conversation_list[0].message;
                    conv.SenderId = cr_model.conversation_list[0].sender.id;

                    _context.Conversations.Add(conv);
                    await _context.SaveChangesAsync();




                    await transaction.CommitAsync();

                    var chat_room = ModelBindingResolver.ResolveChatRoom(db_chat_room);

                    chat_room.conversation_list = new List<ConversationModel>();
                    var con_model = ModelBindingResolver.ResolveConversation(conv);
                    con_model.sender = cr_model.conversation_list[0].sender;

                    chat_room.conversation_list.Add(con_model);
                    chat_room.member_list = cr_model.member_list;


                    foreach(var item in cr_model.member_list)
                    {
                        if(item.id == conv.SenderId)
                        {
                            continue;
                        }

                        var db_user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(a => a.Id == item.id);
                        if(db_user.ConnectionId != null)
                        {
                            _ = ChatHub.Clients.Client(db_user.ConnectionId).SendAsync("receive_msg", chat_room);
                        }
                        
                    }
                    

                    return new JsonResult(new
                    {
                        success = true,
                        error = false,
                        chat_room
                    });
                }
                catch(Exception ex)
                {
                    await transaction.RollbackAsync();

                    return new JsonResult(new
                    {
                        success = false,
                        error = true,
                        error_msg = ex.Message
                    });
                }
            }
        }






        [HttpPost]
        public async Task<IActionResult> DeleteMessages(PostMethodData postMethodData)
        {
            using(var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var chat_room = JsonConvert.DeserializeObject<ChatRoomModel>(postMethodData.json_data);
                    var db_conversations = await _context.Conversations.AsNoTracking().Where(a => a.ChatRoomId == chat_room.id).ToListAsync();
                    foreach(var item in db_conversations)
                    {
                        var delete_conv = new ConversationDelete();
                        delete_conv.ChatRoomId = item.ChatRoomId;
                        delete_conv.ConversationId = item.Id;
                        delete_conv.CreatedDate = DateTime.Now;
                        delete_conv.DeletedBy = chat_room.user.id;
                        delete_conv.ForAll = false;

                        _context.ConversationDeletes.Add(delete_conv);
                        await _context.SaveChangesAsync();
                    }


                   await transaction.CommitAsync();

                    return new JsonResult(new
                    {
                        success = true,
                        error = false,
                    });

                }
                catch(Exception ex)
                {
                    await transaction.RollbackAsync();

                    return new JsonResult(new
                    {
                        success = false,
                        error = true,
                        error_msg = ex.Message
                    });
                }
            }
        }


     
      

    }
}
