using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace app.shared.Libs.DTOs.User
{
    public class UserDTO
    {
        public string Username { get; set; } = string.Empty;
         public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}