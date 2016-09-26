using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtConspiracy.Models
{
    public class RegisterModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string FullName { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Timestamp]
        public DateTime JoinDate { get; set; }
    }
}