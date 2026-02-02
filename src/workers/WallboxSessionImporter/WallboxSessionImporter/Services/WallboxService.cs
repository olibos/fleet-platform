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
    public IAsyncEnumerable<Charger> GetAllChargers(string groupUid, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>
        {
            ["filters"] = "[]",
            ["include"] = "charger_info,charger_config,charger_status",
            ["limit"] = "50",
            ["offset"] = "0",
        };

        return Paginate(
            QueryHelpers.AddQueryString($"/perseus/organizations/{groupUid}/chargers", query),
            WallboxJsonSerializerContext.Default.ResponseListObjectCharger,
            cancellationToken);
    }

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
            ["limit"] = "50",
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

            url = PatchUrl(url, response.Links?.Next);
        }
    }

    private string? PatchUrl(string url, Uri? linksNext)
    {
        if (linksNext is null) return null;

        var baseUri = client.BaseAddress is not null ? new Uri(client.BaseAddress, url) : new Uri(url);
        var result = new UriBuilder(baseUri)
        {
            Query = linksNext.Query,
        };
        return result.ToString();
    }
}