using CMSRPPF.Areas.Admin.Models;
using CMSRPPF.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMSRPPF.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator, Moderator, Operator")]
    public class LicentaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Noutati
        public ActionResult Index()
        {
            List<LicentaViewModel> list;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                // Init the list
                list = db.Licenta.ToArray().OrderByDescending(i => i.Created).Select(x => new LicentaViewModel(x)).ToList();
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
        public ActionResult Create(LicentaViewModel model)
        {
            // Check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                LicentaDTO dto = new LicentaDTO();

        

                // DtO Title
                dto.Title = model.Title;
               

                //Make sure title and slug are unique
                if (db.Licenta.Any(x => x.Title == model.Title))
                {
                    ModelState.AddModelError("", "That title already exists.");
                    return View(model);

                }
                //DTO the rest


                dto.Content = model.Content;
                dto.UserId = User.Identity.GetUserId();
                dto.Created = DateTime.Now;
      
                //Save DTO
                db.Licenta.Add(dto);

                db.SaveChanges();


            }
            //Set TempData message
            TempData["SMM"] = "You have added a new event!";

            //Redirect
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Moderator")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            // Declare pageVM
            LicentaViewModel model;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                // Get the page
                LicentaDTO dto = db.Licenta.Find(id);

                // Confirm page exists
                if (dto == null)
                {
                    return Content("The post does not exist.");
                }

                // Init pageVM
                model = new LicentaViewModel(dto);
            }

            // Return view with model
            return View(model);
        }


        [Authorize(Roles = "Moderator")]
        [HttpPost]
        public ActionResult Edit(LicentaViewModel model)
        {

            // Check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (ApplicationDbContext db = new ApplicationDbContext())
            {

                // Get page id
                int id = model.Id;

                // Get the page
                LicentaDTO dto = db.Licenta.Find(id);
                
                // DTO the title
                dto.Title = model.Title;


                // Make sure title and slug are unique
                if (db.Licenta.Where(x => x.Id != id).Any(x => x.Title == model.Title))
                {
                    ModelState.AddModelError("", "That title already exists.");
                    return View(model);
                }

                // DTO the rest
                dto.Content = model.Content;
                dto.Edited = DateTime.Now;

                // Save the DTO
                db.SaveChanges();
            }

            // Set TempData message
            TempData["SMM"] = "You have edited the post!";

            // Redirect
            return RedirectToAction("Index");
        }



        [Authorize(Roles = "Moderator")]
        public ActionResult Details(int id)
        {
            // Declare PageVM
            LicentaViewModel model;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                // Get the page
                LicentaDTO dto = db.Licenta.Find(id);

                // Confirm page exists
                if (dto == null)
                {
                    return Content("The post does not exist.");
                }


                // Init PageVM
                model = new LicentaViewModel(dto);
            }

            // Return view with model
            return View(model);
        }

        [Authorize(Roles = "Moderator")]
        public ActionResult Delete(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                // Get the page

                LicentaDTO dto = db.Licenta.Find(id);
               
                // Remove the page
                db.Licenta.Remove(dto);

                // Save
                db.SaveChanges();
            }

            // Redirect
            return RedirectToAction("Index");
        }

    }
}
