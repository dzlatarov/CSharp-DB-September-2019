namespace SoftUni
{
    using SoftUni.Data;
    using System;
    using System.Text;
    using System.Linq;
    using SoftUni.Models;
    using System.Globalization;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var context = new SoftUniContext())
            {
                string result = GetEmployee147(context);
                Console.WriteLine(result);
            }
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employee = context.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    FullName = e.FirstName + " " + e.LastName,
                    e.JobTitle,
                    Projects = e.EmployeesProjects.Select(p => p.Project.Name).OrderBy(p => p)
                })
                .FirstOrDefault();


            sb.AppendLine($"{employee.FullName} - {employee.JobTitle}");

            foreach (var projectName in employee.Projects)
            {
                sb.AppendLine($"{projectName}");
            }


            return sb.ToString().TrimEnd();
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var addresses = context.Addresses
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.Town.Name)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .Select(a => new
                {
                    AddressText = a.AddressText,
                    TownName = a.Town.Name,
                    EmployeeCount = a.Employees.Count
                });

            foreach (var address in addresses)
            {
                sb.AppendLine($"{address.AddressText}, {address.TownName} - {address.EmployeeCount} employees");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.EmployeesProjects
                .Any(p => p.Project.StartDate.Year >= 2001 && p.Project.StartDate.Year <= 2003))
                .Take(10)
                .Select(e => new
                {
                    FullName = e.FirstName + " " + e.LastName,
                    ManagerName = e.Manager.FirstName + " " + e.Manager.LastName,
                    Projects = e.EmployeesProjects
                    .Select(p => new
                    {
                        p.Project.Name,
                        p.Project.StartDate,
                        p.Project.EndDate
                    })
                });

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FullName} - Manager: {employee.ManagerName}");


                foreach (var project in employee.Projects)
                {
                    string format = "M/d/yyyy h:mm:ss tt";
                    string startDate = project.StartDate.ToString(format, CultureInfo.InvariantCulture);

                    string endDate = project.EndDate != null
                        ? project.EndDate.Value.ToString(format, CultureInfo.InvariantCulture)
                        : "not finished";

                    sb.AppendLine($"--{project.Name} - {startDate} - {endDate}");
                }
            }


            return sb.ToString().TrimEnd();
        }
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var newaddress = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };


            var employeeToFind = context.Employees
               .FirstOrDefault(e => e.LastName == "Nakov");

            employeeToFind.Address = newaddress;
            context.SaveChanges();

            var addresses = context.Employees
                .OrderByDescending(em => em.AddressId)
                .Take(10)
                .Select(e => e.Address.AddressText);

            foreach (var address in addresses)
            {
                sb.AppendLine(address);
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Select(em => new
                {
                    em.FirstName,
                    em.LastName,
                    DepartmentName = em.Department.Name,
                    em.Salary
                })
            .Where(em => em.DepartmentName == "Research and Development")
            .OrderBy(em => em.Salary)
            .ThenByDescending(em => em.FirstName);


            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} from {employee.DepartmentName} - ${employee.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(em => em.Salary > 50000)
                .OrderBy(em => em.FirstName);

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                 .OrderBy(em => em.EmployeeId);

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
