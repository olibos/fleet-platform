// <copyright file="Group.cs" company="Olibos">
// Copyright (c) Olibos. All rights reserved.
// </copyright>

namespace WallboxSessionImporter.Models;

using System.Text.Json.Serialization;

public sealed class Group
{
    [JsonPropertyName("group_id")]
    public int Id { get; set; }

    [JsonPropertyName("group_uid")]
    public required string UniqueId { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }
}