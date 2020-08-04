using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodosEscolaresPorTipoCalendarioIdQuery : IRequest<IEnumerable<Dominio.PeriodoEscolar>>
    {
        public ObterPeriodosEscolaresPorTipoCalendarioIdQuery(long tipoCalendarioId)
        {
            TipoCalendarioId = tipoCalendarioId;
        }

        public long TipoCalendarioId { get; set; }
    }

    public class ObterPorTipoCalendarioIdQueryValidator : AbstractValidator<ObterPeriodosEscolaresPorTipoCalendarioIdQuery>
    {
        public ObterPorTipoCalendarioIdQueryValidator()
        {

            RuleFor(c => c.TipoCalendarioId)
                .NotEmpty()
                .WithMessage("O tipo de calendário deve ser informado.");
        }
    }
}
