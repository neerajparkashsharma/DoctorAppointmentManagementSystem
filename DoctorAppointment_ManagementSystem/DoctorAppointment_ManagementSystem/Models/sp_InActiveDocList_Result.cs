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
    
    public partial class sp_InActiveDocList_Result
    {
        public int User_ID { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public System.DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string CNIC { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public bool isActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public int UserRoleId { get; set; }
    }
}
