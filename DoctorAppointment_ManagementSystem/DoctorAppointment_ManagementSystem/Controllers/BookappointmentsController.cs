using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoctorAppointment_ManagementSystem.Models;
using DoctorAppointment_ManagementSystem.Report;

namespace DoctorAppointment_ManagementSystem.Controllers
{

    public class BookappointmentsController : Controller
    {
        private DoctorAppointment_ManagemenetSystemEntities db = new DoctorAppointment_ManagemenetSystemEntities();

        [CustomAuthenticationFilter]
        [CustomAuthorizationFilter("Admin")]
        // GET: Bookappointments
        public ActionResult Index()
        {
            var appointments = db.appointments.Include(a => a.User).Include(a => a.User1).Include(a => a.timeslot);
            ViewBag.totalApp = db.appointments.Include(a => a.User).Include(a => a.User1).Include(a => a.timeslot).Count();
            return View(appointments.ToList());
        }

        public ActionResult Report(appointment app)
        {
            var appointments = db.appointments.Include(a => a.User).Include(a => a.User1).Include(a => a.timeslot);
            appointmentReport appo = new appointmentReport();
            byte[] abytes = appo.prepareReport(appointments.ToList());

            return File(abytes, "applicaton/pdf");

        }


        [CustomAuthenticationFilter]
        [CustomAuthorizationFilter("Admin")]
        // GET: Bookappointments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            appointment appointment = db.appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        [CustomAuthenticationFilter]
        [CustomAuthorizationFilter("Patient")]
        // GET: Bookappointments/Create
        public ActionResult Create(int? Doctor_ID)
        { 
        //{
        //    if (Doctor_ID == null)
        //    {

            
            ViewBag.Doctor_ID = new SelectList(db.Users.Where(x=>x.UserRoleId == 2 && x.isActive == true), "User_ID", "FullName",Doctor_ID);
            //}
            //else
            //{
            //    ViewBag.Doctor_ID = new SelectList(db.Users.Where(x => x.UserRoleId == 2 && x.isActive == true), "User_ID", "FullName");
            //    ViewBag.SelectedDoctor_ID = Doctor_ID;
            //}
            ViewBag.Patient_ID = Session["UserID"];
            ViewBag.SlotID = new SelectList(db.timeslots, "slotID", "slotName");
            return View(new appointment() { Patient_ID = Convert.ToInt32(Session["UserID"]) });
        }

        // POST: Bookappointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
     
        [CustomAuthenticationFilter]
        [CustomAuthorizationFilter("Patient")]
        public ActionResult Create([Bind(Include = "AppID,AppointmentNo,AppDate,Doctor_ID,Patient_ID,SlotID,Status")] appointment appointment)
        {
            if (ModelState.IsValid)
            {
                if (!db.appointments.Any(b => b.SlotID == appointment.SlotID
                   && b.Doctor_ID == appointment.Doctor_ID && b.AppDate == appointment.AppDate))
                {
                    db.appointments.Add(appointment);
                    db.SaveChanges();
                    TempData["appointments"] = appointment;
                    
                    return RedirectToAction("AllAppointments","appointments");
                }
            }
            else
            {
                var appointmentError = "Appointment Date is not available!";
                 ModelState.AddModelError("AppointmentError", appointmentError);

               
            }

            ViewBag.Doctor_ID = new SelectList(db.Users.Where(x => x.UserRoleId == 2), "User_ID", "FullName", appointment.Doctor_ID);

            ViewBag.SlotID = new SelectList(db.timeslots, "slotID", "slotName", appointment.SlotID);
          

            return View(appointment);
        }







        [CustomAuthenticationFilter]
        [CustomAuthorizationFilter("Admin")]
        // GET: Bookappointments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            appointment appointment = db.appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            ViewBag.Doctor_ID = new SelectList(db.Users, "User_ID", "FullName", appointment.Doctor_ID);
            ViewBag.Patient_ID = new SelectList(db.Users, "User_ID", "FullName", appointment.Patient_ID);
            ViewBag.SlotID = new SelectList(db.timeslots, "slotID", "slotName", appointment.SlotID);
            return View(appointment);
        }

        // POST: Bookappointments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthenticationFilter]
        [CustomAuthorizationFilter("Admin")]
        public ActionResult Edit([Bind(Include = "AppID,AppointmentNo,AppDate,Doctor_ID,Patient_ID,SlotID,Status")] appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Doctor_ID = new SelectList(db.Users, "User_ID", "FullName", appointment.Doctor_ID);
            ViewBag.Patient_ID = new SelectList(db.Users, "User_ID", "FullName", appointment.Patient_ID);
            ViewBag.SlotID = new SelectList(db.timeslots, "slotID", "slotName", appointment.SlotID);
            return View(appointment);
        }

        [CustomAuthenticationFilter]
        [CustomAuthorizationFilter("Admin")]

        // GET: Bookappointments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            appointment appointment = db.appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: Bookappointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [CustomAuthenticationFilter]
        [CustomAuthorizationFilter("Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            appointment appointment = db.appointments.Find(id);
            db.appointments.Remove(appointment);
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
