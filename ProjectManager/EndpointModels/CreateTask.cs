namespace ProjectManager.EndpointModels;

public static class CreateTaskEndpoint
{
    public record Request(Guid ProjectId, string Title, string? Description);
}