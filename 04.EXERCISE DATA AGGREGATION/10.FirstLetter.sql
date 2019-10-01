SELECT LEFT(w.FirstName,1) AS [FirstLetter]
	       
FROM WizzardDeposits w
WHERE w.DepositGroup = 'Troll Chest'
GROUP BY LEFT(w.FirstName,1)