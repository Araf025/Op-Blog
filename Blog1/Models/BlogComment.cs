using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog1.Models
{
    public class BlogComment
    {
        [Key]
        public int CommentId { get; set; }
        public int BlogId { get; set; }
        public string CommentAuthor { get; set; }
        [Required]
        public string Comment { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual Blog Blog { get; set; }
    }
}