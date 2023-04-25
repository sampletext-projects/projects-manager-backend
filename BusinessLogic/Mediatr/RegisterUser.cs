using DataAccess.Models;
using DataAccess.RepositoryNew;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Mediatr;

public static class RegisterUser
{
    public record Command(string Email, string Password) : IRequest<Unit>;

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .Length(8, 50);
        }
    }

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IRepository<AppUser> _repository;

        public Handler(IRepository<AppUser> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            if (await _repository.GetAll()
                    .AnyAsync(x => x.Email == request.Email, cancellationToken))
            {
                throw new BusinessException("Email уже занят");
            }

            var appUser = new AppUser()
            {
                Email = request.Email,
                Password = request.Password,
                Username = ""
            };

            await _repository.Add(appUser, cancellationToken);
            
            return Unit.Value;
        }
    }
}