using DataAccess;
using DataAccess.Models;
using DataAccess.RepositoryNew;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Mediatr;

public class GetProjectComments
{
    public record Command(Guid UserId, Guid ProjectId) : IRequest<CommandResult>;

    public record CommandResult(ICollection<Item> Comments);

    public record Item(Guid Id, string Content, Guid AuthorId, string AuthorUsername, DateTime CreatedAt);


    public class Handler : IRequestHandler<Command, CommandResult>
    {
        private readonly IRepository<ProjectComment> _repository;
        private readonly IDbExtensions _dbExtensions;

        public Handler(IRepository<ProjectComment> repository, IDbExtensions dbExtensions)
        {
            _dbExtensions = dbExtensions;
            _repository = repository;
        }

        public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var canView = await _dbExtensions.CanViewProject(request.UserId, request.ProjectId, cancellationToken);

            if (!canView)
            {
                throw new BusinessException("Вы не можете просматривать комментарии к данному проекту");
            }

            var items = await _repository.GetAll()
                .Where(x => x.ProjectId == request.ProjectId)
                .Select(
                    x => new Item(
                        x.Id,
                        x.Content,
                        x.AuthorId,
                        x.Author.Username ?? x.Author.Email,
                        x.CreatedAt
                    )
                )
                .ToListAsync(cancellationToken);

            return new CommandResult(items);
        }
    }
}