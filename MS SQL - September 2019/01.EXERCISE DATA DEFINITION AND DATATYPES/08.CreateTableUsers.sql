CREATE TABLE Users
(
	Id INT IDENTITY(1,1),
	Username VARCHAR(30) NOT NULL,
	Password VARCHAR(26) NOT NULL,
	ProfilePicture VARBINARY(MAX),
	LastLoginTime SMALLDATETIME,
	IsDeleted BIT,

	CONSTRAINT PK_Users
	PRIMARY KEY (Id),

	CONSTRAINT UQ_Username
	UNIQUE (Username)
)
INSERT INTO Users (Username, Password, ProfilePicture, LastLoginTime, IsDeleted)
VALUES ('IvanPeshev91', '123456', 1000, '2014-01-19 10:00:01', 1),
	   ('gotiniq94', '1111111111', NULL, '2018-09-05 07:30:22', 0),
	   ('skinnhead', 'helloWorld', 5236, '2010-07-09 12:05:31', 0),
	   ('harabia', 'abcdefg', NULL, '2019-08-06 14:20:25', 1),
	   ('softuni', 'nakov', 3523, '2016-01-19 19:15:55', 1)
