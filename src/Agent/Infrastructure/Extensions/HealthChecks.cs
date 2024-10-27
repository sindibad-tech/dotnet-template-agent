using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace Sindibad.SAD.AgentTemplate.Agent.Infrastructure.Extensions;
public static class HealthChecks
{
    #region Constants

    #endregion

    #region Configuration

    public static void ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
    {
        // try adding all future required health checks here for: external services, databases, etc
        services
            .AddHealthChecks();
    }

    #endregion

    #region Util

    // healh allows successful response when degraded since app must self-heal
    private static readonly IDictionary<HealthStatus, HttpStatusCode> HealthStatuses = new Dictionary<HealthStatus, HttpStatusCode>()
    {
        [HealthStatus.Healthy] = HttpStatusCode.OK,
        [HealthStatus.Degraded] = HttpStatusCode.OK,
        [HealthStatus.Unhealthy] = HttpStatusCode.ServiceUnavailable,
    };

    // rediness returns failed response on degraded to stop incoming traffic
    private static readonly IDictionary<HealthStatus, HttpStatusCode> ReadinessStatuses = new Dictionary<HealthStatus, HttpStatusCode>()
    {
        [HealthStatus.Healthy] = HttpStatusCode.OK,
        [HealthStatus.Degraded] = HttpStatusCode.FailedDependency,
        [HealthStatus.Unhealthy] = HttpStatusCode.ServiceUnavailable,
    };

    #endregion
}
