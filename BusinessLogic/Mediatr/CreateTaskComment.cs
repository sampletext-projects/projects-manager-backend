using DataAccess;
using DataAccess.Models;
using DataAccess.RepositoryNew;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Mediatr;

public static class CreateTaskComment
{
    public record Command(Guid UserId, Guid TaskId, string Content) : IRequest<CommandResult>;

    public record CommandResult(Guid Id);

    public class Handler : IRequestHandler<Command, CommandResult>
    {
        private readonly IRepository<ProjectTask> _taskRepository;
        private readonly IRepository<TaskComment> _taskCommentRepository;
        private readonly IDbExtensions _dbExtensions;

        public Handler(IDbExtensions dbExtensions, IRepository<TaskComment> taskCommentRepository, IRepository<ProjectTask> taskRepository)
        {
            _dbExtensions = dbExtensions;
            _taskCommentRepository = taskCommentRepository;
            _taskRepository = taskRepository;
        }

        public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var canEdit = await _dbExtensions.CanEditTask(request.UserId, request.TaskId, cancellationToken);

            if (!canEdit)
            {
                throw new BusinessException("Вы не можете добавлять комментарии к данной задаче");
            }

            var taskComment = new TaskComment()
            {
                Id = Guid.NewGuid(),
                Content = request.Content,
                AuthorId = request.UserId,
                TaskId = request.TaskId,
                CreatedAt = DateTime.UtcNow
            };

            await _taskCommentRepository.Add(taskComment, cancellationToken);

            return new CommandResult(taskComment.Id);
        }
    }
}