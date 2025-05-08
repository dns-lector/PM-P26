SELECT  R.Id, MAX(R.Name), COUNT(*), AVG( CAST( F.Rate AS FLOAT ) )
FROM Realties R JOIN Feedbacks F ON R.Id = F.RealtyId
GROUP BY R.Id;

-- ініціалізація акумулятора
INSERT INTO AccRates SELECT NEWID(), R.Id, 
	AVG( CAST( F.Rate AS FLOAT ) ), COUNT( * ), 
	CURRENT_TIMESTAMP FROM Realties R JOIN Feedbacks F ON R.Id = F.RealtyId
	GROUP BY R.Id;

SELECT * FROM AccRates;

DELETE FROM AccRates;

DROP TRIGGER FeedbacksTrigger;
-- 3,0952380952381	21
-- 3,14285714285714	21

-- використання акумулятора
SELECT  R.Id, R.Name, COALESCE(A.CntRates,0),  COALESCE(A.AvgRate,0)
FROM Realties R LEFT JOIN AccRates A ON R.Id = A.RealtyId