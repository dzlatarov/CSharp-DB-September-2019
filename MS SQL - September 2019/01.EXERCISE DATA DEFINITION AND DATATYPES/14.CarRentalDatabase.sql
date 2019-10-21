CREATE DATABASE CarRental
	USE CarRental

CREATE TABLE Categories(
	Id INT IDENTITY(1,1),
	CategoryName NVARCHAR(50) NOT NULL,
	DailyRate DECIMAL(7,2),
	WeeklyRate DECIMAL(7,2),
	MonthlyRate DECIMAL(7,2),
	WeekendRate DECIMAL(7,2),

	CONSTRAINT PK_Categories
	PRIMARY KEY (Id)
)

CREATE TABLE Cars(
	Id INT IDENTITY(1,1),
	PlateNumber NVARCHAR(10) NOT NULL,
	Manufacturer NVARCHAR(20) NOT NULL,
	Model NVARCHAR(20) NOT NULL,
	CarYear INT NOT NULL,
	CategoryId INT,
	Doors INT NOT NULL,
	Picture VARBINARY(MAX),
	Condition NVARCHAR(MAX),
	Available BIT NOT NULL,

	CONSTRAINT PK_Cars
	PRIMARY KEY (Id),

	CONSTRAINT FK_Cars_Categories
	FOREIGN KEY (CategoryId)
	REFERENCES Categories(Id)
)


CREATE TABLE Employees(
	Id INT IDENTITY(1,1),
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	Title NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX),

	CONSTRAINT PK_Employees
	PRIMARY KEY (Id)
)

CREATE TABLE Customers(
	Id INT IDENTITY(1,1),
	DriverLicenceNumber NVARCHAR(20) NOT NULL,
	FullName NVARCHAR(100) NOT NULL,
	Address NVARCHAR(50) NOT NULL,
	City NVARCHAR(30) NOT NULL,
	ZIPCode INT NOT NULL,
	Notes NVARCHAR(MAX),

	CONSTRAINT PK_Customers
	PRIMARY KEY (Id)
)

CREATE TABLE RentalOrders(
	Id INT IDENTITY(1,1),
	EmployeeId INT,
	CustomerId INT,
	CarId INT,
	TankLevel INT NOT NULL,
	KilometrageStart INT NOT NULL,
	KilometrageEnd INT NOT NULL,
	TotalKilometrage AS KilometrageEnd - KilometrageStart,
	StartDate DATE NOT NULL,
	EndDate DATE NOT NULL,
	TotalDays AS DATEDIFF(DAY, StartDate, EndDate),
	RateApplied DECIMAL(7,2),
	TaxRate DECIMAL(7,2),
	OrderStatus NVARCHAR(50),
	Notes NVARCHAR(MAX),

	CONSTRAINT PK_RentalOrders
	PRIMARY KEY (Id),

	CONSTRAINT FK_RentalOrders_Employees
	FOREIGN KEY (EmployeeId)
	REFERENCES Employees(Id),

	CONSTRAINT FK_RentalOrders_Customers
	FOREIGN KEY (CustomerId)
	REFERENCES Customers(Id),
	
	CONSTRAINT FK_RentalOrders_Cars
	FOREIGN KEY (CarId)
	REFERENCES Cars(Id)
) 

INSERT INTO Categories (CategoryName, DailyRate, WeeklyRate, MonthlyRate, WeekendRate)
	VALUES ('Car', 15, 50, 120, 30.50),
		   ('Truck', 5, 20, 50, 15.20),
		   ('Bus', 2, 14, 23, 5.50)


INSERT INTO Cars (PlateNumber, Manufacturer, Model, CarYear, CategoryId, Doors, Picture, Condition, Available)
	VALUES ('dfsdf', 'BMW', '320D', 2007, 1, 2, 5555, 'Used', 1),
		   ('csafgfsg', 'Audi', 'Q5', 2018, 2, 5, 12345, 'New', 1),
		   ('rhtrghr', 'Volvo', 'MAN', 1999, 3, 3, 13247, 'Used', 1) 

INSERT INTO Employees (FirstName, LastName, Title, Notes)
	VALUES ('Ivan', 'Ivanov', 'Seller', 'Very good seller'),
		   ('Pesho', 'Peshev', 'Seller', 'Not that good seler'),
		   ('Minka', 'Tragicheva', 'Manager', 'Top manager')

INSERT INTO Customers (DriverLicenceNumber, FullName, Address, City, ZIPCode, Notes)
	VALUES ('sdfsdfsd', 'Ivan Gospodinov', 'kukurigo namerigo 1', 'Polski Trumbesh', 1234, 'Nice Customer'),
		   ('fdsgvdfgdf', 'Maria Petrova', 'Svoboda 12', 'Plovdiv', 1596, 'Angryy woman'),
		   ('egrhrgth', 'Todor Peichev', 'Pohod 14', 'Varna', 1456, 'Master') 
		   
INSERT INTO RentalOrders (EmployeeId, CustomerId, CarId, TankLevel, KilometrageStart, KilometrageEnd, StartDate, EndDate, Notes)
	VALUES (1, 2, 1, 70, 100500, 101200, '2018-01-19', '2018-01-25', 'Very nice customer'),
		   (2, 3, 2, 60, 150000, 155000, '2016-05-01', '2016-05-28', 'Agressive customer'),
		   (3, 1, 3, 100, 222222, 224555, '2019-09-15', '2019-09-30', 'Calm and happy customer')
		   
