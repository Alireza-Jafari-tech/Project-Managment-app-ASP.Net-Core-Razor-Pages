using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using AuthTest.Model;

namespace AuthTest.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Ignore<SelectListItem>();
            builder.Ignore<SelectListGroup>();

            // builder.Entity<MemberTask>()
            //     .HasOne(mt => mt.Member)
            //     .WithMany(u => u.MemberTasks)
            //     .HasForeignKey(mt => mt.MemberId);

            // builder.Entity<MemberTask>()
            //     .HasOne(mt => mt.Task)
            //     .WithMany(t => t.MemberTasks)
            //     .HasForeignKey(mt => mt.TaskId);

            // One-to-many: manager can create many projects
            builder.Entity<Project>()
                .HasOne(p => p.CreatedBy)
                .WithMany(u => u.CreatedProjects)
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-many: manager manages many projects
            builder.Entity<Project>()
                .HasOne(p => p.Manager)
                .WithMany(u => u.ManagedProjects)
                .HasForeignKey(p => p.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);


            // Project <==> Member
            builder.Entity<ProjectMember>()
                .HasKey(pm => new { pm.ProjectId, pm.MemberId });

            // builder.Entity<ProjectMember>()
            //     .HasOne(pm => pm.Project)
            //     .WithMany(p => p.ProjectMembers)
            //     .HasForeignKey(pm => pm.ProjectId)
            //     .OnDelete(DeleteBehavior.Cascade);

            // builder.Entity<ProjectMember>()
            //     .HasOne(pm => pm.Member)
            //     .WithMany(u => u.ProjectMembers)
            //     .HasForeignKey(pm => pm.MemberId)
            //     .OnDelete(DeleteBehavior.Cascade);

            // Task creator
            builder.Entity<TeamMemberTask>()
                .HasOne(t => t.CreatedBy)
                .WithMany(u => u.CreatedTasks)
                .HasForeignKey(t => t.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<TeamMemberTask> Tasks { get; set; }
        public DbSet<ManagerProfile> ManagerProfiles { get; set; }
        public DbSet<MemberProfile> MemberProfiles { get; set; }
        public DbSet<MemberExperienceLevel> MemberExperienceLevels { get; set; }
        public DbSet<InvitationCode> InvitationCodes { get; set; }
        public DbSet<MemberTaskStatus> TasksStatus { get; set; }
        public DbSet<ProjectStatus> ProjectsStatus { get; set; }
        public DbSet<TaskPriority> TasksPriority { get; set; }
    }
}