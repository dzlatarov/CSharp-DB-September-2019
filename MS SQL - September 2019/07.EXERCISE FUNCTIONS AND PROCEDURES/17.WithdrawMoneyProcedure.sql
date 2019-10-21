CREATE PROC usp_WithdrawMoney (@AccountId INT, @MoneyAmount DECIMAL(15,4))
AS
BEGIN TRANSACTION
	IF(@MoneyAmount < 0)
	BEGIN
		ROLLBACK
		RAISERROR('The amout should be positive number',16,1)
		RETURN
	END

	DECLARE @accountIdChecker INT= (SELECT a.Id FROM Accounts a WHERE a.Id = @AccountId)
	DECLARE @balance DECIMAL(15,4) = (SELECT a.Balance FROM Accounts a WHERE a.Id = @AccountId)
	IF(@accountIdChecker IS NULL)
	BEGIN
		ROLLBACK
		RAISERROR('The accound id is not correct',16, 1)
		RETURN
	END

	IF(@balance < @MoneyAmount)
	BEGIN
		ROLLBACK
		RAISERROR('Not enough funds',16,1)
		RETURN
	END

	UPDATE Accounts
	SET Balance = Balance - @MoneyAmount
	WHERE Id = @AccountId
COMMIT