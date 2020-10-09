using CMSRPPF.Areas.Admin.Models;
using CMSRPPF.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace CMSRPPF.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator, Moderator, Operator" )]
    public class DashboardController : Controller
    {

        private ApplicationDbContext context = new ApplicationDbContext();

        private ApplicationUserManager _userManager;


        public DashboardController(ApplicationUserManager userManager)
        {

            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public DashboardController()
        {

        }

        // GET: Admin/Dashboard

        public async Task<ActionResult> Index()
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            ViewBag.Id = currentUser.Id;
            ViewBag.Username = currentUser.UserName;
            ViewBag.Email = currentUser.Email;
            ViewBag.Phone = currentUser.PhoneNumber;
            ViewBag.Firstname = currentUser.FirstName;
            ViewBag.Lastname = currentUser.LastName;
            ViewBag.Address = currentUser.Address;
            ViewBag.Gender = currentUser.Gender;
            ViewBag.Birthday = currentUser.Birthday.HasValue ? currentUser.Birthday.Value.ToString("dd-MM-yyyy") : string.Empty;

            return View();
        }
    }
}