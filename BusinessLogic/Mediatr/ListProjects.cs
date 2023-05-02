using DataAccess.Models;
using DataAccess.RepositoryNew;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Mediatr;

public static class ListProjects
{
    public record Command(Guid UserId) : IRequest<CommandResult>;

    public record CommandResult(ICollection<Item> Projects);

    public record Item(string Title, string Description);
    
    public class Handler : IRequestHandler<Command, CommandResult>
    {
        private readonly IRepository<Project> _repository;

        public Handler(IRepository<Project> repository)
        {
            _repository = repository;
        }

        public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var projects = await _repository.GetAll()
                .Where(x => x.CreatorId != request.UserId)
                .Select(x => new Item(x.Title, x.Description))
                .ToListAsync(cancellationToken);

            return new CommandResult(projects);
        }
    }
}