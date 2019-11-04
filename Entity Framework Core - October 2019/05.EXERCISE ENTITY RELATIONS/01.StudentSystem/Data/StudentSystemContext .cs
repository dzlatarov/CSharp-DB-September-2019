namespace P01_StudentSystem.Data
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Models;
    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {

        }
        public StudentSystemContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Student> Students { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<Homework> HomeworkSubmissions { get; set; }

        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Config.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureStudentEntity(modelBuilder);

            ConfigureCourseEntity(modelBuilder);

            ConfigureResourceEntity(modelBuilder);

            ConfigureHomeworkEntity(modelBuilder);

            ConfigureStudentCourse(modelBuilder);

            modelBuilder.Seed();
        }

        private void ConfigureStudentCourse(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentCourse>()
               .HasKey(sc => new { sc.StudentId, sc.CourseId });

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.CourseEnrollments)
                .HasForeignKey(sc => sc.StudentId);

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(s => s.StudentsEnrolled)
                .HasForeignKey(sc => sc.CourseId);
        }

        private void ConfigureHomeworkEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Homework>()
               .HasKey(c => c.HomeworkId);

            modelBuilder.Entity<Homework>()
               .Property(c => c.Content)
               .IsRequired()
               .IsUnicode(false);
        }

        private void ConfigureResourceEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Resource>()
                .HasKey(c => c.ResourceId);

            modelBuilder.Entity<Resource>()
               .Property(c => c.Name)
               .HasMaxLength(50)
               .IsRequired()
               .IsUnicode();

            modelBuilder.Entity<Resource>()
               .Property(c => c.Name)
               .IsRequired()
               .IsUnicode(false);
        }

        private void ConfigureCourseEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                .HasKey(c => c.CourseId);

            modelBuilder.Entity<Course>()
                .HasMany(c => c.StudentsEnrolled)
                .WithOne(se => se.Course);

            modelBuilder.Entity<Course>()
                .HasMany(c => c.Resources)
                .WithOne(se => se.Course);

            modelBuilder.Entity<Course>()
                .HasMany(c => c.HomeworkSubmissions)
                .WithOne(se => se.Course);

            modelBuilder.Entity<Course>()
                .Property(c => c.Name)
                .HasMaxLength(80)
                .IsRequired()
                .IsUnicode();

            modelBuilder.Entity<Course>()
                .Property(c => c.Description)
                .IsRequired(false)
                .IsUnicode();
        }

        private void ConfigureStudentEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasKey(s => s.StudentId);

            modelBuilder.Entity<Student>()
                .HasMany(s => s.HomeworkSubmissions)
                .WithOne(h => h.Student);

            modelBuilder.Entity<Student>()
                .HasMany(s => s.CourseEnrollments)
                .WithOne(ce => ce.Student);

            modelBuilder.Entity<Student>()
                .Property(s => s.Name)
                .HasMaxLength(100)
                .IsRequired()
                .IsUnicode();

            modelBuilder.Entity<Student>()
                .Property(s => s.PhoneNumber)
                .HasMaxLength(10);
        }
    }
}
