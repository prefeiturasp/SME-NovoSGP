using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodosEscolaresPorTipoCalendarioQuery: IRequest<IEnumerable<PeriodoEscolar>>
    {
        public ObterPeriodosEscolaresPorTipoCalendarioQuery(long tipoCalendarioId)
        {
            TipoCalendarioId = tipoCalendarioId;
        }

        public long TipoCalendarioId { get; set; }
    }

    public class ObterPeriodosEscolaresPorTipoCalendarioQueryValidator: AbstractValidator<ObterPeriodosEscolaresPorTipoCalendarioQuery>
    {
        public ObterPeriodosEscolaresPorTipoCalendarioQueryValidator()
        {
            RuleFor(a => a.TipoCalendarioId)
                .NotEmpty()
                .WithMessage("O tipo de calendário deve ser informado para localizar seus períodos escolares.");
        }
    }
}
