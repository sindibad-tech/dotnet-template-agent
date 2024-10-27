namespace Sindibad.SAD.AgentTemplate.Agent.ConfigModels;
internal class OpenTelemetryConfig
{
    public required bool Enabled { get; set; }
    public string? Endpoint { get; set; }
    public required Dictionary<string, string> Headers { get; set; } = [];

    public string JoinedHeaders => string.Join(',', Headers.Select(s => $"{s.Key}={s.Value}"));
}
