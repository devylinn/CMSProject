using CMSRPPF.Areas.Admin.Models;
using CMSRPPF.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMSRPPF.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
     
            
            return View();
        }

       
        public ActionResult _Carousel()
        {
            List<CarouselViewModel> sliderimages = new List<CarouselViewModel>();
            string CS = ConfigurationManager.ConnectionStrings["Db"].ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spGetAllSliderImage", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    CarouselViewModel slider = new CarouselViewModel();
                    slider.ID = Convert.ToInt32(rdr["ID"]);
                    slider.Name = rdr["Name"].ToString();
                    slider.FileSize = Convert.ToInt32(rdr["FileSize"]);
                    slider.FilePath = rdr["FilePath"].ToString();

                    sliderimages.Add(slider);
                }
            }
            return PartialView("_Carousel",sliderimages);
        }

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Evenimente
        public ActionResult _Evenimente()
        {
            return PartialView("_Evenimente", db.Events.OrderByDescending(i => i.Created).Take(3).ToList());
        }

    }
}