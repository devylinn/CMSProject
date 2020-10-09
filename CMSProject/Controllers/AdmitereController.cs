using CMSRPPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMSRPPF.Controllers
{
    public class AdmitereController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admitere
        public ActionResult Index()
        {
            return View(db.Admitere.OrderByDescending(i => i.Created).ToList());
        }
    }
}