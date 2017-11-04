using Microsoft.EntityFrameworkCore;
using System;
using TaskTracker.Domain;

namespace TaskTracker.DataAccess.Interfaces
{
    public class TaskTrackerContext : DbContext
    {
        public TaskTrackerContext(DbContextOptions<TaskTrackerContext> options)
            : base(options)
        {
        }

        public TaskTrackerContext() 
            : base()
        {
        }

        public DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>(builder =>
            {
                builder.Property<int>("Id").ValueGeneratedOnAdd();
                builder.Property<DateTime>("Added");
                builder.Property<string>("Description");
                builder.Property<DateTime>("Edited").IsConcurrencyToken();
                builder.Property<string>("Name");
                builder.Property<int>("Priority");
                builder.Property<TaskStatus>("Status");
                builder.Property<TimeSpan>("Duration");
                builder.HasKey("Id");
                builder.ToTable("Task");
            });
        }
    }

}
