// <copyright file="WallboxAuthentication.cs" company="Olibos">
// Copyright (c) Olibos. All rights reserved.
// </copyright>

namespace WallboxSessionImporter.Services;

using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Microsoft.Extensions.Options;
using WallboxSessionImporter.Models;
using WallboxSessionImporter.Serialization;

public class WallboxAuthentication(HttpClient client, IOptions<Wallbox> options)
{
    private WallboxToken? _token;

    public async Task<WallboxToken> GetToken(CancellationToken cancellationToken = default)
    {
        if (_token?.AccessTokenExpiresOn <= DateTimeOffset.UtcNow) return _token;
        var basic = Encoding.UTF8.GetBytes($"{options.Value.UserName}:{options.Value.Password}");
        var request = new HttpRequestMessage(HttpMethod.Get, "/users/signin")
        {
            Headers =
            {
              Authorization = new("Basic", Convert.ToBase64String(basic)),
              UserAgent = { new ProductInfoHeaderValue("WallboxSessionImporter", "1.0.0") },
              Accept = { new MediaTypeWithQualityHeaderValue("application/json") },
            },
        };
        request.Headers.Add("Partner", "wallbox");
        var response = await client.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
        var payload = await response.Content.ReadFromJsonAsync<Response<Object<WallboxToken>>>(WallboxJsonSerializerContext.Default.ResponseObjectWallboxToken, cancellationToken: cancellationToken);
        _token = payload?.Data.Attributes ?? throw new MissingFieldException("Invalid response");
        return _token;
    }
}