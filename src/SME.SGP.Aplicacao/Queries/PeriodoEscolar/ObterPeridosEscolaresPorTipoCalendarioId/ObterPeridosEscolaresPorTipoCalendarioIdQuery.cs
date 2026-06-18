using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPeridosEscolaresPorTipoCalendarioIdQuery : IRequest<IEnumerable<PeriodoEscolar>>
    {
        public ObterPeridosEscolaresPorTipoCalendarioIdQuery(long tipoCalendarioId)
        {
            TipoCalendarioId = tipoCalendarioId;
        }

        public long TipoCalendarioId { get; set; }
    }

    public class ObterPeridosEscolaresPorTipoCalendarioIdQueryValidator : AbstractValidator<ObterPeridosEscolaresPorTipoCalendarioIdQuery>
    {
        public ObterPeridosEscolaresPorTipoCalendarioIdQueryValidator()
        {
            RuleFor(a => a.TipoCalendarioId)
                .NotEmpty()
                .WithMessage("O tipo de calendário deve ser informado para consulta de seus períodos escolares");
        }
    }
}
