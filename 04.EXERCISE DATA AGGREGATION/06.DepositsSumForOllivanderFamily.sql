SELECT w.DepositGroup,
	   SUM(w.DepositAmount) AS TotalSum
FROM WizzardDeposits w
WHERE w.MagicWandCreator = 'Ollivander family'
GROUP BY w.DepositGroup