// <copyright file="WallboxService.cs" company="Olibos">
// Copyright (c) Olibos. All rights reserved.
// </copyright>

namespace WallboxSessionImporter.Services;

using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization.Metadata;
using Microsoft.AspNetCore.WebUtilities;
using WallboxSessionImporter.Models;
using WallboxSessionImporter.Serialization;

public class WallboxService(HttpClient client, TimeProvider time)
{
    public IAsyncEnumerable<Group> GetAllGroups(CancellationToken cancellationToken = default)
    {
        return Paginate(
            "/v4/space-accesses?limit=999",
            WallboxJsonSerializerContext.Default.ResponseListObjectGroup,
            cancellationToken);
    }

    public IAsyncEnumerable<Session> GetAllSessions(int groupId, DateTimeOffset? from = null, CancellationToken cancellationToken = default)
    {
        DateTimeOffset tomorrow = time.GetUtcNow().Date.AddDays(1);
        var query = new Dictionary<string, string?>
        {
            ["filters"] = from is null ?
                            """
                            {"filters":[]}
                            """ :
                            $$"""
                            {"filters":[{"field":"start_time","operator":"gte","value":{{from.Value.ToUnixTimeSeconds()}}},{"field":"start_time","operator":"lte","value":{{tomorrow.ToUnixTimeSeconds()}}}]}
                            """,
            ["fields[charger_charging_session]"] = string.Empty,
            ["limit"] = "5000",
            ["offset"] = "0",
        };

        return Paginate(
            QueryHelpers.AddQueryString($"/v4/groups/{groupId}/charger-charging-sessions", query),
            WallboxJsonSerializerContext.Default.ResponseListObjectSession,
            cancellationToken);
    }

    private async IAsyncEnumerable<TModel> Paginate<TModel>(string? url, JsonTypeInfo<Response<List<Object<TModel>>>> jsonTypeInfo, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (url is not null)
        {
            var response = await client.GetFromJsonAsync(
                               url,
                               jsonTypeInfo,
                               cancellationToken)
                           ?? throw new InvalidDataException("Unable to deserialize the response");

            foreach (var item in response.Data)
            {
                yield return item.Attributes;
            }

            url = response.Links?.Next;
        }
    }
}