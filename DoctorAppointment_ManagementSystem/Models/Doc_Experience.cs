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
    
    public partial class Doc_Experience
    {
        public int ID { get; set; }
        public int DocID { get; set; }
        public System.DateTime WorkingFrom { get; set; }
    
        public virtual User User { get; set; }
    }
}
