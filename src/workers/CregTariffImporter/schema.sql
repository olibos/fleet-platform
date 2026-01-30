CREATE SCHEMA creg

GO

CREATE TABLE creg.Tariffs
(
    [Month]     date            NOT NULL,
    Brussels    decimal(5, 2)   NOT NULL,
    Flanders    decimal(5, 2)   NOT NULL,
    Wallonia    decimal(5, 2)   NOT NULL,

    CONSTRAINT PK_Tariffs PRIMARY KEY ([Month])
);

GO

CREATE FUNCTION [creg].[GetTariffs](@date DATE, @region VARCHAR (50))
RETURNS DECIMAL (5, 2)
AS
BEGIN
    SET @date = DATEFROMPARTS(datepart(YEAR, @date), datepart(MONTH, @date), 1);
    DECLARE @tariff AS DECIMAL (4, 2);
    IF (@date >= '2025-01-01')
        BEGIN
            DECLARE @quarter AS INT, @year AS INT, @count AS INT;
            SET @year = datepart(YEAR, @date);
            SET @quarter = datepart(QUARTER, @date) - 2;
            IF (@quarter < 0)
                BEGIN
                    SET @quarter = 3;
                    SET @year = @year - 1;
                END
            DECLARE @end AS DATE = DATEFROMPARTS(@year, (@quarter * 3) + 1, 1);
            DECLARE @start AS DATE = DATEADD(MONTH, -2, @end);
            SELECT @tariff = round(avg(CASE @region WHEN 'Flanders' THEN Flanders WHEN 'Brussels' THEN Brussels WHEN 'Wallonia' THEN Wallonia ELSE -1 END), 2),
                   @count = count(*)
            FROM   creg.Tariffs AS c
            WHERE  c.[Month] BETWEEN @start AND @end
            IF (@count != 3)
                SET @tariff = -1;
        END
    ELSE
        BEGIN
            SELECT @tariff = CASE @region WHEN 'Flanders' THEN Flanders WHEN 'Brussels' THEN Brussels WHEN 'Wallonia' THEN Wallonia ELSE -1 END
            FROM   creg.Tariffs AS c
            WHERE  c.[Month] = @date
        END
    if (isnull(@tariff, -1) <= 0) RETURN CONVERT(int, 'No tarrif available for the requested period!');
    RETURN @tariff;
END