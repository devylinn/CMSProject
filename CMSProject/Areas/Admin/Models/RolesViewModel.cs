using CMSRPPF.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CMSRPPF.Areas.Admin.Models
{
    public class RolesViewModel
    {
        public RolesViewModel() { }

        public RolesViewModel(ApplicationRole role)
        {
            Id = role.Id;
            RoleName = role.Name;
            
          
        }
  
        public string Id { get; set; }

        [Required]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

        [Display(Name = "No. of Users")]
        public int Count { get; set; }

       

    }
}