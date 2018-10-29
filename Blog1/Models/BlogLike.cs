using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog1.Models
{
    public class BlogLike
    {
        [Key]
        public int LikeId { get; set; }
        public int BlogId { get; set; }
        public string LikeAuthor { get; set; }
        public bool Like { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual Blog Blog { get; set; }
    }
}