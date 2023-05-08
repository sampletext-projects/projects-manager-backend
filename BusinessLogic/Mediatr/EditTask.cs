using DataAccess;
using DataAccess.Models;
using DataAccess.RepositoryNew;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskStatus = System.Threading.Tasks.TaskStatus;

namespace BusinessLogic.Mediatr;

public static class EditTask
{
    public record Command(Guid UserId, Guid TaskId, string Title, string? Description) : IRequest<CommandResult>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Название не должно быть пусто");

            RuleFor(x => x.TaskId)
                .NotEmpty()
                .WithMessage("Не выбран задача.");

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Не заполнен заголовок");
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
                throw new BusinessException("Вы не можете редактировать данную задачу");
            }

            var task = await _repository.GetAll()
                .Where(x => x.Id == request.TaskId)
                .FirstOrDefaultAsync(cancellationToken);

            if (task is null)
            {
                throw new BusinessException("Задача не найдена");
            }
            
            task.Title = request.Title;
            task.Description = request.Description;

            await _repository.Update(task, cancellationToken);

            return new CommandResult();
        }
    }
}