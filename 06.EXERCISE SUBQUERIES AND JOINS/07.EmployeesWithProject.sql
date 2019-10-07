SELECT TOP 5
	   e.EmployeeID,
	   e.FirstName,
	   pr.Name AS [ProjectName]
	FROM Employees e
LEFT JOIN  EmployeesProjects ep
	ON e.EmployeeID = ep.EmployeeID
LEFT JOIN Projects pr
	ON ep.ProjectID = pr.ProjectID
WHERE pr.StartDate > '08-13-2002' AND pr.EndDate IS NULL
ORDER BY e.EmployeeID