using PostalService.Models;

namespace PostalService.Services;

public class PostalService : IPostalService
{
    private readonly Dictionary<string, PostcodeInfo> _data;

    public PostalService()
    {
        _data = new(StringComparer.OrdinalIgnoreCase)
        {
            ["812006"] = new PostcodeInfo("IN", "BR", "Bhagalpur", "Sajour", "812006"),
            ["560083"] = new PostcodeInfo("IN", "KA", "Bengaluru Urban", "Bannerghatta Road", "560083"),
            ["94105"]  = new PostcodeInfo("US", "CA", "San Francisco", "SOMA", "94105")
        };
    }

    public PostcodeInfo? GetPostcodeInfo(string code)
    {
        _data.TryGetValue(code, out var info);
        return info;
    }
}