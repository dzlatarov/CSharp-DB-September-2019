CREATE DATABASE Movies
	USE Movies	

CREATE TABLE Directors(
	Id INT IDENTITY(1,1),
	DirectorName NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX),

	CONSTRAINT PK_Directors
	PRIMARY KEY (Id)
)

CREATE TABLE Genres(
	Id INT IDENTITY(1,1),
	GenreName NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX),

	CONSTRAINT PK_Genres
	PRIMARY KEY (Id)
)

CREATE TABLE Categories(
	Id INT IDENTITY(1,1),
	CategoryName NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX),

	CONSTRAINT PK_Categories
	PRIMARY KEY (Id)
)

CREATE TABLE Movies(
	Id INT IDENTITY(1,1),
	Title NVARCHAR(50) NOT NULL,
	DirectorId INT,
	CopyrightYear INT NOT NULL,
	Length INT NOT NULL,
	GenreId INT,
	CategoryId INT,
	Rating DECIMAL(3,2),
	Notes NVARCHAR(MAX),

	CONSTRAINT PK_Movies
	PRIMARY KEY (Id),

	CONSTRAINT FK_Movies_Directors
	FOREIGN KEY (DirectorId)
	REFERENCES Directors(Id),

	CONSTRAINT FK_Movies_Genres
	FOREIGN KEY (GenreId)
	REFERENCES Genres(Id),

	CONSTRAINT FK_Movies_Categories
	FOREIGN KEY (CategoryId)
	REFERENCES Categories(Id)
)

INSERT INTO Directors (DirectorName, Notes)
	VALUES ('Steven Spielberg', 'One of the most influential personalities in the history of cinema'),
	       ('Martin Scorsese', 'He was raised in the neighborhood of Little Italy'),
		   ('Alfred Hitchcock', 'Alfred Joseph Hitchcock was born in Leytonstone'),
		   ('Stanley Kubrick', 'Stanley Kubrick was born in Manhattan'),
		   ('Quentin Tarantino', 'Quentin Jerome Tarantino was born in Knoxville')

INSERT INTO Genres (GenreName, Notes)
	VALUES ('Action', 'An action story is similar to adventure'),
		   ('Drama', 'drama is a genre of narrative fiction'),
		   ('Adventure', 'An adventure story is about a protagonist who journeys to epic or distant places to accomplish something'),
		   ('Comedy', 'Comedy is a story that tells about a series of funny, or comical events'),
		   ('Crime', 'A crime story is about a crime that is being committed or was committed')

INSERT INTO Categories (CategoryName, Notes)
	VALUES ('The Best', 'The best'),
		   ('Good', 'Not too bad'),
		   ('Good to watch', 'Not too bad, not to great'),
		   ('So so', 'you can definately skip it'),
		   ('Worst', 'dont start it')

INSERT INTO Movies (Title, DirectorId, CopyrightYear, Length, GenreId, CategoryId, Rating, Notes)
	VALUES ('Schindler`s List', 1, 1993, 195, 2, 1, 8.9, 'very nice film'),
		   ('Taxi Driver', 2, 1976, 120, 5, 2, 8.3, 'WOW'),
		   ('The Wrong Man', 3, 1956, 110, 2, 2, 7.4, 'Amazing film'),
		   ('2001: A Space Odyssey', 4, 1968, 149, 3, 3, 8.3, 'good job'),
		   ('Reservoir Dogs', 5, 1992, 99, 5, 4, 8.3, 'well done')