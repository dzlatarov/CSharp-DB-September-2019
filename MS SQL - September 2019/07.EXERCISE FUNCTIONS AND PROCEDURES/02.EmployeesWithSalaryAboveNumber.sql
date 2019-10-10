CREATE PROC usp_GetEmployeesSalaryAboveNumber (@inputNumber DECIMAL(18,4)) 
AS
SELECT e.FirstName,
	   e.LastName
FROM Employees e
WHERE e.Salary >= @inputNumber