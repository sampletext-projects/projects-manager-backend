using DataAccess;
using DataAccess.Models;
using DataAccess.RepositoryNew;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskStatus = DataAccess.Models.TaskStatus;

namespace BusinessLogic.Mediatr;

public static class ChangeTaskStatus
{
    public record Command(Guid UserId, Guid TaskId, TaskStatus NewStatus) : IRequest<CommandResult>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.TaskId)
                .NotEmpty()
                .WithMessage("Не выбрана задача.");

            RuleFor(x => x.NewStatus)
                .NotEqual(TaskStatus.Unknown)
                .WithMessage("Не выбран новый статус задачи");
        }
    }

    public record CommandResult();
    
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
            var canEdit = await _dbExtensions.CanEditTask(request.UserId, request.TaskId, cancellationToken);

            if (!canEdit)
            {
                throw new BusinessException("Вы не можете редактировать данный проект");
            }

            var projectTask = await _repository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == request.TaskId, cancellationToken);

            if (projectTask is null)
            {
                throw new BusinessException("Не найдена задача");
            }

            projectTask.Status = request.NewStatus;
            
            await _repository.Update(projectTask, cancellationToken);

            return new CommandResult();
        }
    }
}