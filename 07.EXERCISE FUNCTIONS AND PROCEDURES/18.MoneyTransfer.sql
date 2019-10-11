CREATE PROC usp_TransferMoney (@SenderId INT, @ReceiverId INT, @Amount DECIMAL(18,4))
AS
BEGIN TRANSACTION
	DECLARE @sender INT = (SELECT a.Id FROM Accounts a WHERE a.Id = @SenderId)
	DECLARE @receiver INT = (SELECT a.Id FROM Accounts a WHERE a.Id = @ReceiverId)
	
	EXEC usp_DepositMoney @ReceiverId, @Amount
	EXEC usp_WithdrawMoney @SenderId, @Amount
COMMIT