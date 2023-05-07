using DataAccess;
using DataAccess.Models;
using DataAccess.RepositoryNew;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Mediatr;

public static class GetProjectParticipants
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

    public record CommandResult(ICollection<Item> Participants);

    public record Item(Guid Id, string Email, string? Username, ParticipationRole Role);

    public class Handler : IRequestHandler<Command, CommandResult>
    {
        private readonly IRepository<Participation> _participationRepository;
        private readonly IDbExtensions _dbExtensions;

        public Handler(IRepository<Participation> participationRepository, IDbExtensions dbExtensions)
        {
            _participationRepository = participationRepository;
            _dbExtensions = dbExtensions;
        }

        public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var canView = await _dbExtensions.CanView(request.UserId, request.ProjectId, cancellationToken);

            if (!canView)
            {
                throw new BusinessException("Вы не можете просматривать участников данного проекта");
            }

            var items = await _participationRepository.GetAll()
                .Where(x => x.ProjectId == request.ProjectId)
                .Select(
                    x => new Item(
                        x.User.Id,
                        x.User.Email,
                        x.User.Username,
                        x.Role
                    )
                )
                .ToListAsync(cancellationToken);

            return new CommandResult(items);
        }
    }
}