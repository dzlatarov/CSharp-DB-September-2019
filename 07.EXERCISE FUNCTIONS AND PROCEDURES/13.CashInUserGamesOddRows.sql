CREATE FUNCTION ufn_CashInUsersGames (@gameName NVARCHAR(50))
RETURNS TABLE
RETURN (SELECT SUM(k.Cash) AS SumCash
	FROM
		(SELECT ug.Cash ,
			ROW_NUMBER() OVER (ORDER BY ug.Cash DESC) AS RowNumber
	FROM UsersGames ug
		INNER JOIN Games g
			ON ug.GameId = g.Id
	WHERE g.Name = @gameName) AS k
	WHERE k.RowNumber % 2 = 1)