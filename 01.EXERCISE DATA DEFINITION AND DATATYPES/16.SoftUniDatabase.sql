CREATE DATABASE SoftUni
	USE SoftUni

CREATE TABLE Towns(
	Id INT IDENTITY(1,1),
	[Name] NVARCHAR(40) NOT NULL,

	CONSTRAINT PK_Towns
	PRIMARY KEY (Id)
)

CREATE TABLE Addresses(
	Id INT IDENTITY(1,1),
	AddressText NVARCHAR(MAX),
	TownId INT,

	CONSTRAINT PK_Addresses
	PRIMARY KEY (Id),

	CONSTRAINT FK_Addresses_Towns
	FOREIGN KEY (TownId)
	REFERENCES Towns(Id)
)

CREATE TABLE Departments(
	Id INT IDENTITY(1,1),
	[Name] NVARCHAR(50),

	CONSTRAINT PK_Departments
	PRIMARY KEY (Id)
)

CREATE TABLE Employees(
	Id INT IDENTITY(1,1),
	FirstName NVARCHAR(50) NOT NULL,
	MiddleName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	JobTitle NVARCHAR(40) NOT NULL,
	DepartmentId INT,
	HireDate DATE,
	Salary DECIMAL(7,2) NOT NULL,
	AddressId INT,

	CONSTRAINT PK_Employees
	PRIMARY KEY (Id),

	CONSTRAINT FK_Employees_Departments
	FOREIGN KEY (DepartmentId)
	REFERENCES Departments(Id),

	CONSTRAINT FK_Employees_Addresses
	FOREIGN KEY (AddressId)
	REFERENCES Addresses(Id)
)

INSERT INTO Towns (Name)
	VALUES ('Sofia'),
		   ('Plovdiv'),
		   ('Varna')

INSERT INTO Addresses (AddressText, TownId)
	VALUES ('Kanarche 43', 2),
		   ('Hristo Botev', 3),
		   ('Ivan Vazov', 1)

INSERT INTO Departments (Name)
	VALUES ('CEO'),
		   ('Technical'),
		   ('Support')

INSERT INTO Employees (FirstName, MiddleName, LastName, JobTitle, DepartmentId, HireDate, Salary, AddressId)
	VALUES ('Ivan', 'Ivanov', 'Ivanov', 'Cleaner', 3, '2019-05-10', 1500, 1),
		   ('Gergana', 'Petrova', 'Ivanova', 'Trainer ', 2, '2018-10-01', 1000, 2),
		   ('Petar', 'Petrov', 'Shopov', 'Manager', 1, '2016-05-01', 2500, 3) 