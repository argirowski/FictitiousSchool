using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<FictitiousSchoolApplication> SubmitApplications { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseDate> CourseDates { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Participant> Participants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FictitiousSchoolApplication>()
                .HasKey(sa => sa.Id);

            modelBuilder.Entity<FictitiousSchoolApplication>()
                .HasOne(sa => sa.Course)
                .WithMany()
                .HasForeignKey(sa => sa.CourseId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            modelBuilder.Entity<FictitiousSchoolApplication>()
                .HasOne(sa => sa.CourseDate)
                .WithMany()
                .HasForeignKey(sa => sa.CourseDateId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            modelBuilder.Entity<FictitiousSchoolApplication>()
                .HasOne(sa => sa.Company)
                .WithMany()
                .HasForeignKey(sa => sa.CompanyId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete

            modelBuilder.Entity<FictitiousSchoolApplication>()
                .HasMany(sa => sa.Participants)
                .WithOne()
                .HasForeignKey(p => p.SubmitApplicationId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete

            modelBuilder.Entity<Course>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Course>()
                .HasMany(c => c.CourseDates)
                .WithOne(cd => cd.Course)
                .HasForeignKey(cd => cd.CourseId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete

            modelBuilder.Entity<Company>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Participant>()
                .HasKey(p => p.Id);

            // Apply the seed data
            modelBuilder.Seed();
        }
    }
}