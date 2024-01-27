using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReCaptcha.Models
{
    public class ContactUs
    {
        [Required]
        public string name { get; set; }
        [Required]
        public string address { get; set; }
        public string note { get; set; }
        [Required]
        public string recapcha { get; set; }
    }
}