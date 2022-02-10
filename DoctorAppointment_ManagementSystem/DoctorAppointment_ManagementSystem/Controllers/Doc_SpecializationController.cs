using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoctorAppointment_ManagementSystem.Models;

namespace DoctorAppointment_ManagementSystem.Controllers
{
    public class Doc_SpecializationController : Controller
    {
        private DoctorAppointment_ManagemenetSystemEntities db = new DoctorAppointment_ManagemenetSystemEntities();
        [AllowAnonymous]
        // GET: Doc_Specialization
        public ActionResult Index()
        {
            var doc_Specialization = db.Doc_Specialization.Include(d => d.User).Include(d => d.Specialization);
            return View(doc_Specialization.ToList());
        }

        [CustomAuthenticationFilter]
        [CustomAuthorizationFilter("Doctor")]
        // GET: Doc_Specialization/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doc_Specialization doc_Specialization = db.Doc_Specialization.Find(id);
            if (doc_Specialization == null)
            {
                return HttpNotFound();
            }
            return View(doc_Specialization);
        }



        public ActionResult GetDoctorsSpecializations( int id )
        {
            ViewBag.specializations = db.Doc_Specialization.ToList().Where(x => x.DoctorID == id).ToList() ;
            return View();
        }


        [CustomAuthenticationFilter]
        [CustomAuthorizationFilter("Doctor")]
        // GET: Doc_Specialization/Create
        public ActionResult Create()
        {
            ViewBag.DoctorID = Convert.ToInt32(Session["UserID"]);
            ViewBag.SpecializationId = new SelectList(db.Specializations, "SpecializationId", "specializationName");
            return View(new Doc_Specialization { DoctorID = Convert.ToInt32(Session["UserID"]) });
        }

        // POST: Doc_Specialization/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthenticationFilter]
        [CustomAuthorizationFilter("Doctor")]
        public ActionResult Create([Bind(Include = "ID,SpecializationId,DoctorID")] Doc_Specialization doc_Specialization)
        {
            if (ModelState.IsValid)
            {
                db.Doc_Specialization.Add(doc_Specialization);
                db.SaveChanges();
                if (Session["UserRoleId"].ToString() == "2")
                {
                    return RedirectToAction("Create","Doc_Experience");
                }
            }

       
            ViewBag.SpecializationId = new SelectList(db.Specializations, "SpecializationId", "specializationName", doc_Specialization.SpecializationId);
            return View(doc_Specialization);
        }

        [CustomAuthenticationFilter]
        [CustomAuthorizationFilter("Doctor")]
        // GET: Doc_Specialization/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doc_Specialization doc_Specialization = db.Doc_Specialization.Find(id);
            if (doc_Specialization == null)
            {
                return HttpNotFound();
            }
            ViewBag.DoctorID = new SelectList(db.Users, "User_ID", "FullName", doc_Specialization.DoctorID);
            ViewBag.SpecializationId = new SelectList(db.Specializations, "SpecializationId", "specializationName", doc_Specialization.SpecializationId);
            return View(doc_Specialization);
        }

        // POST: Doc_Specialization/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthenticationFilter]
        [CustomAuthorizationFilter("Doctor")]
        public ActionResult Edit([Bind(Include = "ID,SpecializationId,DoctorID")] Doc_Specialization doc_Specialization)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doc_Specialization).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DoctorID = new SelectList(db.Users, "User_ID", "FullName", doc_Specialization.DoctorID);
            ViewBag.SpecializationId = new SelectList(db.Specializations, "SpecializationId", "specializationName", doc_Specialization.SpecializationId);
            return View(doc_Specialization);
        }

        [CustomAuthenticationFilter]
        [CustomAuthorizationFilter("Doctor")]
        // GET: Doc_Specialization/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doc_Specialization doc_Specialization = db.Doc_Specialization.Find(id);
            if (doc_Specialization == null)
            {
                return HttpNotFound();
            }
            return View(doc_Specialization);
        }

        // POST: Doc_Specialization/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthenticationFilter]
        [CustomAuthorizationFilter("Doctor")]
        public ActionResult DeleteConfirmed(int id)
        {
            Doc_Specialization doc_Specialization = db.Doc_Specialization.Find(id);
            db.Doc_Specialization.Remove(doc_Specialization);
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
    }
}
