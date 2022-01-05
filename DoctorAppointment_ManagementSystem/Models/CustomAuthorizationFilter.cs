using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DoctorAppointment_ManagementSystem.Models
{
    public class CustomAuthorizationFilter:AuthorizeAttribute
    {
        private readonly string[] allowedroles;
        public CustomAuthorizationFilter(params string[] roles)
        {
            this.allowedroles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            var UserName = Convert.ToString(httpContext.Session["UserName"]);
            if (!string.IsNullOrEmpty(UserName))
                using (var context = new DoctorAppointment_ManagemenetSystemEntities())
                {
                    var userRole = (from u in context.Users
                                    join r in context.Roles on u.UserRoleId equals r.RoleID
                                    where u.UserName == UserName
                                    select new
                                    {
                                        r.RoleName
                                    }).FirstOrDefault();
                    foreach (var role in allowedroles)
                    {
                        if (role == userRole.RoleName) return true;
                    }
                }


            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
               new RouteValueDictionary
               {
                    { "controller", "Account" },
                    { "action", "UnAuthorized" }
               });
        }
    }
}