using DataAccess;
using DataAccess.Models;
using DataAccess.RepositoryNew;
using MediatR;

namespace BusinessLogic.Mediatr;

public static class CreateProjectComment
{
    public record Command(Guid UserId, Guid ProjectId, string Content) : IRequest<CommandResult>;

    public record CommandResult(Guid Id);
    
    public class Handler : IRequestHandler<Command, CommandResult>
    {
        private readonly IRepository<ProjectComment> _repository;
        private readonly IDbExtensions _dbExtensions;

        public Handler(IRepository<ProjectComment> repository, IDbExtensions dbExtensions)
        {
            _repository = repository;
            _dbExtensions = dbExtensions;
        }

        public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var canEdit = await _dbExtensions.CanEditProject(request.UserId, request.ProjectId, cancellationToken);

            if (!canEdit)
            {
                throw new BusinessException("Вы не можете добавлять комментарии к данному проекту");
            }

            var projectComment = new ProjectComment()
            {
                Id = Guid.NewGuid(),
                Content = request.Content,
                AuthorId = request.UserId,
                ProjectId = request.ProjectId,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.Add(projectComment, cancellationToken);

            return new CommandResult(projectComment.Id);
        }
    }
}