using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog1.Models
{
    public class BlogSection
    {
        [Key]
        public int BlogSectionId { get; set; }
        public int BlogId { get; set; }
        public string SectionTitle { get; set; }
        public string SectionContent { get; set; }
        public string ImageUrl { get; set; }
        //public virtual Blog Blog { get; set; }
    }
}