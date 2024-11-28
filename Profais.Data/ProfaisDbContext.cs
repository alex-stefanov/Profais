using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Profais.Data.Models;

namespace Profais.Data;

/// <summary>
/// Database Application Context
/// </summary>
public class ProfaisDbContext
    : IdentityDbContext<ProfUser, IdentityRole<string>, string>
{
    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<Penalty> Penalties { get; set; }

    public virtual DbSet<ProfProject> Projects { get; set; }

    public virtual DbSet<ProfUserRequest> UserRequests { get; set; }

    public virtual DbSet<ProfProjectRequest> ProjectsRequests { get; set; }

    public virtual DbSet<ProfSpecialistRequest> SpecialistRequests { get; set; }

    public virtual DbSet<ProfTask> Tasks { get; set; }

    public override DbSet<ProfUser> Users { get; set; }

    public virtual DbSet<ProfUserPenalty> UsersPenalties { get; set; }

    public virtual DbSet<ProfUserTask> UsersTasks { get; set; }

    public virtual DbSet<ProfWorkerRequest> WorkerRequests { get; set; }

    public virtual DbSet<TaskMaterial> TasksMaterials { get; set; }

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
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}