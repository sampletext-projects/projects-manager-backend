using DataAccess;
using DataAccess.Models;
using DataAccess.RepositoryNew;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskStatus = DataAccess.Models.TaskStatus;

namespace BusinessLogic.Mediatr;

public static class GetTaskById
{
    public record Command(Guid UserId, Guid TaskId) : IRequest<CommandResult>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.TaskId)
                .NotEmpty()
                .WithMessage("Не выбрана задача");
        }
    }

    public record CommandResult(string Title, string? Description, TaskStatus Status);
    
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
            var canView = await _dbExtensions.CanViewTask(request.UserId, request.TaskId, cancellationToken);
            
            if (!canView)
            {
                throw new BusinessException("Вы не можете просматривать данную задачу");
            }

            var task = await _repository.GetAll()
                .Where(x => x.Id == request.TaskId)
                .Select(
                    x => new CommandResult(
                        x.Title,
                        x.Description,
                        x.Status
                    )
                )
                .FirstOrDefaultAsync(cancellationToken);

            if (task is null)
            {
                throw new BusinessException("Задача не найден");
            }
            
            return task;
        }
    }
}