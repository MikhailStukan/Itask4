using Itask_4.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Itask_4.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Itask_4.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext db;
        private readonly UserManager<AppUser> _usermanager;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            db = context;
            _usermanager = userManager;
            _signInManager = signInManager;

        }

        public  async Task<IActionResult> Index()
        {
            return await GetUsersData();
        }

        private async Task<IActionResult> GetUsersData()
        {
            var result = await (from user in db.Users
                                select new AppUser { Id = user.Id, Name = user.Name, Email = user.Email, RegistrationTime = user.RegistrationTime, LastLoginTime = user.LastLoginTime, isActive = user.isActive }).ToListAsync();

            return View(result);
        }

        public async Task<JsonResult> Delete(string[] id)
        {
            try
            {
                foreach (var idDelete in id)
                {
                    AppUser user = await _usermanager.FindByIdAsync(idDelete);
                    if (user != null)
                    {
                        AppUser current = GetCurrentUser();
                        if (user.Id == current.Id)
                        {
                            _signInManager.SignOutAsync();
                            DeleteCookies();
                        }
                        await _usermanager.DeleteAsync(user);
                    }
                }
                return new JsonResult(1);
            }
            catch
            {
                return new JsonResult(0);
            }
        }

        public async Task<JsonResult> BlockUser(string[] id, string block)
        {
            try
            {
                foreach (var idBlock in id)
                {
                    AppUser user = await _usermanager.FindByIdAsync(idBlock);

                    if(user != null)
                    {
                        if (block.Equals("Unblock"))
                        {
                            EnableUser(user);
                        }
                        else
                        {
                            DisableUser(user);
                        }

                    }
                }
                return new JsonResult(1);
            }
            catch
            {
                return new JsonResult(0);
            }
        }

        private void DisableUser(AppUser user)
        {
            user.LockoutEnabled = true;
            user.isActive = false;
            user.LockoutEnd = new DateTimeOffset(DateTime.Now.AddYears(100));
            db.SaveChanges();
            LogoutIfDisabled();
        }

        private void EnableUser(AppUser user)
        {
            user.LockoutEnabled= false;
            user.isActive = true;
            user.LockoutEnd = new DateTimeOffset(DateTime.Now.AddSeconds(-1));
            db.SaveChanges();
        }
        private AppUser? GetCurrentUser()
        {
            var current_id = _usermanager.GetUserId(User);
            var user = db.Users.SingleOrDefault(u => u.Id == current_id);
            return user;
        }

        private void LogoutIfDisabled()
        {
            AppUser? user = GetCurrentUser();
            if (user != null)
            {
                if(!user.isActive)
                {
                    _signInManager.SignOutAsync();
                }
            }
        }

        private void DeleteCookies()
        {
            foreach (var cookie in HttpContext.Request.Cookies)
            {
                Response.Cookies.Delete(cookie.Key);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}