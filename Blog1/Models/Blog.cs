using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace Blog1.Models
{
    public class Blog
    {
        [Key]
        public int BlogId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Content { get; set; }
        public string ThumbnailImageUrl { get; set; }
        public category Category { get; set; }
        public string Author { get; set; }
        public bool Status { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; } 
        public virtual ICollection<BlogComment> BlogComments { get; set; }
        public virtual ICollection<BlogLike> BlogLikes{ get; set; }
    }
    public enum category
    {
        Movies,
        Art,
        Games,
        Animals,
        Supers,
        Technologies
    }
}