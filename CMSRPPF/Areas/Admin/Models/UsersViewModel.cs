using CMSRPPF.Models;
using System;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMSRPPF.Areas.Admin.Models
{
    public class UsersViewModel
    {
        public UsersViewModel() { }

        public UsersViewModel(ApplicationUser user)
        {
            Id = user.Id;
            UserName = user.UserName;
        }
      

        public string Id { get; set; }

        [Required]
        [Display(Name ="User Name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name ="Email")]
        public string Email { get; set; }

        [Display(Name = "Role Name")]
        public IEnumerable<string> Role { get; set; }

    }

}