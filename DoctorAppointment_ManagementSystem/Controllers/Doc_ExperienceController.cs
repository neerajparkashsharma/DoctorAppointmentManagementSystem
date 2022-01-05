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
    public class Doc_ExperienceController : Controller
    {
        private DoctorAppointment_ManagemenetSystemEntities db = new DoctorAppointment_ManagemenetSystemEntities();

        // GET: Doc_Experience
        public ActionResult Index()
        {
            var doc_Experience = db.Doc_Experience.Include(d => d.User);
            return View(doc_Experience.ToList());
        }

        // GET: Doc_Experience/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doc_Experience doc_Experience = db.Doc_Experience.Find(id);
            if (doc_Experience == null)
            {
                return HttpNotFound();
            }
            return View(doc_Experience);
        }

        // GET: Doc_Experience/Create
        public ActionResult Create()
        {
            ViewBag.DoctorID = Convert.ToInt32(Session["UserID"]);
            ViewBag.DocID = new SelectList(db.Users, "User_ID", "FullName");
            return View();
        }

        // POST: Doc_Experience/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DocID,WorkingFrom")] Doc_Experience doc_Experience)
        {
            if (ModelState.IsValid)
            {
                db.Doc_Experience.Add(doc_Experience);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DocID = new SelectList(db.Users, "User_ID", "FullName", doc_Experience.DocID);
            return View(doc_Experience);
        }

        // GET: Doc_Experience/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doc_Experience doc_Experience = db.Doc_Experience.Find(id);
            if (doc_Experience == null)
            {
                return HttpNotFound();
            }
            ViewBag.DocID = new SelectList(db.Users, "User_ID", "FullName", doc_Experience.DocID);
            return View(doc_Experience);
        }

        // POST: Doc_Experience/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DocID,WorkingFrom")] Doc_Experience doc_Experience)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doc_Experience).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DocID = new SelectList(db.Users, "User_ID", "FullName", doc_Experience.DocID);
            return View(doc_Experience);
        }

        // GET: Doc_Experience/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doc_Experience doc_Experience = db.Doc_Experience.Find(id);
            if (doc_Experience == null)
            {
                return HttpNotFound();
            }
            return View(doc_Experience);
        }

        // POST: Doc_Experience/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Doc_Experience doc_Experience = db.Doc_Experience.Find(id);
            db.Doc_Experience.Remove(doc_Experience);
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
