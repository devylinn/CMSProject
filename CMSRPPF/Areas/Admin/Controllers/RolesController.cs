using CMSRPPF.Areas.Admin.Models;
using CMSRPPF.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using PagedList;

namespace CMSRPPF.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator, Moderator, Operator" )]
    public class RolesController : Controller
    {
        private ApplicationRoleManager _roleManager;
        private ApplicationUserManager _userManager;


        public RolesController()
        {
        }

        public RolesController(ApplicationRoleManager roleManager, ApplicationUserManager userManager)
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

    

        public ActionResult Index(int? page)
        {
             ApplicationDbContext db = new ApplicationDbContext();

             List<RolesViewModel> list = new List<RolesViewModel>();


            foreach (var role in RoleManager.Roles)
               {

              var r = new RolesViewModel(role)
               {

                       RoleName = role.Name,              
                       Count = db.Users.Where(x=>x.Roles.Any(roles => roles.RoleId == role.Id)).ToList().Count()

               };

                list.Add(r);
             
             }
            int pageSize = 5;
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
        public async Task<ActionResult> Create(RolesViewModel model)
        {
            if(ModelState.IsValid)
            {
            
                var role = new ApplicationRole() { Name = model.RoleName };
                await RoleManager.CreateAsync(role);
                return RedirectToAction("Index");
            }

            return View();         
            
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Edit(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);

            return View(new RolesViewModel(role));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<ActionResult> Edit(RolesViewModel model)
        {
            var role = new ApplicationRole() { Id = model.Id, Name = model.RoleName };
            await RoleManager.UpdateAsync(role);
            return RedirectToAction("Index");
        }

        // public async Task<ActionResult> Details(string id)
        // {
        //    var role = await RoleManager.FindByIdAsync(id);
        //    return View(new RolesViewModel(role));
        //}

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Delete(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            return View(new RolesViewModel(role));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            await RoleManager.DeleteAsync(role);
            return RedirectToAction("Index");
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
