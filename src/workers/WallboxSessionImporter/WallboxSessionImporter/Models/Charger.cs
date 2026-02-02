// <copyright file="Charger.cs" company="Olibos">
// Copyright (c) Olibos. All rights reserved.
// </copyright>

namespace WallboxSessionImporter.Models;

using System;
using System.Text.Json.Serialization;
using Olibos.Data.SqlClient.SqlBulkCopy;
using WallboxSessionImporter.Serialization;

[SqlBulkCopy]
public class Charger
{
    [JsonPropertyName("connection_status")]
    [JsonConverter(typeof(JsonStringEnumConverter<ConnectionStatus>))]
    public ConnectionStatus ConnectionStatus { get; set; }

    [JsonPropertyName("connector_type")]
    public required string ConnectorType { get; set; }

    [JsonPropertyName("image")]
    public required string Image { get; set; }

    [JsonPropertyName("last_connection")]
    [JsonConverter(typeof(JsonDateTimeOffsetUnixTimeSecondsConverter))]
    public DateTimeOffset LastConnection { get; set; }

    [JsonPropertyName("location_id")]
    public required string LocationId { get; set; }

    [JsonPropertyName("location_name")]
    public required string LocationName { get; set; }

    [JsonPropertyName("model")]
    public required string Model { get; set; }

    [JsonPropertyName("model_name")]
    public required string ModelName { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("organization_id")]
    public required string OrganizationId { get; set; }

    [JsonPropertyName("part_number")]
    public required string PartNumber { get; set; }

    [JsonPropertyName("pay_per_charge_enabled")]
    public bool PayPerChargeEnabled { get; set; }

    [JsonPropertyName("pay_per_month_enabled")]
    public bool PayPerMonthEnabled { get; set; }

    [JsonPropertyName("puk")]
    public int Puk { get; set; }

    [JsonPropertyName("serial_number")]
    public required string SerialNumber { get; set; }

    [JsonPropertyName("software_update_available")]
    public bool SoftwareUpdateAvailable { get; set; }

    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("timezone")]
    public required string Timezone { get; set; }

    [JsonPropertyName("unique_identifier")]
    public required string UniqueIdentifier { get; set; }
}