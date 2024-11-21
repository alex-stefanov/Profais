namespace Profais.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Profais.Data.Models;

    /// <summary>
    /// Database Application Context
    /// </summary>
    public class ProfaisDbContext
        : IdentityDbContext<ProfUser>
    {
        public virtual DbSet<Material> Materials { get; set; }

        public virtual DbSet<Message> Messages { get; set; }

        public virtual DbSet<Penalty> Penalties { get; set; }

        public virtual DbSet<ProfProject> Projects { get; set; }

        public virtual DbSet<ProfProjectRequest> ProjectsRequests { get; set; }

        public virtual DbSet<ProfSpecialistRequest> SpecialistRequests { get; set; }

        public virtual DbSet<ProfTask> Tasks { get; set; }

        public override DbSet<ProfUser> Users { get; set; }

        public virtual DbSet<ProfUserPenalty> UsersPenalties { get; set; }

        public virtual DbSet<ProfUserTask> UsersTasks { get; set; }

        public virtual DbSet<ProfWorkerRequest> WorkerRequests { get; set; }

        public virtual DbSet<TaskMaterial> TasksMaterials { get; set; }

        public virtual DbSet<Vehicle> Vehicles { get; set; }

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
            modelBuilder.Entity<Message>()
                .HasKey(x => new { x.ProjectId, x.ClientId });

            modelBuilder.Entity<ProfUserPenalty>()
                .HasKey(x => new { x.UserId, x.PenaltyId });

            modelBuilder.Entity<ProfUserTask>()
                .HasKey(x => new { x.WorkerId, x.TaskId, x.VehicleId });

            modelBuilder.Entity<TaskMaterial>()
                .HasKey(x => new { x.MaterialId, x.TaskId });   

            base.OnModelCreating(modelBuilder);
        }
    }
}