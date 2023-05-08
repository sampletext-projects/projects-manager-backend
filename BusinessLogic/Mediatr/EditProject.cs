using DataAccess;
using DataAccess.Models;
using DataAccess.RepositoryNew;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Mediatr;

public static class EditProject
{
    public record Command(Guid UserId, Guid ProjectId, string Title, string? Description, ProjectStyle Style, Visibility Visibility) : IRequest<CommandResult>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Название не должно быть пусто");

            RuleFor(x => x.Style)
                .NotEqual(ProjectStyle.Unknown)
                .WithMessage("Не заполнено направление проекта");

            RuleFor(x => x.Visibility)
                .NotEqual(Visibility.Unknown)
                .WithMessage("Не заполнена видимость проекта");
        }
    }

    public record CommandResult();

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
            var canEdit = await _dbExtensions.CanEditProject(request.UserId, request.ProjectId, cancellationToken);

            if (canEdit)
            {
                throw new BusinessException("Вы не можете редактировать данный проект");
            }
            
            var project = await _repository.GetAll()
                .Where(x => x.Id == request.ProjectId)
                .FirstOrDefaultAsync(cancellationToken);

            if (project is null)
            {
                throw new BusinessException("Проект не найден");
            }
            
            project.Title = request.Title;
            project.Description = request.Description;
            project.Style = request.Style;
            project.Visibility = request.Visibility;

            await _repository.Update(project, cancellationToken);

            return new CommandResult();
        }
    }
}