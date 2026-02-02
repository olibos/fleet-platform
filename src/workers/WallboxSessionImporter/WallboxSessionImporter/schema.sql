CREATE SCHEMA wallbox

CREATE TABLE wallbox.Charger
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ConnectionStatus NVARCHAR(50) NOT NULL,
    ConnectorType NVARCHAR(100) NULL,
    Image NVARCHAR(500) NULL,
    LastConnection DATETIMEOFFSET NOT NULL,
    LocationId NVARCHAR(50) NULL,
    LocationName NVARCHAR(200) NULL,
    Model NVARCHAR(50) NULL,
    ModelName NVARCHAR(100) NULL,
    Name NVARCHAR(200) NULL,
    OrganizationId NVARCHAR(50) NULL,
    PartNumber NVARCHAR(50) NULL,
    PayPerChargeEnabled BIT NOT NULL DEFAULT 0,
    PayPerMonthEnabled BIT NOT NULL DEFAULT 0,
    Puk INT NULL,
    SerialNumber NVARCHAR(50) NULL,
    SoftwareUpdateAvailable BIT NOT NULL DEFAULT 0,
    Status INT NULL,
    Timezone NVARCHAR(100) NULL,
    UniqueIdentifier NVARCHAR(50) NULL,
    CreatedAt DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET()
);

CREATE UNIQUE INDEX IX_Charger_SerialNumber ON wallbox.Charger(SerialNumber)

CREATE TABLE [wallbox].[Groups]
(
    [Id] [int] NOT NULL,
    [UniqueId] [varchar](100) NOT NULL PRIMARY KEY,
    [Name] [varchar](255) NOT NULL
)

CREATE TABLE [wallbox].[Sessions]
(
    [Id] [nvarchar](50) NOT NULL PRIMARY KEY,
    [StartTime] [datetimeoffset](7) NOT NULL,
    [EndTime] [datetimeoffset](7) NOT NULL,
    [ChargingTimeSeconds] [int] NOT NULL,
    [UserId] [int] NOT NULL,
    [UserUid] [nvarchar](50) NOT NULL,
    [UserName] [nvarchar](100) NOT NULL,
    [UserSurname] [nvarchar](100) NULL,
    [UserEmail] [nvarchar](255) NOT NULL,
    [ChargerId] [int] NOT NULL,
    [ChargerName] [nvarchar](100) NOT NULL,
    [ChargerUid] [nvarchar](50) NULL,
    [GroupId] [int] NOT NULL,
    [LocationId] [int] NOT NULL,
    [LocationName] [nvarchar](100) NOT NULL,
    [LocationUid] [nvarchar](50) NULL,
    [Energy] [int] NOT NULL,
    [MidEnergy] [int] NOT NULL,
    [EnergyPrice] [decimal](10, 4) NOT NULL,
    [CurrencyCode] [nvarchar](3) NOT NULL,
    [SessionType] [nvarchar](50) NOT NULL,
    [ApplicationFeePercentage] [decimal](5, 2) NOT NULL,
    [TotalCost] [decimal](10, 2) NOT NULL,
    [OrderUid] [nvarchar](50) NULL,
    [RatePrice] [decimal](10, 4) NULL,
    [RateVariableType] [nvarchar](50) NULL,
    [OrderEnergy] [int] NULL,
    [AccessPrice] [decimal](10, 2) NULL,
    [FeeAmount] [decimal](10, 2) NULL,
    [Total] [decimal](10, 2) NULL,
    [Subtotal] [decimal](10, 2) NULL,
    [TaxAmount] [decimal](10, 2) NULL,
    [TaxPercentage] [decimal](5, 2) NULL,
    [PublicChargeUid] [varchar](50) NULL,
    [OrganizationUid] [varchar](50) NULL
)