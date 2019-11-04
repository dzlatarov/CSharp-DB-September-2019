namespace P01_StudentSystem
{
    using P01_StudentSystem.Data;
    using System;
    using System.Linq;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var dbContext = new StudentSystemContext())
            {
                var students = dbContext.Students                                
                .Select(s => new
                {
                    Name = s.Name,
                    courseNames = s.CourseEnrollments               
                });


                foreach (var student in students)
                {
                    foreach (var course in student.courseNames)
                    {
                        Console.WriteLine($"{student.Name} - {course.Course.Name}");
                    }                   
                }
            }            
        }
    }
}
