using DataAccess;
using DataAccess.Models;
using DataAccess.RepositoryNew;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskStatus = DataAccess.Models.TaskStatus;

namespace BusinessLogic.Mediatr;

public static class ChangeUserRole
{
    public record Command(Guid UserId, Guid ParticipantId, Guid ProjectId, ParticipationRole Role) : IRequest<CommandResult>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.ParticipantId)
                .NotEmpty()
                .WithMessage("Не выбран участник.");

            RuleFor(x => x.Role)
                .NotEqual(ParticipationRole.Unknown)
                .WithMessage("Не выбрана новая роль");
        }
    }

    public record CommandResult();

    public class Handler : IRequestHandler<Command, CommandResult>
    {
        private readonly IRepository<Participation> _repository;

        private readonly IDbExtensions _dbExtensions;

        public Handler(IRepository<Participation> repository, IDbExtensions dbExtensions)
        {
            _repository = repository;
            _dbExtensions = dbExtensions;
        }

        public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var canEdit = await _dbExtensions.CanEditProject(request.UserId, request.ProjectId, cancellationToken);

            if (!canEdit)
            {
                throw new BusinessException("Вы не можете добавлять участников к данному проекту");
            }

            var participation = await _repository.GetAll()
                .Where(x => x.UserId == request.ParticipantId && x.ProjectId == request.ProjectId)
                .FirstOrDefaultAsync(cancellationToken);

            if (participation is null)
            {
                throw new BusinessException("Выбранный пользователь не участник данного проекта");
            }

            participation.Role = request.Role;
            await _repository.Update(participation, cancellationToken);

            return new CommandResult();
        }
    }
}