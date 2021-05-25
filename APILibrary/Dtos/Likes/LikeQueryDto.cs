using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APILibrary.Models.Likes
{
    public class LikeQueryDto
    {
        public int LikeId { get; set; }

        public int PostId { get; set; }

        public string User { get; set; }

        public string Email { get; set; }

        public string LikeDate { get; set; }

        public bool IsEmpty => User is null && LikeDate is null;
    }
}
