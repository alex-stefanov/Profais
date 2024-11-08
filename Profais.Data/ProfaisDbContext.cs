namespace Profais.Data
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Database Application Context
    /// </summary>
    public class ProfaisDbContext : IdentityDbContext<IdentityUser>
    {
        public ProfaisDbContext() { }
        /// <summary>
        /// Database Application Context Constructor
        /// </summary>
        /// <param name="options">Options for the DbContext</param>
        public ProfaisDbContext(DbContextOptions<ProfaisDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}