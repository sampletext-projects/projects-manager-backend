using BusinessLogic.Jwt;
using DataAccess.Models;
using DataAccess.RepositoryNew;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Mediatr;

public class RefreshToken
{
    public record Command(Guid UserId) : IRequest<Response>;

    public record Response(string Token, string RefreshToken);

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
            var jwtGenerationResult = _jwtGeneratorService.GenerateToken(request.UserId);

            return new Response(jwtGenerationResult.Token, jwtGenerationResult.RefreshToken);
        }
    }
}