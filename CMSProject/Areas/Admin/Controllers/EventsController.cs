using CMSRPPF.Areas.Admin.Models;
using CMSRPPF.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMSRPPF.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator, Moderator, Operator")]
    public class EventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Admin/Noutati
        public ActionResult Index()
        {
            List<EventsViewModel> list;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                // Init the list
                list = db.Events.ToArray().OrderByDescending(i => i.Created).Select(x => new EventsViewModel(x)).ToList();
            }

            return View(list);
        }


        [Authorize(Roles = "Moderator")]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Moderator")]
        [HttpPost]
        public ActionResult Create(EventsViewModel model)
        {
            // Check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                EventsDTO dto = new EventsDTO();

                if (model.fileupload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(model.fileupload.FileName);
                    string extension = Path.GetExtension(model.fileupload.FileName);
                    fileName = fileName + DateTime.Now.ToString("yyyymmddMMss") + extension;
                    model.FileName = fileName;
                    model.FilePath = "~/Images/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/"), fileName); 
                    model.fileupload.SaveAs(fileName);
                }
                else
                {
                    string defaultName = "";
                    string fileName = "default";
                    string extension = ".jpg";
                    model.FileName = defaultName;
                    fileName = fileName + extension;
                    model.FilePath = "~/Images/" + fileName;

                }

               
                // DtO Title
                dto.Title = model.Title;
                dto.FileName = model.FileName;
           
                //Make sure title and slug are unique
                if (db.Events.Any(x => x.Title == model.Title))
                {
                    ModelState.AddModelError("", "That title already exists.");
                    return View(model);

                }
                //DTO the rest
              

                dto.Content = model.Content;
                dto.UserId = User.Identity.GetUserId();
                dto.Created = DateTime.Now;
                dto.FilePath = model.FilePath;
                dto.FileName = model.FileName;
                //Save DTO
                db.Events.Add(dto);
               
                db.SaveChanges();


            }
            //Set TempData message
            TempData["SM"] = "You have added a new event!";

            //Redirect
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Moderator")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            // Declare pageVM
            EventsViewModel model;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                // Get the page
                EventsDTO dto = db.Events.Find(id);

                // Confirm page exists
                if (dto == null)
                {
                    return Content("The post does not exist.");
                }

                // Init pageVM
                model = new EventsViewModel(dto);
            }

            // Return view with model
            return View(model);
        }

        [Authorize(Roles = "Moderator")]
        [HttpPost]
        public ActionResult Edit(string fullName, EventsViewModel model)
        {

            // Check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (model.fileupload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(model.fileupload.FileName);
                    string extension = Path.GetExtension(model.fileupload.FileName);
                    //DateTime.Now.ToString("yyyymmddMMss") 
                    fileName = fileName + DateTime.Now.ToString("yyyymmddMMss") + extension;
                    model.FileName = fileName;

                    model.FilePath = "~/Images/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    model.fileupload.SaveAs(fileName);
                }
                else
                {
                    string defaultName = "";
                    string fileName = "default";
                    string extension = ".jpg";
                    model.FileName = defaultName;
                    fileName = fileName + extension;
                    model.FilePath = "~/Images/" + fileName;

                }
                // Get page id
                int id = model.Id;



                // Get the page
                EventsDTO dto = db.Events.Find(id);

                fullName = dto.FileName.ToString();

                string fullPath = Request.MapPath("~/Images/" + fullName);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                // DTO the title
                dto.Title = model.Title;

 

                // Make sure title and slug are unique
                if (db.Events.Where(x => x.Id != id).Any(x => x.Title == model.Title) )
                {
                    ModelState.AddModelError("", "That title already exists.");
                    return View(model);
                }

                // DTO the rest
                dto.Content = model.Content;
                dto.FilePath = model.FilePath;
                dto.FileName = model.FileName;



                
                // Save the DTO
                db.SaveChanges();
            }

            // Set TempData message
            TempData["SM"] = "You have edited the post!";

            // Redirect
            return RedirectToAction("Index");
        }



        [Authorize(Roles = "Moderator")]
        public ActionResult Details(int id)
        {
            // Declare PageVM
            EventsViewModel model;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                // Get the page
                EventsDTO dto = db.Events.Find(id);

                // Confirm page exists
                if (dto == null)
                {
                    return Content("The post does not exist.");
                }


                // Init PageVM
                model = new EventsViewModel(dto);
            }

            // Return view with model
            return View(model);
        }

        [Authorize(Roles = "Moderator")]
        public ActionResult Delete(string fullName ,int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                // Get the page

                EventsDTO dto = db.Events.Find(id);
                fullName = dto.FileName.ToString();

                string fullPath = Request.MapPath("~/Images/" + fullName);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                // Remove the page
                db.Events.Remove(dto);

                // Save
                db.SaveChanges();
            }

            // Redirect
            return RedirectToAction("Index");
        }

    }

}