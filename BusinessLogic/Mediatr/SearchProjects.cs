using DataAccess.Models;
using DataAccess.RepositoryNew;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Mediatr;

public static class SearchProjects
{
    public record Command(string Search) : IRequest<CommandResult>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Search)
                .NotEmpty()
                .WithMessage("Строка поиска не должны быть пуста");
        }
    }

    public record CommandResult(ICollection<Item> Projects);

    public record Item(string Title, string? Description);

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
                .Where(x => x.Title.StartsWith(request.Search) || (x.Description != null && x.Description.StartsWith(request.Search)))
                .Select(x => new Item(x.Title, x.Description))
                .ToListAsync(cancellationToken);

            return new CommandResult(projects);
        }
    }
}