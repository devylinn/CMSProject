using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CMSRPPF.Areas.Admin.Models
{
    public class PermissionsViewModel
    {

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please Select the Role")]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

        [NotMapped]
        [Display(Name = "Roles List")]
        public List<IdentityRole> RolesList { get; set; }
    }
}