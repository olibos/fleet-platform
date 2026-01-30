namespace CregTariffImporter;

using System.Diagnostics.CodeAnalysis;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
public class Settings
{
    public required string ConnectionString { get; set; }
}