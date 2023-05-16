using DataAccess.Models;
using DataAccess.RepositoryNew;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Mediatr;

public static class ExploreProjects
{
    public record Command(Guid UserId) : IRequest<CommandResult>;

    public record CommandResult(ICollection<Item> Projects);

    public record Item(Guid Id, string Title, string? Description);
    
    public class Handler : IRequestHandler<Command, CommandResult>
    {
        private readonly IRepository<Project> _repository;

        public Handler(IRepository<Project> repository)
        {
            _repository = repository;
        }

        public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
        {
            // ищем открытые проекты, созданные другими пользователями
            var projects = await _repository.GetAll()
                .Where(x => x.Visibility == Visibility.Visible && x.Participations != null && x.Participations.Any(y => y.Role == ParticipationRole.Creator && y.UserId != request.UserId))
                .Select(x => new Item(x.Id, x.Title, x.Description))
                .ToListAsync(cancellationToken);

            return new CommandResult(projects);
        }
    }
}