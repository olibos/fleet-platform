// <copyright file="Session.cs" company="Olibos">
// Copyright (c) Olibos. All rights reserved.
// </copyright>

namespace WallboxSessionImporter.Models;

using System.Text.Json.Serialization;
using Olibos.Data.SqlClient.SqlBulkCopy;
using WallboxSessionImporter.Serialization;

[SqlBulkCopy]
public class Session
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("start_time")]
    [JsonConverter(typeof(JsonDateTimeOffsetUnixTimeSecondsConverter))]
    public required DateTimeOffset StartTime { get; init; }

    [JsonPropertyName("end_time")]
    [JsonConverter(typeof(JsonDateTimeOffsetUnixTimeSecondsConverter))]
    public required DateTimeOffset EndTime { get; init; }

    [JsonPropertyName("charging_time")]
    public required int ChargingTimeSeconds { get; init; }

    [JsonPropertyName("user_id")]
    public required int UserId { get; init; }

    [JsonPropertyName("user_uid")]
    public required string UserUid { get; init; }

    [JsonPropertyName("user_name")]
    public required string UserName { get; init; }

    [JsonPropertyName("user_surname")]
    public string? UserSurname { get; init; }

    [JsonPropertyName("user_email")]
    public required string UserEmail { get; init; }

    [JsonPropertyName("charger_id")]
    public required int ChargerId { get; init; }

    [JsonPropertyName("charger_name")]
    public required string ChargerName { get; init; }

    [JsonPropertyName("group_id")]
    public required int GroupId { get; init; }

    [JsonPropertyName("location_id")]
    public required int LocationId { get; init; }

    [JsonPropertyName("location_name")]
    public required string LocationName { get; init; }

    /// <summary>
    /// Gets the energy consumed in Wh (watt-hours).
    /// </summary>
    [JsonPropertyName("energy")]
    public required int Energy { get; init; }

    [JsonPropertyName("mid_energy")]
    public required int MidEnergy { get; init; }

    /// <summary>
    /// Gets the energy price per kWh.
    /// </summary>
    [JsonPropertyName("energy_price")]
    public required decimal EnergyPrice { get; init; }

    [JsonPropertyName("currency_code")]
    public required string CurrencyCode { get; init; }

    [JsonPropertyName("session_type")]
    public required string SessionType { get; init; }

    [JsonPropertyName("application_fee_percentage")]
    public required decimal ApplicationFeePercentage { get; init; }

    [JsonPropertyName("order_uid")]
    public string? OrderUid { get; init; }

    [JsonPropertyName("rate_price")]
    public decimal? RatePrice { get; init; }

    [JsonPropertyName("rate_variable_type")]
    public string? RateVariableType { get; init; }

    [JsonPropertyName("order_energy")]
    public int? OrderEnergy { get; init; }

    [JsonPropertyName("access_price")]
    public decimal? AccessPrice { get; init; }

    [JsonPropertyName("fee_amount")]
    public decimal? FeeAmount { get; init; }

    [JsonPropertyName("total")]
    public decimal? Total { get; init; }

    [JsonPropertyName("subtotal")]
    public decimal? Subtotal { get; init; }

    [JsonPropertyName("tax_amount")]
    public decimal? TaxAmount { get; init; }

    [JsonPropertyName("tax_percentage")]
    public decimal? TaxPercentage { get; init; }

    [JsonPropertyName("total_cost")]
    public required decimal TotalCost { get; init; }

    [JsonPropertyName("public_charge_uid")]
    public string? PublicChargeUid { get; init; }

    [JsonPropertyName("organization_uid")]
    public string? OrganizationUid { get; init; }

    [JsonPropertyName("charger_uid")]
    public string? ChargerUid { get; init; }

    [JsonPropertyName("location_uid")]
    public string? LocationUid { get; init; }
}