namespace ProjectManager.EndpointModels;

public static class EditTaskEndpoint
{
    public record Request(Guid TaskId, string Title, string? Description);
}