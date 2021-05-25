using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APILibrary.Models.Posts
{
    public class PostQueryDto
    {
        public bool? Hidden { get; set; }

        public string CreatedBy { get; set; }

        public bool IsEmpty => Hidden is null && CreatedBy is null;
    }
}
