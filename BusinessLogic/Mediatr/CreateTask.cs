using DataAccess;
using DataAccess.Models;
using DataAccess.RepositoryNew;
using FluentValidation;
using MediatR;
using TaskStatus = DataAccess.Models.TaskStatus;

namespace BusinessLogic.Mediatr;

public static class CreateTask
{
    public record Command(Guid UserId, Guid ProjectId, string Title, string? Description) : IRequest<CommandResult>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Название не должно быть пусто");

            RuleFor(x => x.ProjectId)
                .NotEmpty()
                .WithMessage("Не выбран проект.");

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Не заполнен заголовок");
        }
    }

    public record CommandResult(Guid TaskId);
    
    public class Handler : IRequestHandler<Command, CommandResult>
    {
        private readonly IRepository<ProjectTask> _repository;

        private readonly IDbExtensions _dbExtensions;

        public Handler(IRepository<ProjectTask> repository, IDbExtensions dbExtensions)
        {
            _repository = repository;
            _dbExtensions = dbExtensions;
        }

        public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var canEdit = await _dbExtensions.CanEditProject(request.UserId, request.ProjectId, cancellationToken);

            if (!canEdit)
            {
                throw new BusinessException("Вы не можете редактировать данный проект");
            }

            var projectTask = new ProjectTask()
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                CreatorId = request.UserId,
                ProjectId = request.ProjectId,
                Status = TaskStatus.Created
            };

            await _repository.Add(projectTask, cancellationToken);

            return new CommandResult(projectTask.Id);
        }
    }
}