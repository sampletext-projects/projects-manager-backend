using DataAccess;
using DataAccess.Models;
using DataAccess.RepositoryNew;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Mediatr;

public static class GetProjectById
{
    public record Command(Guid UserId, Guid ProjectId) : IRequest<CommandResult>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty()
                .WithMessage("Не выбран проект");
        }
    }

    public record CommandResult(string Title, string? Description, ProjectStyle Style, Visibility Visibility);
    
    public class Handler : IRequestHandler<Command, CommandResult>
    {
        private readonly IRepository<Project> _repository;
        private readonly IDbExtensions _dbExtensions;

        public Handler(IRepository<Project> repository, IDbExtensions dbExtensions)
        {
            _repository = repository;
            _dbExtensions = dbExtensions;
        }

        public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var canView = await _dbExtensions.CanViewProject(request.UserId, request.ProjectId, cancellationToken);
            
            if (!canView)
            {
                throw new BusinessException("Вы не можете просматривать данный проект");
            }

            var project = await _repository.GetAll()
                .Where(x => x.Id == request.ProjectId)
                .Select(
                    x => new CommandResult(
                        x.Title,
                        x.Description,
                        x.Style,
                        x.Visibility
                    )
                )
                .FirstOrDefaultAsync(cancellationToken);

            if (project is null)
            {
                throw new BusinessException("Проект не найден");
            }
            
            return project;
        }
    }
}