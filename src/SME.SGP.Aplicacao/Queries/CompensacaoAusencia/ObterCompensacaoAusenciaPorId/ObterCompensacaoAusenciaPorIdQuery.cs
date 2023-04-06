using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterCompensacaoAusenciaPorIdQuery : IRequest<Dominio.CompensacaoAusencia>
    {
        public ObterCompensacaoAusenciaPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }

    public class ObterCompensacaoAusenciaPorIdQueryValidator: AbstractValidator<ObterCompensacaoAusenciaPorIdQuery>
    {
        public ObterCompensacaoAusenciaPorIdQueryValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .WithMessage("O Id da compensação de ausência deve ser informado.");
        }
    }
}
