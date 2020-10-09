using CMSRPPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMSRPPF.Controllers
{
    public class EvenimenteController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Evenimente
        public ActionResult Index()
        {
            return View(db.Events.OrderByDescending(i=>i.Created).ToList());
        }
    }
}