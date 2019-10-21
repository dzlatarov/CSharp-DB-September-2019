SELECT TOP 3
		e.EmployeeID,
		e.FirstName
	FROM Employees e
LEFT JOIN EmployeesProjects ep
	ON e.EmployeeID = ep.EmployeeID
LEFT JOIN Projects pr
	 ON ep.ProjectID = pr.ProjectID
WHERE pr.Name IS NULL
ORDER BY e.EmployeeID