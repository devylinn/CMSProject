using CMSRPPF.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CMSRPPF.Startup))]
namespace CMSRPPF
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
           // CreateUserAndroles();
        }


        //public void CreateUserAndroles()
        //{
        //    ApplicationDbContext context = new ApplicationDbContext();
        //    var rolemanager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());
        //    var usermanager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());
        //    if (!rolemanager.RoleExists("Administrator"))
        //    {
        //        //Create Default Role
        //        var role = new IdentityRole("Administrator");
        //        rolemanager.Create(role);

        //        //Create Default Users
        //        var user = new ApplicationUser();
        //        user.UserName = "admin@admin.com";
        //        user.Email = "admin@admin.com";
        //        string pwd = "cats123A?";
        //        var newuser = usermanager.Create(user, pwd);
        //        if (newuser.Succeeded)
        //        {
        //            usermanager.AddToRole(user.Id, "Administrator");
        //        }
        //    }
        //}
    }


}
