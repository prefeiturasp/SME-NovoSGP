using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExisteInformePorIdQuery : IRequest<bool>
    {
        public ExisteInformePorIdQuery(long id)
        {
            Id = id;
        }
        public long Id { get; }
    }

    public class ExisteInformePorIdQueryValidator : AbstractValidator<ExisteInformePorIdQuery>
    {
        public ExisteInformePorIdQueryValidator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0)
                .WithMessage("O Id do infomes deve ser informado para a pesquisar existência.");
        }
    }
}
