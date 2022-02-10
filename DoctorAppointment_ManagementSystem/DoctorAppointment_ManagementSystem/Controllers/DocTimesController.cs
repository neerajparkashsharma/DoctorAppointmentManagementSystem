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
    [CustomAuthenticationFilter]
    [CustomAuthorizationFilter("Admin")]
    public class DocTimesController : Controller
    {
        private DoctorAppointment_ManagemenetSystemEntities db = new DoctorAppointment_ManagemenetSystemEntities();

        // GET: DocTimes
        public ActionResult Index()
        {
            var docTimes = db.DocTimes.Include(d => d.User).Include(d => d.timeslot);
            return View(docTimes.ToList());
        }

        // GET: DocTimes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocTime docTime = db.DocTimes.Find(id);
            if (docTime == null)
            {
                return HttpNotFound();
            }
            return View(docTime);
        }

        // GET: DocTimes/Create
        public ActionResult Create()
        {
            ViewBag.DoctorID = new SelectList(db.Users, "User_ID", "FullName");
            ViewBag.slotID = new SelectList(db.timeslots, "slotID", "slotName");
            return View();
        }

        // POST: DocTimes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DoctorID,slotID,isBooked")] DocTime docTime)
        {
            if (ModelState.IsValid)
            {
                db.DocTimes.Add(docTime);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DoctorID = new SelectList(db.Users, "User_ID", "FullName", docTime.DoctorID);
            ViewBag.slotID = new SelectList(db.timeslots, "slotID", "slotName", docTime.slotID);
            return View(docTime);
        }

        // GET: DocTimes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocTime docTime = db.DocTimes.Find(id);
            if (docTime == null)
            {
                return HttpNotFound();
            }
            ViewBag.DoctorID = new SelectList(db.Users, "User_ID", "FullName", docTime.DoctorID);
            ViewBag.slotID = new SelectList(db.timeslots, "slotID", "slotName", docTime.slotID);
            return View(docTime);
        }

        // POST: DocTimes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DoctorID,slotID,isBooked")] DocTime docTime)
        {
            if (ModelState.IsValid)
            {
                db.Entry(docTime).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DoctorID = new SelectList(db.Users, "User_ID", "FullName", docTime.DoctorID);
            ViewBag.slotID = new SelectList(db.timeslots, "slotID", "slotName", docTime.slotID);
            return View(docTime);
        }

        // GET: DocTimes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocTime docTime = db.DocTimes.Find(id);
            if (docTime == null)
            {
                return HttpNotFound();
            }
            return View(docTime);
        }

        // POST: DocTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DocTime docTime = db.DocTimes.Find(id);
            db.DocTimes.Remove(docTime);
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
