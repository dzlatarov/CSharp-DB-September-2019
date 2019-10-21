CREATE PROC usp_CalculateFutureValueForAccount (@accountID INT, @interestRate FLOAT)
AS
SELECT @accountID AS [Account Id],
	   ah.FirstName,
	   ah.LastName,
	   a.Balance AS [Current Balance],
	   dbo.ufn_CalculateFutureValue(a.Balance, @interestRate, 5) AS [Balance in 5 years]
FROM AccountHolders ah
INNER JOIN Accounts a
	ON ah.Id = a.AccountHolderId
WHERE a.Id = @accountID