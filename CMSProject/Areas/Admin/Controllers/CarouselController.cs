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

namespace CMSRPPF.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator, Moderator, Operator")]
    public class CarouselController : Controller
    {
        [HttpGet]
        public ActionResult Index()
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
            return View(sliderimages);
        }

        [Authorize(Roles = "Moderator")]
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase fileupload)
        {
            if (fileupload != null)
            {
                string fileName = Path.GetFileName(fileupload.FileName);
                int fileSize = fileupload.ContentLength;
                int Size = fileSize / 1000;
                
                fileupload.SaveAs(Server.MapPath("~/SliderImages/" + fileName));

                string CS = ConfigurationManager.ConnectionStrings["Db"].ConnectionString;
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("spAddNewSliderImage", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.Parameters.AddWithValue("@Name", fileName);
                    cmd.Parameters.AddWithValue("@FileSize", Size);
                    cmd.Parameters.AddWithValue("FilePath", "~/SliderImages/" + fileName);
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Moderator")]
        [HttpPost]
        public ActionResult Delete(string picList, int id)

        {
            string CS = ConfigurationManager.ConnectionStrings["Db"].ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SelectByID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    CarouselViewModel slider = new CarouselViewModel();
              
                    slider.Name = rdr["Name"].ToString();
                    slider.DeleteImg(id);
                    picList = slider.Name;

                    string fullPath = Request.MapPath("~/SliderImages/" + picList);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                }

            }
            return RedirectToAction("Index");
        }


    }
}