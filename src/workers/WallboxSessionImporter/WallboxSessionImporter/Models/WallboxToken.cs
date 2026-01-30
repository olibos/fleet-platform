// <copyright file="WallboxToken.cs" company="Olibos">
// Copyright (c) Olibos. All rights reserved.
// </copyright>

namespace WallboxSessionImporter.Models;

using System.Text.Json.Serialization;
using WallboxSessionImporter.Serialization;

public class WallboxToken
{
    [JsonPropertyName("token")]
    public required string AccessToken { get; init; }

    [JsonPropertyName("ttl")]
    [JsonConverter(typeof(JsonDateTimeOffsetUnixTimeMillisecondsConverter))]
    public required DateTimeOffset AccessTokenExpiresOn { get; init; }

    [JsonPropertyName("refresh_token")]
    public required string RefreshToken { get; init; }

    [JsonPropertyName("refresh_token_ttl")]
    [JsonConverter(typeof(JsonDateTimeOffsetUnixTimeMillisecondsConverter))]
    public required DateTimeOffset RefreshTokenExpiresOn { get; init; }
}