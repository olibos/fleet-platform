// <copyright file="Program.cs" company="Olibos">
// Copyright (c) Olibos. All rights reserved.
// </copyright>

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WallboxSessionImporter;
using WallboxSessionImporter.Http;
using WallboxSessionImporter.Services;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

var services = new ServiceCollection();
services.Configure<Settings>(configuration.GetSection(nameof(Settings)));
services.Configure<Wallbox>(configuration.GetSection(nameof(Wallbox)));
services.AddSingleton<WallboxAuthentication>();
services.AddTransient<WallboxRepository>();
services.AddTransient<AuthenticatedHttpClientHandler>();
services.AddSingleton<TimeProvider>(_ => TimeProvider.System);
services.AddHttpClient<WallboxAuthentication>(static (sp, client) =>
    client.BaseAddress = sp.GetRequiredService<IOptions<Wallbox>>().Value.LoginBaseUrl);
services.AddHttpClient<WallboxService>(static (sp, client) =>
        client.BaseAddress = sp.GetRequiredService<IOptions<Wallbox>>().Value.ApiBaseUrl)
    .AddHttpMessageHandler<AuthenticatedHttpClientHandler>();
var sp = services.BuildServiceProvider();

var client = sp.GetRequiredService<WallboxService>();
await using var repository = sp.GetRequiredService<WallboxRepository>();
await using var transaction = await repository.BeginTransaction();
DateTimeOffset? from = DateTimeOffset.UtcNow.AddMonths(-1);
var groups = await client.GetAllGroups().ToListAsync();
await repository.UpsertGroups(groups, transaction);
var sessions = groups.ToAsyncEnumerable().SelectMany(g => client.GetAllSessions(g.Id, from));
await repository.UpsertSessions(sessions, transaction);
await transaction.CommitAsync();
