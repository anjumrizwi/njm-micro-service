using DistrictService.Models;

namespace DistrictService.Services;

public interface IDistrictService
{
    IEnumerable<District> GetDistricts(string countryCode, string stateCode);
    DistrictAddResult AddDistrict(string countryCode, string stateCode, District district);
}

public enum DistrictAddResult
{
    Success,
    Duplicate,
    Invalid,
    CountryNotFound,
    StateNotFound
}