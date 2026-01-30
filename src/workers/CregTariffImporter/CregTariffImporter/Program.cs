using CregTariffImporter;
using CregTariffImporter.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

var services = new ServiceCollection();
services.Configure<Settings>(configuration.GetSection("Settings"));
services.AddTransient<CregRepository>();
services.AddHttpClient<CregService>(static client =>
    client.BaseAddress = new("https://www.creg.be/sites/default/files/assets/Prices/CREG_Tariff_EV.csv"));
var sp = services.BuildServiceProvider();

var repository = sp.GetRequiredService<CregRepository>();
await using var transaction = await repository.BeginTransaction();
var latest = await repository.GetLatest(transaction);
await foreach (var tariff in sp.GetRequiredService<CregService>().GetAll())
{
    if (tariff.Month <= latest) break;

    await repository.Add(tariff, transaction);
}

await transaction.CommitAsync();
