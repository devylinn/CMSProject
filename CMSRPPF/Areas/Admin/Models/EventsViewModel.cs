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
    public class EventsViewModel
    {

        public EventsViewModel()
        {
        }

        public EventsViewModel(EventsDTO row)
        { 
            Id = row.Id;
            Title = row.Title;
            Content = row.Content;
            UserId = row.UserId;
            Created = row.Created;
            FilePath = row.FilePath;
            FileName = row.FileName;
            User = row.User;

        }

        public int Id { get; set; }
        public string Title { get; set; }
        [AllowHtml]
        public string Content { get; set; }

        [Display(Name = "Created on")]
        public DateTime? Created { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Display(Name ="Author")]
        public string UserId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

        public HttpPostedFileBase fileupload { get; set; }
    }


    [Table("Noutati")]
    public class EventsDTO
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
        public virtual ApplicationUser User { get; set; }

        [Display(Name = "Author")]
        public string UserId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

    }
}