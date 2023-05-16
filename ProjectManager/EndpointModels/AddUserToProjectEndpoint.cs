using DataAccess.Models;

namespace ProjectManager.EndpointModels;

public static class AddUserToProjectEndpoint
{
    public record Request(Guid UserId, Guid ProjectId, ParticipationRole Role);
}