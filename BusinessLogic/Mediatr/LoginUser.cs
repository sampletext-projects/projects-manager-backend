using BusinessLogic.Jwt;
using DataAccess.Models;
using DataAccess.RepositoryNew;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Mediatr;

public static class LoginUser
{
    public record Command(string Email, string Password) : IRequest<Response>;

    public record Response(string Token, string RefreshToken);
    
    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("E-mail не должен быть пустым")
                .EmailAddress()
                .WithMessage("E-mail должен быть email адресом");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Пароль не должен быть пустым")
                .Length(8, 50)
                .WithMessage("Длина пароля должна быть от 8 до 50 символов");
        }
    }
    
    public class Handler : IRequestHandler<Command, Response>
    {
        private readonly IRepository<AppUser> _repository;
        private readonly IJwtGeneratorService _jwtGeneratorService;

        public Handler(IRepository<AppUser> repository, IJwtGeneratorService jwtGeneratorService)
        {
            _repository = repository;
            _jwtGeneratorService = jwtGeneratorService;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetAll()
                .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

            if (user is null)
            {
                throw new BusinessException("Пользователь не найден");
            }

            if (user.Password != request.Password)
            {
                throw new BusinessException("Неверный пароль");
            }

            var jwtGenerationResult = _jwtGeneratorService.GenerateToken(user.Id);

            return new Response(jwtGenerationResult.Token, jwtGenerationResult.RefreshToken);
        }
    }
}