-- налаштування тригера-акумулятора
CREATE TRIGGER FeedbacksTrigger
ON Feedbacks
AFTER INSERT, DELETE, UPDATE
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @r INT;
	DECLARE @d INT;
	DECLARE @p INT;
	DECLARE @m INT;
	DECLARE @mom DATETIME;
	DECLARE @per INT;
	DECLARE @id UNIQUEIDENTIFIER;
	SET @r = ( SELECT COALESCE( SUM(Rate), 0 ) FROM inserted ) ;
	SET @d = ( SELECT COALESCE( SUM(Rate), 0 ) FROM deleted  ) ;
	SET @p = ( SELECT COUNT(Rate) FROM inserted ) ;
	SET @m = ( SELECT COUNT(Rate) FROM deleted  ) ;
	SET @id = COALESCE( (SELECT RealtyId FROM inserted), (SELECT RealtyId FROM deleted) );
	SET @mom = COALESCE( (SELECT Moment FROM inserted), (SELECT Moment FROM deleted) );
	SET @per = YEAR(@mom) * 100 + MONTH(@mom);

	IF( EXISTS( SELECT Id FROM AccRatesPeriods WHERE RealtyId = @id AND [Period] = @per) )

		UPDATE AccRatesPeriods 
		SET 
			AvgRate = (AvgRate * CntRates + @r - @d) / (CntRates + @p - @m),
			CntRates = CntRates + @p - @m,
			LastRateAt = CURRENT_TIMESTAMP
		WHERE
			RealtyId = @id AND [Period] = @per;

	ELSE 

		INSERT INTO AccRatesPeriods VALUES
			( NEWID(), @per, @id, @r, @p, CURRENT_TIMESTAMP ) ;
END;
