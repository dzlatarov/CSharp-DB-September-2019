SELECT e.EmployeeID,
	   e.FirstName,
	   CASE 
			WHEN pr.StartDate >= '01-01-2005' THEN NULL
			ELSE pr.Name
		END AS [ProjectName] 
FROM Employees e
INNER JOIN EmployeesProjects ep
	ON e.EmployeeID = ep.EmployeeID
INNER JOIN Projects pr
	ON ep.ProjectID = pr.ProjectID
WHERE e.EmployeeID = 24
