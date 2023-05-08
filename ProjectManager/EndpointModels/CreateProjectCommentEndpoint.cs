namespace ProjectManager.EndpointModels;

public static class CreateProjectCommentEndpoint
{
    public record Request(Guid ProjectId, string Content);
}