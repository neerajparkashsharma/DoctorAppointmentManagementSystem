using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoctorAppointment_ManagementSystem.Models;
using System.Web.Security;
using System.Web.Routing;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorAppointment_ManagementSystem.Controllers
{
    [CustomAuthenticationFilter]
    public class appointmentsController : Controller
    {

       
        public void InitializeController(RequestContext context)
        {
            base.Initialize(context);
        }

        private DoctorAppointment_ManagemenetSystemEntities db = new DoctorAppointment_ManagemenetSystemEntities();




        [CustomAuthenticationFilter]
        public ActionResult Today()
        {
            var data = db.appointments.ToList().Where(x=>x.AppDate.ToString("dd-MM-yyyy") == DateTime.Now.ToString("dd-MM-yyyy") && x.Doctor_ID == Convert.ToInt32(Session["UserID"]));
            ViewBag.data = data;
            return View();

        }
        [CustomAuthenticationFilter]

        public ActionResult Index()
        {
            var data = db.appointments.ToList();

            if (Convert.ToInt32(Session["UserRole"]) == 2)
            {
                 data = data.OrderBy(a => a.AppDate.ToString()).Where(a => a.Doctor_ID == Convert.ToInt32(Session["UserID"])).ToList();


            }
            else if (Convert.ToInt32(Session["UserRole"]) == 3)
            {
                 data = data.OrderBy(a => a.AppDate.ToString()).ToList().OrderBy(a => a.AppDate.ToString()).Where(a => a.Patient_ID == Convert.ToInt32(Session["UserID"])).ToList();


            }

            ViewBag.data = data;
            return View();

        }

        public ActionResult Bill(int Id)
        {
            ViewBag.data = db.appointments.ToList().Where(x => x.AppID == Id && (x.Patient_ID == Convert.ToInt32(Session["UserID"]) || x.Doctor_ID == Convert.ToInt32(Session["UserID"]))).FirstOrDefault();

            ViewBag.Paymentdata = db.AppointmentPayments.ToList().Where(x => x.AppointmentId == Id && (x.appointment.Patient_ID == Convert.ToInt32(Session["UserID"]) || x.appointment.Doctor_ID == Convert.ToInt32(Session["UserID"]))).FirstOrDefault();
            return View();
        }

        [CustomAuthenticationFilter]

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
        public ActionResult New(int? Doctor_ID)
        {
            //{
            //    if (Doctor_ID == null)
            //    {


            

            ViewBag.Doctor_ID = new SelectList(db.Users.Where(x => x.UserRoleId == 2 && x.isActive == true), "User_ID","UserName", Doctor_ID);
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
        public ActionResult New([Bind(Include = "AppID,AppointmentNo,AppDate,Doctor_ID,Patient_ID,SlotID,Status")] appointment appointment)
        {
            if (ModelState.IsValid)
            {
                if (!db.appointments.Any(b => b.SlotID == appointment.SlotID
                   && b.Doctor_ID == appointment.Doctor_ID && b.AppDate == appointment.AppDate))
                {
                    appointment.StatusId = 1;
                    db.appointments.Add(appointment);
                    db.SaveChanges();
                    TempData["appointments"] = appointment;

                    return RedirectToAction("index", "appointments");
                }
                else 
                {
                    var err = "Slot already Booked!";
                    ModelState.AddModelError("AppointmentError", err);

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


        [CustomAuthenticationFilter]

        public ActionResult AppointmentRecommendations(int Id)
        {
            ViewBag.data = db.appointments.ToList().Where(x => x.AppID == Id && (x.Patient_ID == Convert.ToInt32(Session["UserID"]) || x.Doctor_ID == Convert.ToInt32(Session["UserID"]))).FirstOrDefault();
            ViewBag.list = db.AppointmentRecommendations.ToList().Where(x => x.AppointmentId == Id && (x.appointment.Patient_ID == Convert.ToInt32(Session["UserID"]) || x.appointment.Doctor_ID == Convert.ToInt32(Session["UserID"]))).ToList();
            return View();
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



    //    [CustomAuthorizationFilter("Admin")]
    //    // GET: appointments/Details/5
    //    public ActionResult Details(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }
    //        appointment appointment = db.appointments.Find(id);
    //        if (appointment == null)
    //        {
    //            return HttpNotFound();
    //        }
    //        return View(appointment);
    //    }


    //    [CustomAuthorizationFilter("Patient", "Admin")]

    //    // GET: appointments/Create
    //    public ActionResult Create(int? id)
    //    {


    //        ViewBag.Doctor_ID = new SelectList(db.Users.Where(o => o.UserRoleId == 2 && o.isActive == true), "User_ID", "FullName") ;

    //        ViewBag.SlotID = new SelectList(db.timeslots, "slotID", "slotName");
    //        ViewBag.PatientID = Session["UserID"];
    //        return View(new appointment() { Patient_ID = Convert.ToInt32(Session["UserID"]) } );
    //    }


    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    [CustomAuthenticationFilter]
    //    [CustomAuthorizationFilter("Patient","Admin")]

    //    public ActionResult Create([Bind(Include = "AppID,AppointmentNo,AppDate,Doctor_ID,Patient_ID,SlotID,Status")] appointment appointment)
    //    {


    //        if (ModelState.IsValid)
    //        {

    //        if (!db.appointments.Any(b => b.SlotID == appointment.SlotID
    //                 && b.Doctor_ID == appointment.Doctor_ID && b.AppDate == appointment.AppDate ))
    //        {


    //            db.appointments.Add(appointment);
    //            db.SaveChanges();
    //            TempData["appointments"] = appointment;
    //            ViewBag.Message = "Appointnment Created Successfully!";


    //             return RedirectToAction("patientAppointments");

    //        }
    //        }

    //        ViewBag.Doctor_ID = new SelectList(db.Users.Where(o => o.UserRoleId == 2 && o.isActive == true), "User_ID", "FullName");
    //        ViewBag.SlotID = new SelectList(db.DocTimes, "SlotID", "slotName");

    //        ModelState.AddModelError("", "Slot Already Booked!");
    //         return View(appointment);





    //    }

    //    [CustomAuthenticationFilter]
    //    [CustomAuthorizationFilter("Admin")]
    //    // GET: appointments/Edit/5
    //    public ActionResult Edit(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }
    //        appointment appointment = db.appointments.Find(id);
    //        if (appointment == null)
    //        {
    //            return HttpNotFound();
    //        }
    //        ViewBag.Doctor_ID = new SelectList(db.Users, "User_ID", "FullName", appointment.Doctor_ID);
    //        ViewBag.Patient_ID = new SelectList(db.Users, "User_ID", "FullName", appointment.Patient_ID);
    //        ViewBag.SlotID = new SelectList(db.timeslots, "slotID", "slotName", appointment.SlotID);
    //        return View(appointment);
    //    }

    //    // POST: appointments/Edit/5
    //    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    //    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    [CustomAuthenticationFilter]
    //    [CustomAuthorizationFilter("Admin")]
    //    public ActionResult Edit([Bind(Include = "AppID,AppointmentNo,AppDate,Doctor_ID,Patient_ID,SlotID,Status")] appointment appointment)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            db.Entry(appointment).State = EntityState.Modified;
    //            db.SaveChanges();
    //            return RedirectToAction("Index");
    //        }

    //        ViewBag.Doctor_ID = new SelectList(db.Users, "User_ID", "FullName", appointment.Doctor_ID);
    //        ViewBag.Patient_ID = new SelectList(db.Users, "User_ID", "FullName", appointment.Patient_ID);
    //        ViewBag.SlotID = new SelectList(db.timeslots, "slotID", "slotName", appointment.SlotID);
    //        return View(appointment);
    //    }

    //    [CustomAuthenticationFilter]
    //    [CustomAuthorizationFilter("Admin")]
    //    // GET: appointments/Delete/5
    //    public ActionResult Delete(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }
    //        appointment appointment = db.appointments.Find(id);
    //        if (appointment == null)
    //        {
    //            return HttpNotFound();
    //        }
    //        return View(appointment);
    //    }

    //        [CustomAuthenticationFilter]
    //    [CustomAuthorizationFilter("Admin")]
    //    // POST: appointments/Delete/5
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult DeleteConfirmed(int id)
    //    {
    //        appointment appointment = db.appointments.Find(id);
    //        db.appointments.Remove(appointment);
    //        db.SaveChanges();
    //        return RedirectToAction("Index");
    //    }

    //    protected override void Dispose(bool disposing)
    //    {
    //        if (disposing)
    //        {
    //            db.Dispose();
    //        }
    //        base.Dispose(disposing);
    //    }
    //}


   
}
