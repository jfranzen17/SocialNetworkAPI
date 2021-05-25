using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APILibrary.Models.Users
{
    public class User
    {
        public User(int id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }

        public int Id { get; set; }

        public Guid Guid { get; private set; } = Guid.NewGuid();

        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public DateTime RegisterDate { get; private set; } = DateTime.UtcNow;
    }
}
