using Microsoft.FeatureManagement;

namespace Sindibad.SAD.AgentTemplate.Agent.Infrastructure.FeatureFlags;
public class BypassingFeatureManager : IFeatureManager
{
    public async IAsyncEnumerable<string> GetFeatureNamesAsync()
    {
        await Task.CompletedTask;
        yield break;
    }

    public Task<bool> IsEnabledAsync(string _) => Task.FromResult(true);

    public Task<bool> IsEnabledAsync<TContext>(string _1, TContext _2) => Task.FromResult(true);
}
