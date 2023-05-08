using DataAccess.Models;

namespace ProjectManager.EndpointModels;

public static class EditProjectEndpoint
{
    public record Request(Guid ProjectId, string Title, string? Description, ProjectStyle Style, Visibility Visibility);
}