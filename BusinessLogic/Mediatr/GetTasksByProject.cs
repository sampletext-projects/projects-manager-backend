using DataAccess;
using DataAccess.Models;
using DataAccess.RepositoryNew;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskStatus = DataAccess.Models.TaskStatus;

namespace BusinessLogic.Mediatr;

public static class GetTasksByProject
{
    public record Command(Guid UserId, Guid ProjectId) : IRequest<CommandResult>;


    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty()
                .WithMessage("Не выбран проект.");
        }
    }

    public record CommandResult(ICollection<Item> Tasks);

    public record Item(Guid Id, string Title, string? Description, TaskStatus Status);

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
            var canView = await _dbExtensions.CanViewProject(request.UserId, request.ProjectId, cancellationToken);

            if (!canView)
            {
                throw new BusinessException("Вы не можете просматривать задачи данного проекта");
            }

            var items = await _repository.GetAll()
                .Where(x => x.ProjectId == request.ProjectId)
                .Select(x => new Item(x.Id, x.Title, x.Description, x.Status))
                .ToListAsync(cancellationToken);

            return new CommandResult(items);
        }
    }
}