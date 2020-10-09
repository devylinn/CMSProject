using CMSRPPF.Areas.Admin.Models;
using CMSRPPF.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMSRPPF.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator, Moderator, Operator")]
    public class AdmitereController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            List<AdmitereViewModel> list;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                // Init the list
                list = db.Admitere.ToArray().OrderByDescending(i => i.Created).Select(x => new AdmitereViewModel(x)).ToList();
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
        public ActionResult Create(AdmitereViewModel model)
        {
            // Check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                AdmitereDTO dto = new AdmitereDTO();



                // DtO Title
                dto.Title = model.Title;


                //Make sure title and slug are unique
                if (db.Admitere.Any(x => x.Title == model.Title))
                {
                    ModelState.AddModelError("", "That title already exists.");
                    return View(model);

                }
                //DTO the rest


                dto.Content = model.Content;
                dto.UserId = User.Identity.GetUserId();
                dto.Created = DateTime.Now;

                //Save DTO
                db.Admitere.Add(dto);

                db.SaveChanges();


            }
            //Set TempData message
            TempData["SMMM"] = "You have added a new event!";

            //Redirect
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Moderator")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            // Declare pageVM
            AdmitereViewModel model;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                // Get the page
                AdmitereDTO dto = db.Admitere.Find(id);

                // Confirm page exists
                if (dto == null)
                {
                    return Content("The post does not exist.");
                }

                // Init pageVM
                model = new AdmitereViewModel(dto);
            }

            // Return view with model
            return View(model);
        }


        [Authorize(Roles = "Moderator")]
        [HttpPost]
        public ActionResult Edit(AdmitereViewModel model)
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
                AdmitereDTO dto = db.Admitere.Find(id);

                // DTO the title
                dto.Title = model.Title;


                // Make sure title and slug are unique
                if (db.Admitere.Where(x => x.Id != id).Any(x => x.Title == model.Title))
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
            TempData["SMMM"] = "You have edited the post!";

            // Redirect
            return RedirectToAction("Index");
        }



        [Authorize(Roles = "Moderator")]
        public ActionResult Details(int id)
        {
            // Declare PageVM
            AdmitereViewModel model;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                // Get the page
                AdmitereDTO dto = db.Admitere.Find(id);

                // Confirm page exists
                if (dto == null)
                {
                    return Content("The post does not exist.");
                }


                // Init PageVM
                model = new AdmitereViewModel(dto);
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

                AdmitereDTO dto = db.Admitere.Find(id);

                // Remove the page
                db.Admitere.Remove(dto);

                // Save
                db.SaveChanges();
            }

            // Redirect
            return RedirectToAction("Index");
        }

    }
}