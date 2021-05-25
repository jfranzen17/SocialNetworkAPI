using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace APILibrary.Models.Users
{
    public class UserDto
    {
        public int UserId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
    }
}
