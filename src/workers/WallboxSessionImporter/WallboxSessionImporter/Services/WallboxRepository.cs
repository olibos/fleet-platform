// <copyright file="WallboxRepository.cs" company="Olibos">
// Copyright (c) Olibos. All rights reserved.
// </copyright>

namespace WallboxSessionImporter.Services;

using System.Data;
using System.Data.Common;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Olibos.Data.SqlClient.SqlBulkCopy;
using WallboxSessionImporter.Models;

public sealed class WallboxRepository(IOptions<Settings> options) : IDisposable, IAsyncDisposable
{
    private const string CreateTempSessionQuery =
        """
        CREATE TABLE #Session
        (
            Id                        NVARCHAR(50)      NOT NULL,
            StartTime                 DATETIMEOFFSET(7) NOT NULL,
            EndTime                   DATETIMEOFFSET(7) NOT NULL,
            ChargingTimeSeconds       INT               NOT NULL,
            UserId                    INT               NOT NULL,
            UserUid                   NVARCHAR(50)      NOT NULL,
            UserName                  NVARCHAR(100)     NOT NULL,
            UserSurname               NVARCHAR(100)     NULL,
            UserEmail                 NVARCHAR(255)     NOT NULL,
            ChargerId                 INT               NOT NULL,
            ChargerName               NVARCHAR(100)     NOT NULL,
            GroupId                   INT               NOT NULL,
            LocationId                INT               NOT NULL,
            LocationName              NVARCHAR(100)     NOT NULL,
            Energy                    INT               NOT NULL,
            MidEnergy                 INT               NOT NULL,
            EnergyPrice               DECIMAL(10, 4)    NOT NULL,
            CurrencyCode              NVARCHAR(3)       NOT NULL,
            SessionType               NVARCHAR(50)      NOT NULL,
            ApplicationFeePercentage  DECIMAL(5, 2)     NOT NULL,
            OrderUid                  NVARCHAR(50)      NULL,
            RatePrice                 DECIMAL(10, 4)    NULL,
            RateVariableType          NVARCHAR(50)      NULL,
            OrderEnergy               INT               NULL,
            AccessPrice               DECIMAL(10, 2)    NULL,
            FeeAmount                 DECIMAL(10, 2)    NULL,
            Total                     DECIMAL(10, 2)    NULL,
            Subtotal                  DECIMAL(10, 2)    NULL,
            TaxAmount                 DECIMAL(10, 2)    NULL,
            TaxPercentage             DECIMAL(5, 2)     NULL,
            TotalCost                 DECIMAL(10, 2)    NOT NULL,
            PublicChargeUid           VARCHAR(50)       NULL,
            OrganizationUid           VARCHAR(50)       NULL,
            ChargerUid                NVARCHAR(50)      NULL,
            LocationUid               NVARCHAR(50)      NULL
        );
        """;

    private readonly SqlConnection _connection = new(options.Value.ConnectionString);

    public async ValueTask<DbTransaction> BeginTransaction()
    {
        if (_connection.State != ConnectionState.Open)
        {
            await _connection.OpenAsync();
        }

        return await _connection.BeginTransactionAsync();
    }

    public void Dispose()
    {
        _connection.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _connection.DisposeAsync();
    }

    public async Task UpsertGroups(List<Group> groups, IDbTransaction? transaction = null)
    {
        foreach (var group in groups)
        {
            await _connection.ExecuteAsync(
                @"
            MERGE INTO wallbox.[Groups] AS target
            USING (values(@Id, @UniqueId, @Name)) AS source(Id, UniqueId, Name)
            ON target.UniqueId = source.UniqueId
            WHEN MATCHED THEN
                UPDATE SET 
                    Name = source.Name
            WHEN NOT MATCHED BY TARGET THEN
                INSERT (Id, UniqueId, Name)
                VALUES (source.Id, source.UniqueId, source.Name)
            ;",
                group,
                transaction: transaction);
        }
    }

