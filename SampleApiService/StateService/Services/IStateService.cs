using StateService.Models;

namespace StateService.Services;

public interface IStateService
{
    IEnumerable<State> GetStates(string countryCode);
    StateAddResult AddState(string countryCode, State state);
}

public enum StateAddResult
{
    Success,
    Duplicate,
    Invalid
}