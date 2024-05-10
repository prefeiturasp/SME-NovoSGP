using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolarPorTipoCalendarioQuery : IRequest<IEnumerable<PeriodoEscolar>>
    {
        public ObterPeriodoEscolarPorTipoCalendarioQuery(long tipoCalendarioId)
        {
            TipoCalendarioId = tipoCalendarioId;
        }

        public long TipoCalendarioId { get; set; }
    }

    public class ObterPeriodoEscolarPorTipoCalendarioQueryValidator : AbstractValidator<ObterPeriodoEscolarPorTipoCalendarioQuery>
    {
        public ObterPeriodoEscolarPorTipoCalendarioQueryValidator()
        {
            RuleFor(x => x.TipoCalendarioId).NotEmpty().WithMessage("Informe o Tipo de Calendário para Obter o Periodo Escolar Por Tipo Calendario");
        }
    }
}