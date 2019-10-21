CREATE PROC usp_DepositMoney (@AccountId INT, @MoneyAmount DECIMAL(14,4))
AS
BEGIN TRANSACTION
	IF(@MoneyAmount IS NULL OR @MoneyAmount < 0)
	BEGIN
		ROLLBACK
		RAISERROR('The amount need to be positive number', 16,1)
		RETURN
	END

	UPDATE Accounts 
	SET Balance = Balance + @MoneyAmount
	WHERE Id = @AccountId
COMMIT