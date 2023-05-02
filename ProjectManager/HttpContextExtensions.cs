using BusinessLogic;

namespace ProjectManager;

public static class HttpContextExtensions
{
    public static Guid GetUserId(this HttpContext context)
    {
        if (!Guid.TryParse(
                context.User
                    .FindFirst(KnownJwtClaims.UserId)
                    ?.Value,
                out var userId
            ))
        {
            throw new InvalidOperationException("User is not authorized");
        }

        return userId;
    }
}