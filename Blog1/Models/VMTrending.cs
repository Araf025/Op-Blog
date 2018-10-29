using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog1.Models
{
    public class VMTrending
    {
        [Key]
        public int vmt { get; set; }
        public Blog Blog { get; set; }
        public virtual IEnumerable<Blog> Blogsz { get; set; }
    }
}