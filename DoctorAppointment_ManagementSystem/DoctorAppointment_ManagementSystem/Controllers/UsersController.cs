using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DoctorAppointment_ManagementSystem.Models;

using System.Linq.Dynamic;


namespace DoctorAppointment_ManagementSystem.Controllers
{
    public class UsersController : Controller
    {
        private DoctorAppointment_ManagemenetSystemEntities db = new DoctorAppointment_ManagemenetSystemEntities();

        [CustomAuthenticationFilter]
        [CustomAuthorizationFilter("Admin")]
        // GET: Users
        public ActionResult Index(int page = 1, string sort = "UserName", string sortdir = "asc", string search = "")
        {
            int pageSize = 15;
            int totalRecords = 0;
            if (page < 1) page = 1;
            int skip = (page * pageSize) - pageSize;

            var data = GetUsers(search, sort, sortdir, skip, pageSize, out totalRecords);
            ViewBag.TotalRows = totalRecords;
            ViewBag.search = search;
            return View(data);
        }


        public IEnumerable<User> GetUsers(string search, string sort, string sortdir, int skip, int pageSize, out int totalRecord)
        {
            using (DoctorAppointment_ManagemenetSystemEntities db = new DoctorAppointment_ManagemenetSystemEntities())
            {
                var users = db.Users.Include(u => u.Role);
                var v = (from a in users

                         where
                         a.FullName.Contains(search) ||
                         a.UserName.Contains(search) ||
                          a.Gender.Contains(search) ||


                           a.Role.RoleName.Contains(search) ||
                           a.CNIC.Contains(search) ||


                           a.Phone.Contains(search) ||

                           a.Email.Contains(search) ||
                           a.Address.Contains(search) ||
                           a.City.Contains(search)
                         select a
                         );

                totalRecord = v.Count();
                v = v.OrderBy(sort + " " + sortdir);
                if (pageSize > 0)
                {
                    v = v.Skip(skip).Take(pageSize);
                }
                return v.ToList();

            }
        }


        [HttpPost]
        public JsonResult GetDoctorsSpecializations(int id)
        {
            var specialization = string.Join(",", db.Doc_Specialization.ToList().Where(x => x.DoctorID == id).Select(x => x.Specialization.specializationName).ToList());

            var docInfo  = db.Users.ToList().Where(x => x.User_ID == id);
            if (specialization == "")
            {
                specialization = "N/A";
            }
            
            var name = docInfo.Select(x => x.FullName).FirstOrDefault();
            var experience = db.Doc_Experience.ToList().Where(x => x.DocID == id).Select(x=> (DateTime.Now.Year - x.WorkingFrom.Year)).FirstOrDefault().ToString();
            var gender = docInfo.Select(x => x.Gender).FirstOrDefault();
            var email = docInfo.Select(x => x.Email).FirstOrDefault();
            var age = docInfo.Select(x=> DateTime.Now.Year - x.DOB.Year).FirstOrDefault().ToString();
            var phone = docInfo.Select(x => x.Phone).FirstOrDefault();
            return Json(new {email, phone, age, specialization, name,experience,gender }, JsonRequestBehavior.AllowGet);
        }


        [CustomAuthenticationFilter]
        [CustomAuthorizationFilter("Admin")]
        public ActionResult Index2()
        {
            count();

            return View();
        }

      





