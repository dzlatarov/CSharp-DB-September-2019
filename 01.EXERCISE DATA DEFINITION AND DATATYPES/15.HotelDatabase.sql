CREATE DATABASE Hotel
	USE Hotel

CREATE TABLE Employees(
	Id INT IDENTITY(1,1),
	FirstName NVARCHAR(30) NOT NULL,
	LastName NVARCHAR(30) NOT NULL,
	Title NVARCHAR(30) NOT NULL,
	Notes NVARCHAR(MAX),

	CONSTRAINT PK_Employees
	PRIMARY KEY (Id)
)

CREATE TABLE Customers(	
	AccountNumber INT IDENTITY(1,1),
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	PhoneNumber NVARCHAR(20) NOT NULL,
	EmergencyName NVARCHAR(20) NOT NULL,
	EmergencyNumber NVARCHAR(20) NOT NULL,
	Notes NVARCHAR(MAX),

	CONSTRAINT PK_Customers
	PRIMARY KEY (AccountNumber)
)

CREATE TABLE RoomStatus(
	RoomStatus NVARCHAR(20) NOT NULL,
	Notes NVARCHAR(MAX),

	CONSTRAINT PK_RoomStatus
	PRIMARY KEY (RoomStatus)
)

CREATE TABLE RoomTypes(
	RoomType NVARCHAR(20) NOT NULL,
	Notes NVARCHAR(MAX),

	CONSTRAINT PK_RoomTypes
	PRIMARY KEY (RoomType)
) 

CREATE TABLE BedTypes(
	BedType NVARCHAR(20) NOT NULL,
	Notes NVARCHAR(MAX),

	CONSTRAINT PK_BedTypes
	PRIMARY KEY (BedType)
)

CREATE TABLE Rooms(
	RoomNumber INT IDENTITY(1,1),
	RoomType NVARCHAR(20),
	BedType NVARCHAR(20),
	Rate DECIMAL (7,2),
	RoomStatus NVARCHAR(20),
	Notes NVARCHAR(MAX),

	CONSTRAINT PK_Rooms
	PRIMARY KEY (RoomNumber),

	CONSTRAINT FK_Rooms_RoomTypes
	FOREIGN KEY (RoomType)
	REFERENCES RoomTypes(RoomType),

	CONSTRAINT FK_Rooms_BedTypes
	FOREIGN KEY (BedType)
	REFERENCES BedTypes(BedType),

	CONSTRAINT FK_Rooms_RoomStatus
	FOREIGN KEY (RoomStatus)
	REFERENCES RoomStatus(RoomStatus)
)

CREATE TABLE Payments(
	Id INT IDENTITY(1,1),
	EmployeeId INT,
	PaymentDate DATE NOT NULL,
	AccountNumber INT,
	FirstDateOccupied DATE NOT NULL,
	LastDateOccupied DATE NOT NULL,
	TotalDays AS DATEDIFF(DAY,FirstDateOccupied,LastDateOccupied),
	AmountCharged DECIMAL (7,2) NOT NULL,
	TaxRate DECIMAL (7,2),
	TaxAmount DECIMAL(7,2),
	PaymentTotal DECIMAL(7,2),
	Notes NVARCHAR(MAX),

	CONSTRAINT PK_Payments
	PRIMARY KEY (Id),

	CONSTRAINT FK_Payments_Employees
	FOREIGN KEY (EmployeeId)
	REFERENCES Employees(Id),

	CONSTRAINT FK_Payments_Customers
	FOREIGN KEY (AccountNumber)
	REFERENCES Customers(AccountNumber)	
)

CREATE TABLE Occupancies(
	Id INT IDENTITY(1,1),
	EmployeeId INT,
	DateOccupied DATE,
	AccountNumber INT,
	RoomNumber INT,
	RateApplied DECIMAL(7,2) NOT NULL,
	PhoneCharge DECIMAL(7,2),
	Notes NVARCHAR(MAX),

	CONSTRAINT PK_Occupancies
	PRIMARY KEY (Id),

	CONSTRAINT FK_Occupancies_Employees
	FOREIGN KEY (EmployeeId)
	REFERENCES Employees(Id),

	CONSTRAINT FK_Occupancies_Customers
	FOREIGN KEY (AccountNumber)
	REFERENCES Customers(AccountNumber),

	CONSTRAINT FK_Occupancies_Rooms
	FOREIGN KEY (RoomNumber)
	REFERENCES Rooms(RoomNumber)
)

INSERT INTO Employees (FirstName, LastName, Title, Notes)
	VALUES ('Ivan', 'Ivanov', 'Cleaner', 'Good cleaner'),
		('Pesho', 'Peshev', 'Manager', 'Good guy'),
		('Maria', 'Ivanova', 'Life saver', 'The best saver')
		
INSERT INTO Customers (FirstName, LastName, PhoneNumber, EmergencyName, EmergencyNumber)
	VALUES ('Ginka', 'Shikerova', '+359123456', 'Police', '123'),
		   ('Ivan', 'Todorov', '+3596231', 'Firefighter', '123'),
		   ('Georgi', 'Stambolov', '+3596813', 'Hospital', '123') 

INSERT INTO RoomStatus (RoomStatus, Notes)
	VALUES ('Clean', 'very good'),
		   ('Dirty', 'very bad'),
		   ('Ready', 'Finally')

INSERT INTO RoomTypes (RoomType, Notes)
	VALUES ('Small', 'nice room'),
		   ('Big', 'amazing room'),
		   ('Medium', 'perfect')

INSERT INTO BedTypes (BedType)
	VALUES ('Single'),
		   ('Queen'),
		   ('King') 

INSERT INTO Rooms (RoomType, BedType, Rate, RoomStatus)
	VALUES ('Small', 'Single', 60, 'Dirty'),
		   ('Big', 'Queen', 80, 'Clean'),
		   ('Medium', 'King', 95, 'Ready')

INSERT INTO Payments (EmployeeId, PaymentDate, AccountNumber, FirstDateOccupied, LastDateOccupied, AmountCharged)
	VALUES (1, '2019-05-21', 1, '2019-05-21', '2019-05-28', 700),
		   (2, '2018-02-05', 2, '2018-02-06', '2018-02-08', 100),
		   (3, '2016-11-01', 3, '2016-11-01', '2016-11-11', 1000)

INSERT INTO Occupancies (EmployeeId, AccountNumber, RoomNumber, RateApplied)
	VALUES (1, 1, 1, 20.20),
		   (2, 2, 2, 55.55),
		   (3, 3, 3, 88.25)			      