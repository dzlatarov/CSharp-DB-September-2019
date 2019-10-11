CREATE TABLE Deleted_Employees(
	EmployeeId INT IDENTITY(1,1),
	FirstName NVARCHAR(30) NOT NULL,
	LastName NVARCHAR(30) NOT NULL,
	MiddleName NVARCHAR(30) NOT NULL,
	JobTitle NVARCHAR(30) NOT NULL,
	DepartmentId INT NOT NULL,
	Salary DECIMAL (18,2)

	CONSTRAINT PK_Deleted_Employees
	PRIMARY KEY(EmployeeId),

	CONSTRAINT FK_Deleted_Employees_Departments
	FOREIGN KEY (DepartmentId)
	REFERENCES Departments (DepartmentID)
)

CREATE TRIGGER tr_StoreDeletedEmployees ON Employees AFTER DELETE
AS
INSERT INTO Deleted_Employees
SELECT d.FirstName,
	   d.LastName,
	   d.MiddleName,
	   d.JobTitle,
	   d.DepartmentID,
	   d.Salary
FROM deleted d