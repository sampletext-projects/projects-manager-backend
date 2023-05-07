using DataAccess.Models;
using DataAccess.RepositoryNew;
using FluentValidation;
using MediatR;

namespace BusinessLogic.Mediatr;

public static class CreateProject
{
    public record Command(Guid UserId, string Title, string? Description, ProjectStyle Style, Visibility Visibility) : IRequest<CommandResult>;

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

    public record CommandResult(Guid ProjectId);
    
    public class Handler : IRequestHandler<Command, CommandResult>
    {
        private readonly IRepository<Project> _repository;

        public Handler(IRepository<Project> repository)
        {
            _repository = repository;
        }

        public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var project = new Project()
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                Style = request.Style,
                Visibility = request.Visibility,
                Participations = new List<Participation>()
                {
                    new()
                    {
                        UserId = request.UserId,
                        Role = ParticipationRole.Creator
                    }
                }
            };

            await _repository.Add(project, cancellationToken);

            return new CommandResult(project.Id);
        }
    }
}