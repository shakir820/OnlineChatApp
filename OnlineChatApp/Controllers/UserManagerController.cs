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
    public class UserManagerController : ControllerBase
    {
        
        OnlineChatDbContext _context;

        public UserManagerController(OnlineChatDbContext context)
        {
            _context = context;
        }




        [HttpPost]
        public async Task<IActionResult> CreateNewUser(UserModel userModel)
        {
            using(var transaction  = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var ne = userModel.email.Normalize();
                    var user_email = await _context.Users.AsNoTracking().FirstOrDefaultAsync(a => a.NormalizedEmail == ne);

                    if(user_email != null)
                    {
                        return new JsonResult(new
                        {
                            success = false,
                            error = true,
                            email_exist = true,
                            error_msg = "Email already exist"
                        });
                    }

                    var user = new User();
                    user.Email = userModel.email;
                    user.FirstName = userModel.first_name;
                    user.LastName = userModel.last_name;
                    user.NormalizedEmail = userModel.email.Normalize();
                    user.NormalizedFirstName = userModel.first_name.Normalize();
                    user.NormalizedLastName = userModel.last_name.Normalize();

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();


                    var um = ModelBindingResolver.ResolveUser(user);

                    return new JsonResult(new
                    {
                        success = true,
                        error = false,
                        user = um
                    });

                }
                catch (Exception ex)
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




        [HttpPost]
        public async Task<IActionResult> LoginUser(UserModel userModel)
        {
            try
            {
                var ne = userModel.email.Normalize();
                var db_user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(a => a.NormalizedEmail == ne);

                if(db_user == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        error = false,
                        email_doesnt_exist = true,
                        error_msg = "Email doesn't exist"
                    });
                }

                var user = ModelBindingResolver.ResolveUser(db_user);


                return new JsonResult(new
                {
                    success = true,
                    error = false,
                    user
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






        public async Task<IActionResult> GetUserById(long id)
        {
            try
            {
                var db_user = await _context.Users.FirstOrDefaultAsync(a => a.Id == id);
                var user = ModelBindingResolver.ResolveUser(db_user);

                return new JsonResult(new
                {
                    success = true,
                    error = false,
                    user
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






        public async Task<IActionResult> GetAllUserList()
        {
            try
            {
                var db_users = await _context.Users.ToListAsync();
                var user_list = new List<UserModel>();
                foreach(var item in db_users)
                {
                    user_list.Add(ModelBindingResolver.ResolveUser(item));
                }

                return new JsonResult(new
                {
                    success = true,
                    error = false,
                    user_list
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






        public async Task<IActionResult> SearchUser(string search_key)
        {
            try
            {
                var nsk = search_key.Normalize();
                var db_user_list = await _context.Users.Where(a => a.NormalizedFirstName.Contains(nsk) || a.NormalizedLastName.Contains(nsk)).ToListAsync();

                var user_list = new List<UserModel>();

                foreach(var item in db_user_list)
                {
                   user_list.Add(ModelBindingResolver.ResolveUser(item));
                }


                return new JsonResult(new
                {
                    success = true,
                    error = false,
                    user_list
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





    }
}
