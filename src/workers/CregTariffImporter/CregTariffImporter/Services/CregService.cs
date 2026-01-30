namespace CregTariffImporter.Services;

using System.Globalization;
using System.Runtime.CompilerServices;
using CregTariffImporter.Models;

public class CregService(HttpClient client)
{
    private static readonly IFormatProvider Culture = new NumberFormatInfo { NumberDecimalSeparator = "," };
    
    public async IAsyncEnumerable<Tariffs>  GetAll([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await using var stream = await client.GetStreamAsync((string?)null, cancellationToken);
        using var reader = new StreamReader(stream);
        var header = await reader.ReadLineAsync(cancellationToken);
        if (header !=
            "Year;Month;Flanders - Digital meter - End-user Price EV (c€/kWh);Flanders - Digital meter - Average 3 months (M-2 to M-4);Brussels - Classic meter - End-user Price EV (c€/kWh);Brussels - Classic meter - Average 3 months (M-2 to M-4);Wallonia - Classic meter - End-user Price EV (c€/kWh);Wallonia - Classic meter - Average 3 months (M-2 to M-4)")
        {
            throw new InvalidDataException();
        }
        
        for (var line = await reader.ReadLineAsync(cancellationToken);
             line != null;
             line = await reader.ReadLineAsync(cancellationToken))
        {
            var span = line.AsSpan();
            var year = Parse<int>(ref span);
            var month = Parse<int>(ref span);
            var flanders = Parse<decimal>(ref span);
            Skip(ref span);
            var brussels = Parse<decimal>(ref span);
            Skip(ref span);
            var wallonia = Parse<decimal>(ref span);
            yield return new Tariffs
            {
                Month = new DateOnly(year, month, 1),
                Flanders = flanders,
                Brussels = brussels,
                Wallonia = wallonia,
            };
        }
    }

    
    private static T Parse<T>(ref ReadOnlySpan<char> span) where T : ISpanParsable<T>
    {
        int separatorIndex = span.IndexOf(';');
        if (separatorIndex == -1)
        {
            separatorIndex = span.Length;
        }
        
        var segment = span[..separatorIndex];
        
        var result = T.Parse(segment, Culture);
        span = span[Math.Min(separatorIndex + 1, span.Length)..];
        
        return result;
    }
    
    private static void Skip(ref ReadOnlySpan<char> span)
    {
        int separatorIndex = span.IndexOf(';');

        if (separatorIndex == -1) return;
        span = span[(separatorIndex + 1)..];
    }
}