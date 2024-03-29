SELECT w.DepositGroup,
	   w.IsDepositExpired,
	   AVG(w.DepositInterest) AS [AverageInterest]
FROM WizzardDeposits w
WHERE w.DepositStartDate > '01-01-1985'
GROUP BY w.DepositGroup,
		 w.IsDepositExpired
ORDER BY w.DepositGroup DESC,
		 w.IsDepositExpired