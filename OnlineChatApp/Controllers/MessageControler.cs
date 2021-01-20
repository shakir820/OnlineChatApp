using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineChatApp.Data;
using OnlineChatApp.Helper;
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

        public MessageController(OnlineChatDbContext context)
        {
            _context = context;
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
                }).AsNoTracking().Where(a => a.m.Id == user_id).ToListAsync();


                var chat_room_list = new List<ChatRoomModel>();

                foreach (var item in db_chat_rooms)
                {
                    var db_chat_memebers = await _context.ChatRoomMembers.Join(_context.Users, o => o.MemeberId, i => i.Id, (o, i) => new
                    {
                        crm = o,
                        u = i
                    }).AsNoTracking().Where(a => a.crm.ChatRoomId == item.cr.Id && a.crm.MemeberId != user_id).ToListAsync();

                    var cr = item.cr;

                    var chat_room = new ChatRoomModel();
                    chat_room.id = cr.Id;
                    chat_room.created_date = cr.CreatedDate;
                    chat_room.is_group = cr.IsGroup;

                    if (cr.IsGroup == false)
                    {
                        var receiver = db_chat_memebers[0].u;
                        chat_room.name = receiver.FirstName + " " + receiver.LastName;
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
                    var db_chat_rrom = await _context.ChatRooms.AsNoTracking().FirstOrDefaultAsync(a => a.Id == chatRoomId);
                    var chat_room = ModelBindingResolver.ResolveChatRoom(db_chat_rrom);

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








      

    }
}
