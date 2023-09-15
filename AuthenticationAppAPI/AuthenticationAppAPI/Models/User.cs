using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthenticationAppAPI.Models
{
    public class User
    {
        public Guid Id { set; get; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string Photo { get; set; }
        public int PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
    }
}