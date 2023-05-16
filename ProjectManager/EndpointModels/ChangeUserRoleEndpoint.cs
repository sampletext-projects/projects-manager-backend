using DataAccess.Models;

namespace ProjectManager.EndpointModels;

public static class ChangeUserRoleEndpoint
{
    public record Request(Guid ParticipantId, Guid ProjectId, ParticipationRole Role);
}