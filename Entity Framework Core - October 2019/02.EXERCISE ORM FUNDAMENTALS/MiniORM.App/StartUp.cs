namespace MiniORM.App
{
    using System;
    using Data;
    using Data.Entities;
    using System.Linq;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            var connectionString = "Server=.\\SQLEXPRESS01;Database=MiniORM;Integrated Security=True";

            var context = new SoftUniDbContextClass(connectionString);

            context.Employees.Add(new Employee
            {
                FirstName = "Gosho",
                LastName = "Inserted",
                DepartmentId = context.Departments.First().Id,
                isEmployed = true,
            });

            var employee = context.Employees.Last();
            employee.FirstName = "Modified";

            context.SaveChanges();
        }
    }
}
