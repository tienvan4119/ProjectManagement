using Microsoft.EntityFrameworkCore;

namespace ProjectManager.Infrastructure.Data
{
    public class ProjectManageContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Data Source=.;Initial Catalog=DemoCodeFirst;Integrated Security=True;Pooling=False");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
        public ProjectManageContext(DbContextOptions<ProjectManageContext> options) : base(options)
        {

        }
    }
}