using DataAccess;
using DataAccess.Models;
using DataAccess.RepositoryNew;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Mediatr;

public static class AddUserToProject
{
    public record Command(Guid UserId, Guid InvitedUserId, Guid ProjectId, ParticipationRole Role) : IRequest<CommandResult>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty()
                .WithMessage("Не выбран проект.");

            RuleFor(x => x.InvitedUserId)
                .NotEmpty()
                .WithMessage("Не выбран пользователь.");

            RuleFor(x => x.Role)
                .NotEqual(ParticipationRole.Unknown)
                .WithMessage("Не выбрана роль.");
        }
    }

    public record CommandResult();

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
            var canEdit = await _dbExtensions.CanEditProject(request.UserId, request.ProjectId, cancellationToken);

            if (!canEdit)
            {
                throw new BusinessException("Вы не можете добавлять участников к данному проекту");
            }

            var isAlreadyMember = await _participationRepository.GetAll()
                .AnyAsync(x => x.UserId == request.InvitedUserId && x.ProjectId == request.ProjectId, cancellationToken);

            if (isAlreadyMember)
            {
                throw new BusinessException("Выбранный пользователь уже участник данного проекта");
            }

            var participation = new Participation()
            {
                ProjectId = request.ProjectId,
                UserId = request.InvitedUserId,
                Role = request.Role,
                GrantedAt = DateTime.UtcNow
            };

            await _participationRepository.Add(participation, cancellationToken);

            return new CommandResult();
        }
    }
}