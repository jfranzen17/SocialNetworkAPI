using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APILibrary.Models.Posts
{
    public class PostDto
    {
        private const string _rangeMessage = "{0} must be between {1} and {2}";
        private const string _stringMessage = "{0} must be between {2} and {1} characters long";

        [Required]
        public int PostId { get; private set; }

        [Required]
        [StringLength(400, ErrorMessage = _stringMessage, MinimumLength = 5)]
        public string Content { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        [Range(typeof(DateTime), "1/1/2020", "1/1/2022", ErrorMessage = _rangeMessage)]
        public DateTime LastDate { get; set; } = DateTime.UtcNow;

        public List<int> LikedBy { get; set; } = new List<int>();
    }
}
