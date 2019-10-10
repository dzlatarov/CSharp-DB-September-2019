CREATE FUNCTION ufn_CalculateFutureValue (@sum DECIMAL(18,4), @yearlyInterestRate FLOAT, @numberOfYears INT)
RETURNS DECIMAL (18,4)
AS
BEGIN
	DECLARE @futureValue DECIMAL(18,4)
		SET @futureValue = @sum * (POWER(1 + @yearlyInterestRate, @numberOfYears))

	RETURN @futureValue
END