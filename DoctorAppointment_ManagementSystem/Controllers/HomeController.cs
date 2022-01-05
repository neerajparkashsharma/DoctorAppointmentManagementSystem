using DoctorAppointment_ManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointment_ManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        DoctorAppointment_ManagemenetSystemEntities db = new DoctorAppointment_ManagemenetSystemEntities();


         [AllowAnonymous]
        // GET: Home
        public ActionResult Home()
        {
            count();
            return View();
        }
        [AllowAnonymous]
        public ActionResult AboutUs()
        {
            count();
            return View();
        }
        [AllowAnonymous]
        public ActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult ContactUs([Bind(Include = "ID,Name,EmailAddress,Message1")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Messages.Add(message);
                db.SaveChanges();
                return RedirectToAction("Home","Home");
            }

            return View(message);
        }

        public void count()
        {
            ViewBag.DocActiveCount = db.Users.Where(x=>x.isActive == true && x.UserRoleId == 2).ToList().Count();
            ViewBag.DocInActiveCount = db.sp_InActiveDocList().Count();
            ViewBag.PatientActiveCount = db.sp_ActivePatientsList().Count();
            ViewBag.PatientInActiveCount = db.sp_InActivePatientsList().Count();
            ViewBag.UsersActiveCount = db.sp_TotalActiveUsers().Count();
            ViewBag.UsersInActiveCount = db.sp_TotalInActiveUsers().Count();


        }
    }
}