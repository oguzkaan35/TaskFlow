using Microsoft.EntityFrameworkCore;

using TaskFlow.Api.Entities;

namespace TaskFlow.Api.Data;

public class TaskFlowDbContext : DbContext
{
    public TaskFlowDbContext(DbContextOptions<TaskFlowDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Project> Projects { get; set; }

    public DbSet<TaskItem> TaskItems { get; set; }

    public DbSet<TaskState> TaskStatuses { get; set; }

    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TaskItem>()
            .HasOne(x => x.Project)
            .WithMany(x => x.Tasks)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TaskItem>()
            .HasOne(x => x.AssignedUser)
            .WithMany(x => x.AssignedTasks)
            .HasForeignKey(x => x.AssignedUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TaskItem>()
            .HasOne(x => x.Status)
            .WithMany(x => x.Tasks)
            .HasForeignKey(x => x.StatusId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Comment>()
            .HasOne(x => x.TaskItem)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.TaskItemId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Comment>()
            .HasOne(x => x.User)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);



        modelBuilder.Entity<TaskState>().HasData(
    new TaskState
    {
        Id = 1,
        StatusName = "Bekliyor"
    },
    new TaskState
    {
        Id = 2,
        StatusName = "Devam Ediyor"
    },
    new TaskState
    {
        Id = 3,
        StatusName = "Tamamlandı"
    }
);


    }
}
