-- акумулятор з розділенням за періодами (місячна статистика)
CREATE TABLE AccRatesPeriods (
	Id UNIQUEIDENTIFIER PRIMARY KEY,
	[Period] INT,   -- 202410
	RealtyId UNIQUEIDENTIFIER,
	AvgRate FLOAT,
	CntRates INT,
	LastRateAt DATETIME
);
