CREATE TABLE NotificationEmails(
	Id INT IDENTITY(1,1),
	Recipient INT NOT NULL,
	[Subject] NVARCHAR(30) NOT NULL,
	Body NVARCHAR(MAX) NOT NULL,

	CONSTRAINT PK_NotificationEmails
	PRIMARY KEY (Id),

	CONSTRAINT FK_NotificationEmails_Accounts
	FOREIGN KEY (Recipient)
	REFERENCES Accounts(Id)
)

GO

CREATE TRIGGER tr_LogEmail
	ON Logs
AFTER INSERT
	AS
BEGIN
		DECLARE @accountID INT = (SELECT AccountId FROM inserted)
		DECLARE @oldSum DECIMAL(18,2) = (SELECT OldSum FROM inserted)
		DECLARE @newSum DECIMAL(18,2) = (SELECT NewSum FROM inserted)
		DECLARE @subject NVARCHAR(50) = 'Balance change for account: ' + CAST(@accountID AS NVARCHAR(20))
		DECLARE @currentDateTime DATETIME = GETDATE()
		DECLARE @body NVARCHAR(MAX) = 'On ' + CAST(@currentDateTime AS NVARCHAR(20)) + ' your balance was changed from ' + CAST(@oldSum AS NVARCHAR(20)) +
		' to ' + CAST(@newSum AS NVARCHAR(20)) + '.'

		INSERT INTO NotificationEmails (Recipient, [Subject], Body)
			VALUES (@accountID, @subject, @body)
END