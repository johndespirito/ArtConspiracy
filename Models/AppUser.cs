using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtConspiracy.Models
{
    public class AppUser : IdentityUser
    {
        public string city { get; set; }
        public string state { get; set; }
        public string fullName { get; set; }
        public DateTime joinDate { get; set; }
    }
}