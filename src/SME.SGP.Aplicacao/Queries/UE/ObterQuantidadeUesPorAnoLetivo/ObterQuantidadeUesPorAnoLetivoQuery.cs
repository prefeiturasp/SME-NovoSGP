using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeUesPorAnoLetivoQuery : IRequest<int>
    {
        public ObterQuantidadeUesPorAnoLetivoQuery(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }

        public int AnoLetivo { get; set; }
    }

    public class ObterQuantidadeUesPorAnoLetivoQueryValidator : AbstractValidator<ObterQuantidadeUesPorAnoLetivoQuery>
    {
        public ObterQuantidadeUesPorAnoLetivoQueryValidator()
        {
            RuleFor(c => c.AnoLetivo)
               .Must(a => a > 0)
               .WithMessage("O ano letivo deve ser informado para consulta de quantidade de UEs.");

        }
    }
}
