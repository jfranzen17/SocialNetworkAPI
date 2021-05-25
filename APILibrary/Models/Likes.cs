using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using APILibrary.Models.Posts;
using APILibrary.Models.Users;

namespace APILibrary.Models.Likes
{
    public class Like
    {
        public Like(int postId, User user)
        {
            PostId = postId;
            User = user;
            LastDate = LastDate;
        }

        public Guid Guid { get; private set; } = Guid.NewGuid();

        public int PostId { get; set; }

        [Required]
        public User User { get; set; }

        public DateTime LastDate { get; set; } = DateTime.Now;
    }
}
