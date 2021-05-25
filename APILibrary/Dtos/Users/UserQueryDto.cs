using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APILibrary.Models.Users
{
    public class UserQueryDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string RegisterDate { get; set; }

        public bool IsEmpty => Name is null && Email is null;
    }
}
