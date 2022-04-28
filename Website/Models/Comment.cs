using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Website.Models;
using System.ComponentModel.DataAnnotations;

namespace Website.Models
{
    public class Comment
    {
        public Comment()
        {
            this.Time = DateTime.Now;
        }

        public int Id { get; set; }
        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime Time { get; set; }
        public string AuthorId { get; set; }
        public virtual ApplicationUser Author { get; set; }
        public bool Anonymous { get; set; }
        public int IdeaId { get; set; }
        public int submissionid { get; set; }
        public virtual Post Post { get; set; }
    }
}