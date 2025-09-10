using System.Collections.Concurrent;
using StateService.Models;

namespace StateService.Services;

public class StateService : IStateService
{
    // countryCode -> stateCode -> stateName
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, string>> _store;

    public StateService()
    {
        _store = StateList.GetCountriesWithState();
    }

    public IEnumerable<State> GetStates(string countryCode)
    {
        if (!_store.TryGetValue(countryCode, out var states))
            return Enumerable.Empty<State>();
        return states
            .Select(kv => new State(kv.Key.ToUpperInvariant(), kv.Value))
            .OrderBy(s => s.Name);
    }

    public StateAddResult AddState(string countryCode, State state)
    {
        if (string.IsNullOrWhiteSpace(state.Code) || string.IsNullOrWhiteSpace(state.Name))
            return StateAddResult.Invalid;

        var states = _store.GetOrAdd(countryCode, _ => new(StringComparer.OrdinalIgnoreCase));
        if (!states.TryAdd(state.Code.Trim(), state.Name.Trim()))
            return StateAddResult.Duplicate;

        return StateAddResult.Success;
    }
}