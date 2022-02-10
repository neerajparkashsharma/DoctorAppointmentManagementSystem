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
    public class AppointmentPaymentsController : Controller
    {
        private DoctorAppointment_ManagemenetSystemEntities db = new DoctorAppointment_ManagemenetSystemEntities();

        // GET: AppointmentPayments
      [CustomAuthenticationFilter]
      [CustomAuthorizationFilter("Admin")]

        [CustomAuthorizationFilter("Doctor")]
        public ActionResult Index()
        {
            var appointmentPayments = db.AppointmentPayments.Include(a => a.appointment);
            return View(appointmentPayments.ToList());
        }

        // GET: AppointmentPayments/Details/5
        [CustomAuthenticationFilter]
        [CustomAuthorizationFilter("Admin")]

        [CustomAuthorizationFilter("Doctor")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppointmentPayment appointmentPayment = db.AppointmentPayments.Find(id);
            if (appointmentPayment == null)
            {
                return HttpNotFound();
            }
            return View(appointmentPayment);
        }

        // GET: AppointmentPayments/Create
        [CustomAuthenticationFilter]
        [CustomAuthorizationFilter("Admin")]

        [CustomAuthorizationFilter("Doctor")]
        public ActionResult Create(int? AppointmentId)
        {

            var list = db.appointments.Include(x => x.AppointmentPayments).ToList().Where(x => x.AppointmentPayments.Count() == 0 || x.AppointmentPayments == null && x.Doctor_ID == Convert.ToInt32(Session["UserId"])/*.Select(y => y.IsPaid == false || y.IsPaid == null).FirstOrDefault() && x.Doctor_ID == Convert.ToInt32(Session["UserId"])*/).ToList();

            /// if payment is done, then go to view.. 
            ViewBag.AppointmentId = new SelectList(list, "AppID", "AppointmentNo");
            return View();
        }

        // POST: AppointmentPayments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthenticationFilter]
        [CustomAuthorizationFilter("Admin")]

        [CustomAuthorizationFilter("Doctor")]
        public ActionResult Create([Bind(Include = "AppointmentPaymentId,AppointmentId,Fees,IsPaid,PaidOn")] AppointmentPayment appointmentPayment)
        {
            if (ModelState.IsValid)
            {
                if (appointmentPayment.IsPaid == true && appointmentPayment.PaidOn == null)
                {
                    appointmentPayment.PaidOn = DateTime.Now;
                }

                db.AppointmentPayments.Add(appointmentPayment);
                db.SaveChanges();
               

                    return RedirectToAction("Index","Appointments");
            }

            var list = db.appointments.Include(x => x.AppointmentPayments).ToList().Where(x => x.AppointmentPayments.Count() == 0 || x.AppointmentPayments == null && x.Doctor_ID == Convert.ToInt32(Session["UserId"])/*.Select(y => y.IsPaid == false || y.IsPaid == null).FirstOrDefault() && x.Doctor_ID == Convert.ToInt32(Session["UserId"])*/).ToList();


            ViewBag.AppointmentId = new SelectList(list, "AppID", "AppointmentNo", appointmentPayment.AppointmentId);
            return View(appointmentPayment);
        }

        [CustomAuthenticationFilter]
        [CustomAuthorizationFilter("Admin")]

        [CustomAuthorizationFilter("Doctor")]
        // GET: AppointmentPayments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppointmentPayment appointmentPayment = db.AppointmentPayments.Find(id);
            if (appointmentPayment == null)
            {
                return HttpNotFound();
            }
            ViewBag.AppointmentId = new SelectList(db.appointments.ToList().Select(x => x.AppointmentPayments.Where(yx => yx.IsPaid == false && yx.appointment.Doctor_ID == Convert.ToInt32(Session["UserId"]))), appointmentPayment.AppointmentId);
            return View(appointmentPayment);
        }

        // POST: AppointmentPayments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthenticationFilter]
        [CustomAuthorizationFilter("Admin")]

        [CustomAuthorizationFilter("Doctor")]
        public ActionResult Edit([Bind(Include = "AppointmentPaymentId,AppointmentId,Fees,IsPaid,PaidOn")] AppointmentPayment appointmentPayment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointmentPayment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AppointmentId = new SelectList(db.appointments.ToList().Select(x => x.AppointmentPayments.Where(yx => yx.IsPaid == false && yx.appointment.Doctor_ID == Convert.ToInt32(Session["UserId"]))), "AppID", "AppointmentNo", appointmentPayment.AppointmentId);
            return View(appointmentPayment);
        }

        // GET: AppointmentPayments/Delete/5
        [CustomAuthenticationFilter]
        [CustomAuthorizationFilter("Admin")]

        [CustomAuthorizationFilter("Doctor")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppointmentPayment appointmentPayment = db.AppointmentPayments.Find(id);
            if (appointmentPayment == null)
            {
                return HttpNotFound();
            }
            return View(appointmentPayment);
        }

        // POST: AppointmentPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AppointmentPayment appointmentPayment = db.AppointmentPayments.Find(id);
            db.AppointmentPayments.Remove(appointmentPayment);
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
