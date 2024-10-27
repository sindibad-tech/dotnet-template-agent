using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;
using Sindibad.SAD.AgentTemplate.Agent.Infrastructure.FeatureFlags;

namespace Sindibad.SAD.AgentTemplate.Agent.Infrastructure.Extensions;
public static class FeatureFlags
{
    #region Constants

    private const string CONFIGURATION_SECTION = "FeatureFlags";

    private const string CONFIGURATION_DISABLED_KEY = "Disabled";

    #endregion

    #region Configuraation

    public static void ConfigureFeatureFlags(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
    {
        var config = configuration.GetSection(CONFIGURATION_SECTION);

        services.AddFeatureManagement();

        if (bool.TryParse(config[CONFIGURATION_DISABLED_KEY], out var disabled) && disabled)
        {
            services.RemoveAll<IFeatureManager>();
            services.AddSingleton<IFeatureManager, BypassingFeatureManager>();
        }
    }

    #endregion
}
