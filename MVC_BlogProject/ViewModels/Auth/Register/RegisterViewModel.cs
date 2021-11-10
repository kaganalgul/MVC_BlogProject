using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_BlogProject.ViewModels.Auth.Register
{
    public class RegisterViewModel
    {
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [RegularExpression(@"^[a-zA-Z]\w{8,14}$", ErrorMessage = "Your password should be at least 8 characters")]
        public string Password { get; set; }
    }
}
