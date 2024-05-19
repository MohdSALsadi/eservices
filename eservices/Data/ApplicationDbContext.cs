using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pattern_of_life.Models.Entity;
using Pattern_of_life.Service;
using Pattern_of_life.Services;

namespace Pattern_of_life.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IContextAccessor contextAccessor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IContextAccessor contextAccessor)
            : base(options)
        {
            this.contextAccessor = contextAccessor;

        }
        //public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        //public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        //public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        //public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        //public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        //public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        //public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
        public DbSet <ShipActivity> ShipActivity { get; set; } 
        public DbSet <VesselType> VesselType { get; set; }
        public DbSet <FlagState>  FlagStates { get; set; }  
        public  DbSet <ActivityName> ActivityName { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<ShipMovement> ShipMovement { get; set; }
        public DbSet<CoreNotification> CoreNotification { get; set; }
        public DbSet<Location> Location { get; set; }

        

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<AuditableEntity> entry in ChangeTracker.Entries<AuditableEntity>().ToList())
            {
                string userId = contextAccessor.UserId();
                string ipAddress = contextAccessor.IPAddress();
                entry.Entity.TenantId = 1;
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedTime = DateTime.Now;
                        entry.Entity.CreatedBy = userId;
                        entry.Entity.CreatorIp = ipAddress;
                        break;

                    case EntityState.Modified:
                        entry.Entity.ModificationTime = DateTime.Now;
                        entry.Entity.ModifiedBy = userId;
                        entry.Entity.ModifierIp = ipAddress;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }


    }

}