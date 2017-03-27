using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace AreaAnalyserVer3.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
             // : base("DefaultConnection", throwIfV1Schema: false)
             : base("AzureConnection", throwIfV1Schema: false)
        {
        
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        

        public DbSet<PriceRegister> PriceRegister { get; set; }
        public DbSet<GardaStation> GardaStation { get; set; }
        public DbSet<AnnualReport> AnnualReport { get; set; }
        public DbSet<Offence> Offence { get; set; }
        public DbSet<Town> Town { get; set; }
        public DbSet<School> School { get; set; }
        public DbSet<Business> Business { get; set; }
        public DbSet<Leaver> Leaving { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

        }
    }

   
}