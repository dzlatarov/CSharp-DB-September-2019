CREATE PROC usp_EmployeesBySalaryLevel (@salaryLevel NVARCHAR(10))
AS
SELECT e.FirstName,
	   e.LastName
FROM Employees e
WHERE dbo.ufn_GetSalaryLevel (e.Salary) = @salaryLevel