using CMSRPPF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMSRPPF.Areas.Admin.Models
{
    public class LicentaViewModel
    {

        public LicentaViewModel()
        {
        }

        public LicentaViewModel(LicentaDTO row)
        {
            Id = row.Id;
            Title = row.Title;
            Content = row.Content;
            UserId = row.UserId;
            Created = row.Created;
            Edited = row.Edited;
            User = row.User;

        }

        public int Id { get; set; }
        public string Title { get; set; }
        [AllowHtml]
        public string Content { get; set; }

        [Display(Name = "Created on")]
        public DateTime? Created { get; set; }


        [Display(Name = "Last edited")]
        public DateTime? Edited { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Display(Name = "Author")]
        public string UserId { get; set; }

    }


    [Table("Licenta")]
    public class LicentaDTO
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        [AllowHtml]
        public string Content { get; set; }
        [Display(Name = "Created on")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm }", ApplyFormatInEditMode = true)]
        public DateTime? Created { get; set; }

        [Display(Name = "Last edited")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm }", ApplyFormatInEditMode = true)]
        public DateTime? Edited { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Display(Name = "Author")]
        public string UserId { get; set; }

    }
}
