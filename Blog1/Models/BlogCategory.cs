using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Blog1.Models
{
    public class BlogCategory
    {
        [Key]
        [Column(Order = 1)]
        public int CategoryId { get; set; }
  
        [Key]
        [Column(Order = 2)]
        public int BlogId { get; set; }

    }
}