using PostalService.Models;

namespace PostalService.Services;

public interface IPostalService
{
    PostcodeInfo? GetPostcodeInfo(string code);
}