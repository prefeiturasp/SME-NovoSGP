using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterBimestreDoPeriodoEscolarQuery : IRequest<int>
    {
        public ObterBimestreDoPeriodoEscolarQuery(long periodoEscolarId)
        {
            PeriodoEscolarId = periodoEscolarId;
        }

        public long PeriodoEscolarId { get; }
    }

    public class ObterBimestreDoPeriodoEscolarQueryValidator : AbstractValidator<ObterBimestreDoPeriodoEscolarQuery>
    {
        public ObterBimestreDoPeriodoEscolarQueryValidator()
        {
            RuleFor(a => a.PeriodoEscolarId)
                .NotEmpty()
                .WithMessage("O identificador do período escolar deve ser informado para consulta do bimestre");
        }
    }
}
