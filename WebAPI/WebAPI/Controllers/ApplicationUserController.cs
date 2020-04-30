using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        public ApplicationUserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        //GET : api/ApplicationUser
        public async Task<Object> GetApplicationUsrers()
        {
            var users = await _userManager.Users.ToListAsync();

            List<ApplicationUserModel> applicationUsers = new List<ApplicationUserModel>();

            foreach(var user in users)
            {
                ApplicationUserModel userModel = new ApplicationUserModel()
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    FullName = user.FullName
                };
                applicationUsers.Add(userModel);
            }

            return applicationUsers;
        }

        [HttpPost]
        [Route("Register")]
        // Post : /api/ApplicationUser/Register
        public async Task<object> PostApplication(ApplicationUserModel model)
        {
            var applicationUser = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName
            };

            try
            {
                var result = await _userManager.CreateAsync(applicationUser, model.Password);
                return Ok(result);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //DELETE :api/ApplicationUser
        [HttpDelete("{userName}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string userName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _userManager.Users.Where(e => e.UserName == userName).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }
            await _userManager.DeleteAsync(user);

            return Ok(user);
        }
    }
}