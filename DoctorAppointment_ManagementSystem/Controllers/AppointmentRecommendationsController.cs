﻿using System;
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
    public class AppointmentRecommendationsController : Controller
    {
        private DoctorAppointment_ManagemenetSystemEntities db = new DoctorAppointment_ManagemenetSystemEntities();

        // GET: AppointmentRecommendations
        [CustomAuthenticationFilter]
        [CustomAuthorizationFilter("Doctor")]
        public ActionResult Index()
        {
            var appointmentRecommendations = db.AppointmentRecommendations.Include(a => a.appointment);
            return View(appointmentRecommendations.ToList());
        }

        // GET: AppointmentRecommendations/Details/5
        [CustomAuthenticationFilter]
        [CustomAuthorizationFilter("Doctor")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppointmentRecommendation appointmentRecommendation = db.AppointmentRecommendations.Find(id);
            if (appointmentRecommendation == null)
            {
                return HttpNotFound();
            }
            return View(appointmentRecommendation);
        }

        // GET: AppointmentRecommendations/Create

        [CustomAuthenticationFilter]
        [CustomAuthorizationFilter("Doctor")]
        public ActionResult Create(int? AppointmentId)
        {
           
            ViewBag.AppointmentId = new SelectList(db.appointments.ToList().Where(x=>x.Doctor_ID == Convert.ToInt32(Session["UserId"])), "AppID", "AppointmentNo", AppointmentId);

         
            return View();
        }

        // POST: AppointmentRecommendations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthenticationFilter]
        [CustomAuthorizationFilter("Doctor")]
        public ActionResult Create([Bind(Include = "AppointmentRecommendationsId,Recommendation,AppointmentId")] AppointmentRecommendation appointmentRecommendation)
        {
            if (ModelState.IsValid)
            {
                db.AppointmentRecommendations.Add(appointmentRecommendation);
                db.SaveChanges();

             
                return RedirectToAction("Index");


            }

            ViewBag.AppointmentId = new SelectList(db.appointments.ToList().Where(x=>x.Doctor_ID == Convert.ToInt32(Session["UserId"])  ), "AppID", "AppointmentNo", appointmentRecommendation.AppointmentId);
            return View(appointmentRecommendation);
        }
        [CustomAuthenticationFilter]
        [CustomAuthorizationFilter("Doctor")]
        // GET: AppointmentRecommendations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppointmentRecommendation appointmentRecommendation = db.AppointmentRecommendations.Find(id);
            if (appointmentRecommendation == null)
            {
                return HttpNotFound();
            }
            ViewBag.AppointmentId = new SelectList(db.appointments, "AppID", "AppointmentNo", appointmentRecommendation.AppointmentId);
            return View(appointmentRecommendation);
        }

        // POST: AppointmentRecommendations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthenticationFilter]
        [CustomAuthorizationFilter("Doctor")]
        public ActionResult Edit([Bind(Include = "AppointmentRecommendationsId,Recommendation,AppointmentId")] AppointmentRecommendation appointmentRecommendation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointmentRecommendation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AppointmentId = new SelectList(db.appointments, "AppID", "AppointmentNo", appointmentRecommendation.AppointmentId);
            return View(appointmentRecommendation);
        }

        // GET: AppointmentRecommendations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppointmentRecommendation appointmentRecommendation = db.AppointmentRecommendations.Find(id);
            if (appointmentRecommendation == null)
            {
                return HttpNotFound();
            }
            return View(appointmentRecommendation);
        }

        // POST: AppointmentRecommendations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AppointmentRecommendation appointmentRecommendation = db.AppointmentRecommendations.Find(id);
            db.AppointmentRecommendations.Remove(appointmentRecommendation);
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
