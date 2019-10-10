CREATE FUNCTION ufn_GetSalaryLevel (@salary DECIMAL(18,4))
RETURNS NVARCHAR(20)
AS
BEGIN
	DECLARE @salaryLevel NVARCHAR(20)
	IF(@salary < 30000)
		SET @salaryLevel = 'Low'	
	ELSE IF(@salary BETWEEN 30000 AND 50000)	
		SET @salaryLevel = 'Average'	
	ELSE 
		SET @salaryLevel = 'High'

	RETURN @salaryLevel	 
END