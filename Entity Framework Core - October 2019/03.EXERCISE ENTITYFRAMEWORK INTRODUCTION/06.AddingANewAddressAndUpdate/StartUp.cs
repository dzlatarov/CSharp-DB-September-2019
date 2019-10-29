namespace SoftUni
{
    using SoftUni.Data;
    using System;
    using System.Text;
    using System.Linq;
    using SoftUni.Models;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var context = new SoftUniContext())
            {
                string result = AddNewAddressToEmployee(context);
                Console.WriteLine(result);
            }
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
