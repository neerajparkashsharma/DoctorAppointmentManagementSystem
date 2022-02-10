using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoctorAppointment_ManagementSystem.Models
{
    public class CustomDate:ValidationAttribute
    {
        public CustomDate() :base("Dear User, Under 18 can't create their profile!")
        {

        }
        public override bool IsValid(object value) 
        {

            DateTime dt = Convert.ToDateTime(value);
            int age = DateTime.Now.Year - dt.Year;
            if ( age < 18)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        
    }
}