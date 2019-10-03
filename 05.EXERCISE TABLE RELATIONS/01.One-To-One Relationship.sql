CREATE TABLE Passports(
	PassportID INT IDENTITY(101,1),
	PassportNumber NVARCHAR(20) NOT NULL

	CONSTRAINT PK_Passports
	PRIMARY KEY (PassportID)
)

CREATE TABLE Persons(
	PersonID INT IDENTITY(1,1),
	FirstName NVARCHAR(50) NOT NULL,
	Salary DECIMAL(9,2) NOT NULL,
	PassportID INT NOT NULL,

	CONSTRAINT PK_Persons
	PRIMARY KEY (PersonID),

	CONSTRAINT FK_Persons_Passports
	FOREIGN KEY  (PassportID)
	REFERENCES Passports (PassportID)
)

INSERT INTO Passports (PassportNumber) 
	VALUES ('N34FG21B'),
		   ('K65LO4R7'),
		   ('ZE657QP2')

INSERT INTO Persons (FirstName, Salary, PassportID)
	VALUES ('Roberto', 43300.00, 102),
		   ('Tom', 56100.00, 103),
		   ('Yana', 60200.00, 101)