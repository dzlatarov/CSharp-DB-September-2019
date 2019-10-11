CREATE PROC usp_AssignProject (@emloyeeId INT, @projectID INT)
AS
BEGIN TRANSACTION
		INSERT INTO EmployeesProjects (EmployeeID, ProjectID)
		VALUES (@emloyeeId, @projectID)

		DECLARE @projectCount INT = (SELECT COUNT(*)
									FROM EmployeesProjects ep
									WHERE ep.EmployeeID = @emloyeeId)
		IF(@projectCount > 3)
		BEGIN
			ROLLBACK
			RAISERROR('The employee has too many projects!',16,1)
			RETURN
		END	    
COMMIT