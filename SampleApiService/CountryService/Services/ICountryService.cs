using CountryService.Models;

namespace CountryService.Services;

public interface ICountryService
{
    IEnumerable<Country> GetAllCountries();
    CountryAddResult AddCountry(Country country);
}

public enum CountryAddResult
{
    Success,
    Duplicate,
    Invalid
}