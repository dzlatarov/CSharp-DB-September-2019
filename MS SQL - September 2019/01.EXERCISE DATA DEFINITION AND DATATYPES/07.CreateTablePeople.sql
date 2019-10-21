CREATE TABLE People
(
	Id INT IDENTITY(1,1),
	Name NVARCHAR(200) NOT NULL,
	Picture VARBINARY(MAX),
	Height DECIMAL(3,2),
	Weight DECIMAL (5,2),
	Gender CHAR(1) NOT NULL,
	Birthdate DATE NOT NULL,
	Biography NVARCHAR(MAX),
	
	CONSTRAINT PK_People
	PRIMARY KEY (Id)  
)

INSERT INTO People (Name, Picture, Height, Weight, Gender, Birthdate, Biography)
	VALUES('Ivan', 1000, 1.73324, 100.25, 'm', '1994-01-15', 'hello Ivan'),
		  ('Pesho', NULL, 1.63, 86.30, 'm', '1988-09-25', 'hello Pesho'),
		  ('Georgi', 12345, 1.8033, 68.80, 'm', '1985-12-01', 'hello Georgi'),
		  ('Petar', 5555, 1.19555, 58.40, 'm', '1990-10-29', 'Hello Petar'),
		  ('Gergana', NULL, 1.99999, 66.22, 'f', '1998-02-28', 'Hello Gergana')
		  
