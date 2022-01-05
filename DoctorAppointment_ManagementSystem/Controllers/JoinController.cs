using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoctorAppointment_ManagementSystem.Models.ViewModels;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using DoctorAppointment_ManagementSystem.Models;

namespace DoctorAppointment_ManagementSystem.Controllers
{
    public class JoinController : Controller
    {
        // GET: Join
        public ActionResult ListOfDoc()
        {
            //List<DocProfile> dp = new List<DocProfile>();
            //string mainconn = ConfigurationManager.ConnectionStrings["DoctorAppointment_ManagemenetSystemEntities"].ConnectionString;
            //SqlConnection sqlconn = new SqlConnection(mainconn);
            //string sqlquery = "select u.UserName,u.FullName,u.DOB,u.Gender,u.Email,u.City,s.specializationName from [User] u inner join  Doc_Specialization ds on ds.DoctorID = u.User_ID inner join Specialization s on s.SpecializationId = ds.SpecializationId where u.UserRoleId = 2 and u.isActive = 1";
            //SqlCommand sqlcommand = new SqlCommand(sqlquery, sqlconn);
            //SqlDataAdapter sda = new SqlDataAdapter(sqlcommand);
            //DataTable dt = new DataTable();
            //sda.Fill(dt);

            //foreach (DataRow dr in dt.Rows)
            //{
            //    DocProfile dpf = new DocProfile();
            //    dpf.UserName = dr["UserName"].ToString();
            //    dpf.FullName = dr["FullName"].ToString();
            //    dpf.DOB = Convert.ToDateTime(dr["DOB"]);
            //    dpf.Gender = dr["Gender"].ToString();
            //    dpf.Email = dr["Email"].ToString();
            //    dpf.City = dr["City"].ToString();
            //    dpf.specializationName = dr["UserName"].ToString();

            //    dp.Add(dpf);


            return View();
        }
          
        }
    }
