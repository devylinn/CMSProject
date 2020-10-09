using CMSRPPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMSRPPF.Controllers
{
    public class AcademicController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Academic
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Licenta()
        {
            return View(db.Licenta.OrderByDescending(i => i.Created).ToList());
        }

        public ActionResult Masterat()
        {
            return View();
        }
    }
}