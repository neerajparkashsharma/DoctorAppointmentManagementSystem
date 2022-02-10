using System;
using System.Linq;
using System.Web.Mvc;
using DoctorAppointment_ManagementSystem.Models;

namespace DoctorAppointment_ManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        // GET: Account
        public ActionResult Login()
        {

            return View();
        }
          
   

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]

        public ActionResult Login(Login u )
        {
            //string username; int userid,  userrole;          
           
                using (var context = new DoctorAppointment_ManagemenetSystemEntities())
                {

                    var obj = context.Users.Where(x => x.UserName.Equals(u.UserName) && x.Password.Equals(u.Password)).FirstOrDefault();
                    if (obj != null)
                    {

                        Session["UserID"] = obj.User_ID;
                        Session["UserName"] = obj.UserName;
                        Session["UserRole"] = obj.UserRoleId;
                        Session["FullName"] = obj.FullName;
                  



                    if (Convert.ToInt32(Session["UserRole"]) == 3)
                        {
                            TempData["Message"] = "Welcome! " + Session["UserName"];  
                            return RedirectToAction("Home", "Home");
                        }
                        else if (Convert.ToInt32(Session["UserRole"]) == 1)
                        {
                            TempData["Message"] = "Welcome! " + Session["UserName"];
                            return RedirectToAction("Index2", "Users");
                        }
                        else
                        {
                            TempData["Message"] = "Welcome! " + Session["UserName"];
                            return RedirectToAction("index", "appointments");
                        }

                    }

                    else
                    {
                     ModelState.AddModelError("", "Invalid Login");
                     return View();
                    }


                }

            }
    

        public ActionResult UnAuthorized()
        {
            
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            //ModelState.AddModelError("", "Session Expired! Please Login");
            return RedirectToAction("Login");
        }


    }
}