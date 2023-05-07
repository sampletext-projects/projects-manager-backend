using DataAccess.Models;

namespace ProjectManager.EndpointModels;

public static class CreateProjectEndpoint
{
    public record Request(string Title, string? Description, ProjectStyle Style, Visibility Visibility);
}