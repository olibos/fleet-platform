CREATE SCHEMA wallbox

GO

create table wallbox.[Groups]
(
    [Id]        int             NOT NULL,
    [UniqueId]  varchar(100)    NOT NULL Primary Key,
    [Name]      varchar(255)    NOT NULL
);

CREATE TABLE wallbox.Sessions
(
    -- Primary Key
    Id NVARCHAR(50) PRIMARY KEY,
    
    -- Time fields
    StartTime DATETIMEOFFSET NOT NULL,
    EndTime DATETIMEOFFSET NOT NULL,
    ChargingTimeSeconds INT NOT NULL,
    
    -- User information
    UserId INT NOT NULL,
    UserUid NVARCHAR(50) NOT NULL,
    UserName NVARCHAR(100) NOT NULL,
    UserSurname NVARCHAR(100) NULL,
    UserEmail NVARCHAR(255) NOT NULL,
    
    -- Charger information
    ChargerId INT NOT NULL,
    ChargerName NVARCHAR(100) NOT NULL,
    ChargerUid NVARCHAR(50) NULL,
    
    -- Location information
    GroupId INT NOT NULL,
    LocationId INT NOT NULL,
    LocationName NVARCHAR(100) NOT NULL,
    LocationUid NVARCHAR(50) NULL,
    
    -- Energy metrics (stored in Wh)
    Energy INT NOT NULL,
    MidEnergy INT NOT NULL,
    EnergyPrice DECIMAL(10, 4) NOT NULL,
    
    -- Financial information
    CurrencyCode NVARCHAR(3) NOT NULL,
    SessionType NVARCHAR(50) NOT NULL,
    ApplicationFeePercentage DECIMAL(5, 2) NOT NULL,
    TotalCost DECIMAL(10, 2) NOT NULL,
    
    -- Optional order/pricing fields
    OrderUid NVARCHAR(50) NULL,
    RatePrice DECIMAL(10, 4) NULL,
    RateVariableType NVARCHAR(50) NULL,
    OrderEnergy INT NULL,
    AccessPrice DECIMAL(10, 2) NULL,
    FeeAmount DECIMAL(10, 2) NULL,
    Total DECIMAL(10, 2) NULL,
    Subtotal DECIMAL(10, 2) NULL,
    TaxAmount DECIMAL(10, 2) NULL,
    TaxPercentage DECIMAL(5, 2) NULL,
    PublicChargeUid VARCHAR(50) NULL,
    
    -- Organization
    OrganizationUid VARCHAR(50) NULL,
);
