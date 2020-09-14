using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Models.ViewModel.User
{
    public class UserViewModel
    {
        public string Name { get; set; }
        public string Famili { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
        public string UserName { get; set; }
    }
}
