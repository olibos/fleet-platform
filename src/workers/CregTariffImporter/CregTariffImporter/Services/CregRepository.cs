using System.Data;
using System.Data.Common;
using CregTariffImporter.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace CregTariffImporter.Services;

public sealed class CregRepository(IOptions<Settings> options): IDisposable, IAsyncDisposable
{
    private readonly SqlConnection _connection = new(options.Value.ConnectionString);

    public async ValueTask<DbTransaction> BeginTransaction()
    {
        if (_connection.State != ConnectionState.Open)
        {
            await _connection.OpenAsync();
        }

        return await _connection.BeginTransactionAsync();
    }
    
    public async Task<DateOnly> GetLatest(IDbTransaction? transaction = null)
    {
        return await _connection.ExecuteScalarAsync<DateOnly?>("select max([Month]) from creg.Tariffs", transaction: transaction) ?? DateOnly.MinValue;
    }

    public async Task Add(Tariffs tariff, IDbTransaction? transaction = null)
    {
        await _connection.ExecuteAsync("insert into creg.Tariffs([Month], Brussels, Flanders, Wallonia) values (@Month, @Brussels, @Flanders, @Wallonia)", tariff, transaction: transaction);
    }

    public void Dispose()
    {
        _connection.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _connection.DisposeAsync();
    }
}