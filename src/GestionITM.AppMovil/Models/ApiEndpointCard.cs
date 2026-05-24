namespace GestionITM.AppMovil.Models;

public sealed class ApiEndpointCard
{
    public string Method { get; init; } = string.Empty;
    public string Path { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string AccessLabel { get; init; } = string.Empty;
    public string ExampleBody { get; init; } = string.Empty;
    public bool HasExampleBody => !string.IsNullOrWhiteSpace(ExampleBody);
}