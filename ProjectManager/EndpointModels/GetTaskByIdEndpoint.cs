using Microsoft.AspNetCore.Mvc;

namespace ProjectManager.EndpointModels;

public static class GetTaskByIdEndpoint
{
    public record Request([FromQuery] Guid TaskId);
}