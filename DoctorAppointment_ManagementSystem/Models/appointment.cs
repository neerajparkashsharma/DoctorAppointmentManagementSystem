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
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class appointment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public appointment()
        {
            this.AppointmentPayments = new HashSet<AppointmentPayment>();
            this.AppointmentRecommendations = new HashSet<AppointmentRecommendation>();
        }
    
        public int AppID { get; set; }
        public string AppointmentNo { get; set; }
        [CustomAppointmentDate]
        [DataType(DataType.Date)]
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime AppDate { get; set; }
        public int Doctor_ID { get; set; }
        public int Patient_ID { get; set; }
        public int SlotID { get; set; }
        public Nullable<int> StatusId { get; set; }
        public string Reason { get; set; }
    
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
        public virtual timeslot timeslot { get; set; }
        public virtual AppointmentStatu AppointmentStatu { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppointmentPayment> AppointmentPayments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppointmentRecommendation> AppointmentRecommendations { get; set; }
    }
}
