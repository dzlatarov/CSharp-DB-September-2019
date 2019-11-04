namespace P01_StudentSystem
{
    using Microsoft.EntityFrameworkCore;
    using P01_StudentSystem.Data.Models;
    using P01_StudentSystem.Data.Models.Enums;
    using System;
    public static class FillingDatabaseWithSomeDataMethod
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData
             (
                new Student
                {
                    StudentId = 1,
                    Name = "Georgi",
                    RegisteredOn = DateTime.Now
                },

               new Student
               {
                   StudentId = 2,
                   Name = "Maria",
                   RegisteredOn = DateTime.Now
               }

             );

            modelBuilder.Entity<Course>().HasData
           (
                new Course
                {
                    CourseId = 1,
                    Name = "Math",
                    Price = 200m
                },

                new Course
                {
                    CourseId = 2,
                    Name = "Entity Framework",
                    Price = 480m
                }
           );

            modelBuilder.Entity<Resource>().HasData
            (
                new Resource
                {
                    ResourceId = 1,
                    Name = "Course resources",
                    ResourceType = ResourceType.Presentation,
                    CourseId = 2
                },

                new Resource
                {
                    ResourceId = 2,
                    Name = "Math resources",
                    ResourceType = ResourceType.Document,
                    CourseId = 1
                }
            );

            modelBuilder.Entity<Homework>().HasData
            (
                new Homework
                {
                    HomeworkId = 1,
                    Content = "http://www.facebook.com/",
                    ContentType = ContentType.Zip,
                    SubmissionTime = DateTime.Now,
                    StudentId = 1,
                    CourseId = 1
                },

                new Homework
                {
                    HomeworkId = 2,
                    Content = "http://www.google.com/",
                    ContentType = ContentType.Pdf,
                    SubmissionTime = DateTime.Now,
                    StudentId = 2,
                    CourseId = 2
                }
            );
        }
    }
}
