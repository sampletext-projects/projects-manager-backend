using DataAccess.Models;
using DataAccess.RepositoryNew;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Mediatr;

public static class JoinProject
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

    public record CommandResult();

    public class Handler : IRequestHandler<Command, CommandResult>
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly IRepository<Participation> _participationRepository;

        public Handler(IRepository<Project> projectRepository, IRepository<Participation> participationRepository)
        {
            _projectRepository = projectRepository;
            _participationRepository = participationRepository;
        }

        public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var isAlreadyMember = await _participationRepository.GetAll()
                .AnyAsync(x => x.UserId == request.UserId && x.ProjectId == request.ProjectId, cancellationToken);

            if (isAlreadyMember)
            {
                throw new BusinessException("Вы уже участник данного проекта");
            }

            var projectVisibility = await _projectRepository.GetAll()
                .Where(x => x.Id == request.ProjectId)
                .Select(x => x.Visibility)
                .FirstOrDefaultAsync(cancellationToken);

            if (projectVisibility != Visibility.Visible)
            {
                throw new BusinessException("Вы не можете присоединиться к данному проекту");
            }

            var participation = new Participation()
            {
                ProjectId = request.ProjectId,
                UserId = request.UserId,
                Role = ParticipationRole.Readonly
            };

            await _participationRepository.Add(participation, cancellationToken);

            return new CommandResult();
        }
    }
}