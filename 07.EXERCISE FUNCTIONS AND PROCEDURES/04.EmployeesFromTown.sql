CREATE PROC usp_GetEmployeesFromTown (@TownName NVARCHAR(20))
AS
SELECT e.FirstName,
	   e.LastName
FROM Employees e
INNER JOIN Addresses ad
	ON e.AddressID = ad.AddressID
INNER JOIN Towns t
	ON ad.TownID = t.TownID
WHERE t.Name = @TownName