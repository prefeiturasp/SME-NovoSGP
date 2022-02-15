using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoFechamentoPorCalendarioIdQuery : IRequest<FechamentoDto>
    {
        public ObterPeriodoFechamentoPorCalendarioIdQuery(long tipoCalendarioId)
        {
            TipoCalendarioId = tipoCalendarioId;
        }

        public long TipoCalendarioId { get; }
    }

    public class ObterPeriodoFechamentoPorCalendarioIdQueryValidator : AbstractValidator<ObterPeriodoFechamentoPorCalendarioIdQuery>
    {
        public ObterPeriodoFechamentoPorCalendarioIdQueryValidator()
        {
            RuleFor(a => a.TipoCalendarioId)
                .NotEmpty()
                .WithMessage("O tipo de calendario deve ser informado para consulta de período de fechamento");
        }
    }
}
