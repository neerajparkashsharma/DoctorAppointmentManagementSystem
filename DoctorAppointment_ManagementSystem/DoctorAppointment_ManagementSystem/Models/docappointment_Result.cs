//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DoctorAppointment_ManagementSystem.Models
{
    using System;
    
    public partial class docappointment_Result
    {
        public int AppID { get; set; }
        public string AppointmentNo { get; set; }
        public System.DateTime AppDate { get; set; }
        public int Doctor_ID { get; set; }
        public int Patient_ID { get; set; }
        public int SlotID { get; set; }
        public bool Status { get; set; }
    }
}
