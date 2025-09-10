using System.Collections.Concurrent;

namespace StateService.Services
{
    public static class StateList
    {
        // Seed
        private static readonly ConcurrentDictionary<string, string> _IndianStates = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["AP"] = "Andhra Pradesh",
            ["AR"] = "Arunachal Pradesh",
            ["AS"] = "Assam",
            ["BR"] = "Bihar",
            ["CT"] = "Chhattisgarh",
            ["GA"] = "Goa",
            ["GJ"] = "Gujarat",
            ["HR"] = "Haryana",
            ["HP"] = "Himachal Pradesh",
            ["JH"] = "Jharkhand",
            ["KA"] = "Karnataka",
            ["KL"] = "Kerala",
            ["MP"] = "Madhya Pradesh",
            ["MH"] = "Maharashtra",
            ["MN"] = "Manipur",
            ["ML"] = "Meghalaya",
            ["MZ"] = "Mizoram",
            ["NL"] = "Nagaland",
            ["OR"] = "Odisha",
            ["PB"] = "Punjab",
            ["RJ"] = "Rajasthan",
            ["SK"] = "Sikkim",
            ["TN"] = "Tamil Nadu",
            ["TG"] = "Telangana",
            ["TR"] = "Tripura",
            ["UP"] = "Uttar Pradesh",
            ["UT"] = "Uttarakhand",
            ["WB"] = "West Bengal",
            ["AN"] = "Andaman and Nicobar Islands",
            ["CH"] = "Chandigarh",
            ["DN"] = "Dadra and Nagar Haveli and Daman and Diu",
            ["DL"] = "Delhi",
            ["JK"] = "Jammu and Kashmir",
            ["LA"] = "Ladakh",
            ["LD"] = "Lakshadweep",
            ["PY"] = "Puducherry"
        };
        // Seed
        private static readonly ConcurrentDictionary<string, string> _UnitedStates = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["CA"] = "California",
            ["WA"] = "Washington",
            ["AL"] = "Alabama",
            ["AK"] = "Alaska",
            ["AZ"] = "Arizona",
            ["AR"] = "Arkansas",
            ["CO"] = "Colorado",
            ["CT"] = "Connecticut",
            ["DE"] = "Delaware",
            ["FL"] = "Florida",
            ["GA"] = "Georgia",
            ["HI"] = "Hawaii",
            ["ID"] = "Idaho",
            ["IL"] = "Illinois",
            ["IN"] = "Indiana",
            ["IA"] = "Iowa",
            ["KS"] = "Kansas",
            ["KY"] = "Kentucky",
            ["LA"] = "Louisiana",
            ["ME"] = "Maine",
            ["MD"] = "Maryland",
            ["MA"] = "Massachusetts",
            ["MI"] = "Michigan",
            ["MN"] = "Minnesota",
            ["MS"] = "Mississippi",
            ["MO"] = "Missouri",
            ["MT"] = "Montana",
            ["NE"] = "Nebraska",
            ["NV"] = "Nevada",
            ["NH"] = "New Hampshire",
            ["NJ"] = "New Jersey",
            ["NM"] = "New Mexico",
            ["NY"] = "New York",
            ["NC"] = "North Carolina",
            ["ND"] = "North Dakota",
            ["OH"] = "Ohio",
            ["OK"] = "Oklahoma",
            ["OR"] = "Oregon",
            ["PA"] = "Pennsylvania",
            ["RI"] = "Rhode Island",
            ["SC"] = "South Carolina",
            ["SD"] = "South Dakota",
            ["TN"] = "Tennessee",
            ["TX"] = "Texas",
            ["UT"] = "Utah",
            ["VT"] = "Vermont",
            ["VA"] = "Virginia",
            ["WV"] = "West Virginia",
            ["WI"] = "Wisconsin",
            ["WY"] = "Wyoming"
        };

        public static ConcurrentDictionary<string, ConcurrentDictionary<string, string>> GetCountriesWithState()
        {
            var countries = new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

            countries.TryAdd("IN", _IndianStates);
            countries.TryAdd("US", _UnitedStates);
           
            return countries;
        }

        public static ConcurrentDictionary<string, string>? GetStatesByCountryCode(string countryCode)
        {
            var countries = GetCountriesWithState();

            countries.TryGetValue(countryCode, out var stateList);
            return stateList;
        }
    }
}
