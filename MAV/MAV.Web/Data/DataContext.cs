namespace MAV.Web.Data
{
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;

    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<Intern> Interns { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<LoanDetail> LoanDetails { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Status> Statuses { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        //Habilitar mi comportamiento de eliminado en cascada. 
        protected override void OnModelCreating(ModelBuilder builder)
        {
            var cascadeFKs = builder.Model
                .GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);
            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }
            base.OnModelCreating(builder);
        }
    }
}
