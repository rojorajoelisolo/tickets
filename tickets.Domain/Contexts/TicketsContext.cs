using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskEntity = tickets.Domain.Entities.Task;
using UserEntity = tickets.Domain.Entities.User;
using TaskStatusEntity = tickets.Domain.Entities.TaskStatus;
using TaskUserEntity = tickets.Domain.Entities.TaskUser;

namespace tickets.Domain.Contexts
{
    public class TicketsContext : IdentityDbContext<UserEntity>
    {
        public TicketsContext(DbContextOptions<TicketsContext> options) : base(options) { }

        public DbSet<TaskEntity> Tasks { get; set; }

        public DbSet<TaskUserEntity> TaskUsers { get; set; }

        public DbSet<TaskStatusEntity> TaskStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskEntity>()
                .HasOne(t => t.CreatedBy)
                .WithMany()
                .HasForeignKey("CreatedById")
                //.HasForeignKey(t => t.CreatedById)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskStatusEntity>()
                .HasOne(ts => ts.Task)
                .WithMany()
                .HasForeignKey("TaskId")
                //.HasForeignKey(ts => ts.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskStatusEntity>()
                .HasOne(ts => ts.UpdatedBy)
                .WithMany()
                .HasForeignKey("UpdatedById")
                //.HasForeignKey(ts => ts.UpdatedById)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
