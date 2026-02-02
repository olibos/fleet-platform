// <copyright file="ConnectionStatus.cs" company="Olibos">
// Copyright (c) Olibos. All rights reserved.
// </copyright>

namespace WallboxSessionImporter.Models;

using System.Text.Json.Serialization;

public enum ConnectionStatus
{
    [JsonPropertyName("offline")]
    Offline,

    [JsonPropertyName("online")]
    Online,
}