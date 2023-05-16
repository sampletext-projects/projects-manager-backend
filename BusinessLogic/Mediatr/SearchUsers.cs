using DataAccess.Models;
using DataAccess.RepositoryNew;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Mediatr;

public static class SearchUsers
{
    
    public record Command(Guid UserId, string Query) : IRequest<CommandResult>;

    public record CommandResult(ICollection<Item> Users);

    public record Item(Guid Id, string Email, string? Username);


    public class Handler : IRequestHandler<Command, CommandResult>
    {
        private readonly IRepository<AppUser> _repository;

        public Handler(IRepository<AppUser> repository)
        {
            _repository = repository;
        }

        public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var items = await _repository.GetAll()
                .Where(x => x.Email.StartsWith(request.Query) || x.Username != null && x.Username.StartsWith(request.Query))
                .Where(x => x.Id != request.UserId)
                .Select(x => new Item(x.Id, x.Email, x.Username))
                .ToListAsync(cancellationToken);

            return new CommandResult(items);
        }
    }
}