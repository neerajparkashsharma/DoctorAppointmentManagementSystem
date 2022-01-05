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
    public class timeslotsController : Controller
    {
        private DoctorAppointment_ManagemenetSystemEntities db = new DoctorAppointment_ManagemenetSystemEntities();

        // GET: timeslots
        public ActionResult Index()
        {
            return View(db.timeslots.ToList());
        }

        // GET: timeslots/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            timeslot timeslot = db.timeslots.Find(id);
            if (timeslot == null)
            {
                return HttpNotFound();
            }
            return View(timeslot);
        }

        // GET: timeslots/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: timeslots/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "slotID,slotName,Date,start_time,end_time")] timeslot timeslot)
        {
            if (ModelState.IsValid)
            {
                db.timeslots.Add(timeslot);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(timeslot);
        }

        // GET: timeslots/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            timeslot timeslot = db.timeslots.Find(id);
            if (timeslot == null)
            {
                return HttpNotFound();
            }
            return View(timeslot);
        }

        // POST: timeslots/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "slotID,slotName,Date,start_time,end_time")] timeslot timeslot)
        {
            if (ModelState.IsValid)
            {
                db.Entry(timeslot).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(timeslot);
        }

        // GET: timeslots/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            timeslot timeslot = db.timeslots.Find(id);
            if (timeslot == null)
            {
                return HttpNotFound();
            }
            return View(timeslot);
        }

        // POST: timeslots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            timeslot timeslot = db.timeslots.Find(id);
            db.timeslots.Remove(timeslot);
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
