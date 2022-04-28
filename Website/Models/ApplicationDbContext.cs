using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Website.Models
{
    
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public IDbSet<Post> Post { get; set; }

        public IDbSet<Comment> Comment { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Website.Models.Department> Departments { get; set; }

        public System.Data.Entity.DbSet<Website.Models.Submission> Submissions { get; set; }

        public System.Data.Entity.DbSet<Website.Models.Category> Categories { get; set; }

    }
}