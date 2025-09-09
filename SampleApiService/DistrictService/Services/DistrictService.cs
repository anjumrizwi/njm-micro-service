using System.Collections.Concurrent;
using DistrictService.Models;

namespace DistrictService.Services;

public class DistrictService : IDistrictService
{
    // country -> state -> districtCode -> districtName
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentDictionary<string, string>>> _store;

    public DistrictService()
    {
        _store = new(StringComparer.OrdinalIgnoreCase);
        // Seed
        _store.TryAdd("IN", new(StringComparer.OrdinalIgnoreCase));
        _store["IN"].TryAdd("BR", new(StringComparer.OrdinalIgnoreCase));
        _store["IN"]["BR"].TryAdd("BH", "Bhagalpur");
        _store["IN"]["BR"].TryAdd("PT", "Patna");
    }

    public IEnumerable<District> GetDistricts(string countryCode, string stateCode)
    {
        if (!_store.TryGetValue(countryCode, out var states))
            return Enumerable.Empty<District>();
        if (!states.TryGetValue(stateCode, out var districts))
            return Enumerable.Empty<District>();
        return districts
            .Select(kv => new District(kv.Key.ToUpperInvariant(), kv.Value))
            .OrderBy(d => d.Name);
    }

    public DistrictAddResult AddDistrict(string countryCode, string stateCode, District district)
    {
        if (string.IsNullOrWhiteSpace(district.Code) || string.IsNullOrWhiteSpace(district.Name))
            return DistrictAddResult.Invalid;

        var states = _store.GetOrAdd(countryCode, _ => new(StringComparer.OrdinalIgnoreCase));
        var districts = states.GetOrAdd(stateCode, _ => new(StringComparer.OrdinalIgnoreCase));
        if (districts.ContainsKey(district.Code.Trim()))
            return DistrictAddResult.Duplicate;

        districts.TryAdd(district.Code.Trim(), district.Name.Trim());
        return DistrictAddResult.Success;
    }
}