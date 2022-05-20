using Microsoft.EntityFrameworkCore;
using DemoJWT.Models;

namespace DemoJWT.Context
{
    public class ApplicationDBContext: DbContext
    {
       public ApplicationDBContext(DbContextOptions<ApplicationDBContext>options): base(options)
        {

        }

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<User_Activity> Activity { get; set; }
    }
}