        [CustomAuthenticationFilter]
        [CustomAuthorizationFilter("Admin", "Patient", "Doctor")]
        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Convert.ToInt32(Session["UserRole"]) == 1)
            {
                User user = db.Users.Find(id);

                if (user == null)
                {
                    return HttpNotFound();

                }
                return View(user);
            }
            else
            {
                User user = db.Users.Find(Session["UserID"]);
                return View(user);
            }




        }
       
        [AllowAnonymous]
        public ActionResult OurDoctors()
        {
            ViewBag.List = db.Users.Where(x => x.isActive == true && x.UserRoleId == 2).ToList();
            var doc = ViewBag.List;
            return View();

        }

       
   


        [AllowAnonymous]
        // GET: Users/Create
        public ActionResult Create()
        {
            if (Convert.ToInt32(Session["UserRole"]) != 1 && Convert.ToInt32(Session["UserRole"]) != 0)
            {
                return RedirectToAction("Home", "Home");
            }

            ViewBag.UserRoleId = new SelectList(db.Roles, "RoleID", "RoleName");
            ViewBag.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ViewBag.UpdatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return View();

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Create([Bind(Include = "User_ID,FullName,UserName,Password,Phone,DOB,Gender,CNIC,Email,City,Address,isActive,CreatedDate,UpdatedDate,CreatedBy,UpdatedBy,UserRoleId")] User user)
        {

            bool usernameExists = db.Users.Any(x => x.UserName.Equals(user.UserName));
            bool emailExists = db.Users.Any(x => x.Email.Equals(user.Email));
            bool CNICExists = db.Users.Any(x => x.CNIC.Equals(user.CNIC));

            if (ModelState.IsValid)
            {
                if (usernameExists)
                {
                    ModelState.AddModelError("", "Username already Exists, Duplicate Username can't be inserted! ");
                    return View();
                }
                if (emailExists)
                {
                    ModelState.AddModelError("", "Email already Exists, if already Registered, Please login! ");
                    return View();
                }
                if (CNICExists)
                {
                    ModelState.AddModelError("", "CNIC already Exists, CNIC can not be duplicate! ");
                    return View();
                }


                else
                {

                    ViewBag.UserRoleId = new SelectList(db.Roles, "RoleID", "RoleName", user.UserRoleId);

                    ViewBag.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;
                    db.Users.Add(user);
                    Session["UserID"] = user.User_ID;
                    Session["UserRoleId"] = ViewBag.UserRoleId;
                    if (Convert.ToInt32(Session["UserID"]) == user.User_ID)
                    {
                        ViewBag.CreatedBy = Session["UserID"];
                    }


                    db.SaveChanges();


                    Session["UserRoleId"] = user.UserRoleId;

                    if (Session["UserRoleId"].ToString() == "2")
                    {

                        Session["UserID"] = user.User_ID;
                        TempData["Message"] = "Thank you for Registeration!";

                        return RedirectToAction("Create", "Doc_Specialization");
                    }

                    else if (Session["UserRoleId"].ToString() == "3")
                    {

                        Session["UserID"] = user.User_ID;
                        TempData["Message"] = "Thank you for Registeration!";

                        return RedirectToAction("Home", "Home");
                    }
                    else
                    {

                        Session["UserID"] = user.User_ID;
                        TempData["Message"] = "Thank you for Registeration!";

                        return RedirectToAction("Index2", "Users");
                    }


                }

            }
            //ViewBag.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //ViewBag.UpdatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); 
            ViewBag.UserRoleId = new SelectList(db.Roles, "RoleID", "RoleName", user.UserRoleId);

            return View(user);

            //return View();
        }


        [CustomAuthenticationFilter]
        [CustomAuthorizationFilter("Admin")]
        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {

            if (Convert.ToInt32(Session["UserRole"]) == 1)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                User user = db.Users.Find(id);
                if (user == null)
                {

                    return HttpNotFound();
                }
                ViewBag.UpdatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                ViewBag.UpdatedBy = Session["UserID"];
                ViewBag.CreatedDate = user.CreatedDate;
                ViewBag.UserRoleId = new SelectList(db.Roles, "RoleID", "RoleName", user.UserRoleId);
                return View(user);
            }
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthenticationFilter]
        [CustomAuthorizationFilter("Admin")]
        public ActionResult Edit([Bind(Include = "User_ID,FullName,UserName,Password,Phone,DOB,Gender,CNIC,Email,City,Address,isActive,CreatedDate,UpdatedDate,CreatedBy,UpdatedBy,UserRoleId")] User user)
        {
            bool usernameExists = db.Users.Any(x => x.UserName.Equals(user.UserName));
            bool emailExists = db.Users.Any(x => x.Email.Equals(user.Email));
            bool CNICExists = db.Users.Any(x => x.CNIC.Equals(user.CNIC));

            if (ModelState.IsValid)
            {
                if (usernameExists)
                {
                    ModelState.AddModelError("", "Username already Exists, Duplicate Username can't be inserted! ");
                    return View();
                }
                if (emailExists)
                {
                    ModelState.AddModelError("", "Email already Exists, if already Registered, Please login! ");
                    return View();
                }
                if (CNICExists)
                {
                    ModelState.AddModelError("", "CNIC already Exists, CNIC can not be duplicate! ");
                    return View();
                }

                ViewBag.UpdatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                ViewBag.CreatedDate = user.CreatedDate;
                ViewBag.UpdatedBy = Session["UserID"];
                db.Entry(user).State = EntityState.Modified;

                db.SaveChanges();
                TempData["Message"] = "Data successfuly edited and saved!";


                return RedirectToAction("Index");

            }
            ViewBag.UpdatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ViewBag.CreatedDate = user.CreatedDate;
            ModelState.AddModelError("", "Edit failed");

            ViewBag.UserRoleId = new SelectList(db.Roles, "RoleID", "RoleName", user.UserRoleId);
            return View(user);
        }


        #region Doc count- List
        [CustomAuthorizationFilter("Admin")]
        public ActionResult ActiveDocList()
        {
            var DocActive = db.Users.Where(x => x.isActive == true && x.UserRoleId == 2).ToList();
            return View(DocActive);
        }

        public ActionResult InActiveDocList()
        {
            var DocInActive = db.sp_InActiveDocList().ToList();
            return View(DocInActive);
        }
        #endregion

        #region Patients count - List
        [CustomAuthorizationFilter("Admin")]
        public ActionResult ActivePatientList()
        {
            var patientsActive = db.sp_ActivePatientsList().ToList();
            return View(patientsActive);
        }
        public ActionResult PatientInActive()
        {
            var patientsInActive = db.sp_InActivePatientsList().ToList();
            return View(patientsInActive);
        }
        #endregion

        #region Users count - LIST
        //Active
        [CustomAuthorizationFilter("Admin")]
        public ActionResult usersActive()
        {
            var usersActive = db.Users.Where(o => o.isActive == true).ToList();
            ViewBag.data = usersActive;
            return View(usersActive);
        }
        //InActive
        public ActionResult usersInActive()
        {
            var usersInActive = db.Users.Where(o => o.isActive == false).ToList();
            return View(usersInActive);
        }
        #endregion



        [CustomAuthenticationFilter]
        [CustomAuthorizationFilter("Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthenticationFilter]
        [CustomAuthorizationFilter("Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }




        public void count()
        {
            ViewBag.DocActiveCount = db.Users.Where(x => x.isActive == true && x.UserRoleId == 2).ToList().Count();
            ViewBag.DocInActiveCount = db.sp_InActiveDocList().Count();
            ViewBag.PatientActiveCount = db.sp_ActivePatientsList().Count();
            ViewBag.PatientInActiveCount = db.sp_InActivePatientsList().Count();
            ViewBag.UsersActiveCount = db.sp_TotalActiveUsers().Count();
            ViewBag.UsersInActiveCount = db.sp_TotalInActiveUsers().Count();


        }
    }
}
