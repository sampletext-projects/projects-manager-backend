namespace ProjectManager.EndpointModels;

public static class CreateTaskCommentEndpoint
{
    public record Request(Guid TaskId, string Content);
}