    public async Task UpsertSessions(IAsyncEnumerable<Session> sessions, DbTransaction? transaction = null)
    {
        await _connection.ExecuteAsync(CreateTempSessionQuery, transaction: transaction);
        using var copy = new SqlBulkCopy(
            _connection,
            SqlBulkCopyOptions.Default,
            (SqlTransaction?)transaction);

        copy.DestinationTableName = "#Session";
        copy.ColumnMappings.Add(nameof(Session.Id), nameof(Session.Id));
        copy.ColumnMappings.Add(nameof(Session.StartTime), nameof(Session.StartTime));
        copy.ColumnMappings.Add(nameof(Session.EndTime), nameof(Session.EndTime));
        copy.ColumnMappings.Add(nameof(Session.ChargingTimeSeconds), nameof(Session.ChargingTimeSeconds));
        copy.ColumnMappings.Add(nameof(Session.UserId), nameof(Session.UserId));
        copy.ColumnMappings.Add(nameof(Session.UserUid), nameof(Session.UserUid));
        copy.ColumnMappings.Add(nameof(Session.UserName), nameof(Session.UserName));
        copy.ColumnMappings.Add(nameof(Session.UserSurname), nameof(Session.UserSurname));
        copy.ColumnMappings.Add(nameof(Session.UserEmail), nameof(Session.UserEmail));
        copy.ColumnMappings.Add(nameof(Session.ChargerId), nameof(Session.ChargerId));
        copy.ColumnMappings.Add(nameof(Session.ChargerName), nameof(Session.ChargerName));
        copy.ColumnMappings.Add(nameof(Session.GroupId), nameof(Session.GroupId));
        copy.ColumnMappings.Add(nameof(Session.LocationId), nameof(Session.LocationId));
        copy.ColumnMappings.Add(nameof(Session.LocationName), nameof(Session.LocationName));
        copy.ColumnMappings.Add(nameof(Session.Energy), nameof(Session.Energy));
        copy.ColumnMappings.Add(nameof(Session.MidEnergy), nameof(Session.MidEnergy));
        copy.ColumnMappings.Add(nameof(Session.EnergyPrice), nameof(Session.EnergyPrice));
        copy.ColumnMappings.Add(nameof(Session.CurrencyCode), nameof(Session.CurrencyCode));
        copy.ColumnMappings.Add(nameof(Session.SessionType), nameof(Session.SessionType));
        copy.ColumnMappings.Add(nameof(Session.ApplicationFeePercentage), nameof(Session.ApplicationFeePercentage));
        copy.ColumnMappings.Add(nameof(Session.OrderUid), nameof(Session.OrderUid));
        copy.ColumnMappings.Add(nameof(Session.RatePrice), nameof(Session.RatePrice));
        copy.ColumnMappings.Add(nameof(Session.RateVariableType), nameof(Session.RateVariableType));
        copy.ColumnMappings.Add(nameof(Session.OrderEnergy), nameof(Session.OrderEnergy));
        copy.ColumnMappings.Add(nameof(Session.AccessPrice), nameof(Session.AccessPrice));
        copy.ColumnMappings.Add(nameof(Session.FeeAmount), nameof(Session.FeeAmount));
        copy.ColumnMappings.Add(nameof(Session.Total), nameof(Session.Total));
        copy.ColumnMappings.Add(nameof(Session.Subtotal), nameof(Session.Subtotal));
        copy.ColumnMappings.Add(nameof(Session.TaxAmount), nameof(Session.TaxAmount));
        copy.ColumnMappings.Add(nameof(Session.TaxPercentage), nameof(Session.TaxPercentage));
        copy.ColumnMappings.Add(nameof(Session.TotalCost), nameof(Session.TotalCost));
        copy.ColumnMappings.Add(nameof(Session.PublicChargeUid), nameof(Session.PublicChargeUid));
        copy.ColumnMappings.Add(nameof(Session.OrganizationUid), nameof(Session.OrganizationUid));
        copy.ColumnMappings.Add(nameof(Session.ChargerUid), nameof(Session.ChargerUid));
        copy.ColumnMappings.Add(nameof(Session.LocationUid), nameof(Session.LocationUid));
        await copy.WriteToServerAsync(sessions);
        Console.WriteLine($"Rows Imported: {copy.RowsCopied}");

        await _connection.ExecuteAsync(
            """
            -- MERGE statement to insert if not exists, update if exists
            MERGE INTO wallbox.Sessions AS target
            USING #Session AS source
            ON target.Id = source.Id
            WHEN NOT MATCHED THEN
                INSERT (
                    Id, StartTime, EndTime, ChargingTimeSeconds,
                    UserId, UserUid, UserName, UserSurname, UserEmail,
                    ChargerId, ChargerName, ChargerUid,
                    GroupId, LocationId, LocationName, LocationUid,
                    Energy, MidEnergy, EnergyPrice,
                    CurrencyCode, SessionType, ApplicationFeePercentage, TotalCost,
                    OrderUid, RatePrice, RateVariableType, OrderEnergy,
                    AccessPrice, FeeAmount, Total, Subtotal,
                    TaxAmount, TaxPercentage, PublicChargeUid, OrganizationUid
                )
                VALUES (
                    source.Id, source.StartTime, source.EndTime, source.ChargingTimeSeconds,
                    source.UserId, source.UserUid, source.UserName, source.UserSurname, source.UserEmail,
                    source.ChargerId, source.ChargerName, source.ChargerUid,
                    source.GroupId, source.LocationId, source.LocationName, source.LocationUid,
                    source.Energy, source.MidEnergy, source.EnergyPrice,
                    source.CurrencyCode, source.SessionType, source.ApplicationFeePercentage, source.TotalCost,
                    source.OrderUid, source.RatePrice, source.RateVariableType, source.OrderEnergy,
                    source.AccessPrice, source.FeeAmount, source.Total, source.Subtotal,
                    source.TaxAmount, source.TaxPercentage, source.PublicChargeUid, source.OrganizationUid
                );
            """,
            transaction: transaction);
    }
}