using System.Collections.Concurrent;
using CountryService.Models;

namespace CountryService.Services;

public class CountryService : ICountryService
{
    private readonly ConcurrentDictionary<string, string> _countries;

    public CountryService()
    {
        _countries = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        _countries.TryAdd("IN", "India");
        _countries.TryAdd("US", "United States");
        _countries.TryAdd("GB", "United Kingdom");
    }

    public IEnumerable<Country> GetAllCountries()
    {
        return _countries
            .Select(kv => new Country(kv.Key.ToUpperInvariant(), kv.Value))
            .OrderBy(c => c.Name);
    }

    public CountryAddResult AddCountry(Country country)
    {
        if (string.IsNullOrWhiteSpace(country.Code) || string.IsNullOrWhiteSpace(country.Name))
            return CountryAddResult.Invalid;

        if (!_countries.TryAdd(country.Code.Trim(), country.Name.Trim()))
            return CountryAddResult.Duplicate;

        return CountryAddResult.Success;
    }
}