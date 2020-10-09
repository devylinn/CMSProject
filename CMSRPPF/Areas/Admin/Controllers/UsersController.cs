using CMSRPPF.Areas.Admin.Models;
using CMSRPPF.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PagedList;

namespace CMSRPPF.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator, Moderator, Operator")]
    public class UsersController : Controller
    {

        private ApplicationDbContext context = new ApplicationDbContext();

        private ApplicationRoleManager _roleManager;
        private ApplicationUserManager _userManager;


        public UsersController()
        {

        }

        public UsersController(ApplicationRoleManager roleManager, ApplicationUserManager userManager)
        {
            RoleManager = roleManager;
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

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }


        // GET: Admin/GetRoles
        public ActionResult Index(int? page)
        {
         
            var userRoles = new List<UsersViewModel>();
            var context = new ApplicationDbContext();
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
           
            List<UsersViewModel> list = new List<UsersViewModel>();

            foreach (var user in UserManager.Users)
            {
                var r = new UsersViewModel(user)
                {
                    UserName = user.UserName,
                    Email=user.Email,

                    Role= userManager.GetRoles(userStore.Users.First(s => s.UserName == user.UserName).Id)
                };

                list.Add(r);
            }

            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<ActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    return RedirectToAction("Index");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);

        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Edit(string id)
        {
            var user = await UserManager.FindByIdAsync(id);

            var model = new InfoViewModel
            {
                FirstName=user.FirstName,
                LastName=user.LastName,
                Email=user.Email,
                Gender = user.Gender,
                PhoneNumber = user.PhoneNumber,
                Birthday= user.Birthday
        };

            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(InfoViewModel model)
        {
            var user = await UserManager.FindByIdAsync(model.Id);
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.Gender = model.Gender;
            user.PhoneNumber = model.PhoneNumber;
            user.Birthday = model.Birthday;
            var updateResult = await UserManager.UpdateAsync(user);
            if (updateResult.Succeeded)
            {
                return RedirectToAction("Index");
            }
            AddErrors(updateResult);

            return View();
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Details(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            ViewBag.Username = user.UserName;
            ViewBag.Email = user.Email;
            ViewBag.Phone = user.PhoneNumber;
            ViewBag.Firstname = user.FirstName;
            ViewBag.Lastname = user.LastName;
            ViewBag.Address = user.Address;
            ViewBag.Gender = user.Gender;
            ViewBag.Birthday = user.Birthday.HasValue ? user.Birthday.Value.ToString("dd-MM-yyyy") : string.Empty;
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
           
            return View(new UsersViewModel(user));

        }

        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var user = await UserManager.FindByIdAsync(id);


            if (user == null)
            {
                return HttpNotFound();

            }
            else
            {
                var result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                return View("Index");
            }
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult AddRole()
        {


            var _context = new ApplicationDbContext();
            var roles = _context.Roles.ToList();

            var viewModel = new PermissionsViewModel
            {
                RolesList = roles
            };
            return View(viewModel);

        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddRole(PermissionsViewModel model)
        {
            var _context = new ApplicationDbContext();

            if (ModelState.IsValid)
            {
                var user = UserManager.FindByName(model.UserName);

                if (user != null)
                {
                    var result = await UserManager.AddToRoleAsync(user.Id, model.RoleName);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Users");
                    }
                    AddErrors(result);
                }
                else
                {
                    ModelState.AddModelError("", "Username not found");
                }
  
            }
            var roles = _context.Roles.ToList();
            model.RolesList = roles;
            return View(model);

        }


        [Authorize(Roles = "Administrator")]
        public ActionResult RemoveRole()
        {
            var _context = new ApplicationDbContext();
            var roles = _context.Roles.ToList();

            var viewModel = new PermissionsViewModel
            {
                RolesList = roles
            };
            return View(viewModel);

        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveRole(PermissionsViewModel model)
        {
            var _context = new ApplicationDbContext();

            if (ModelState.IsValid)
            {
                var user = UserManager.FindByName(model.UserName);

                if (user != null)
                {
                    var result = await UserManager.RemoveFromRoleAsync(user.Id, model.RoleName);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Users");
                    }
                    AddErrors(result);
                }
                else
                {
                    ModelState.AddModelError("", "Username not found");
                }

            }

            
            var roles = _context.Roles.ToList();
            model.RolesList = roles;
            return View(model);

        }





        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

    }

}