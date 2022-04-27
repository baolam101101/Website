using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Models
{
    public class Post
    {
        public Post()
        {
            this.Anonymous = true;
            this.Time = DateTime.Now;
            this.Comment = new HashSet<Comment>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Time { get; set; }
        public string AuthorId { get; set; }
        public virtual ApplicationUser Author { get; set; }
        public string AuthorName { get; set; }
        public string Content { get; set; }
        public bool Anonymous { get; set; }
        public string DocumentName { get; set; }
        public int submissionID { get; set; }   
        public int categoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual ICollection<Comment> Comment { get; set; }
    }
}