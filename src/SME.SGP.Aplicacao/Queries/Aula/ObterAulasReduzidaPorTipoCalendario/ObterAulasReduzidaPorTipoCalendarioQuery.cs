using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasReduzidaPorTipoCalendarioQuery : IRequest<IEnumerable<AulaReduzidaDto>>
    {
        public ObterAulasReduzidaPorTipoCalendarioQuery(long tipoCalendarioId, TipoEscola[] tiposEscola)
        {
            TipoCalendarioId = tipoCalendarioId;
            TiposEscola = tiposEscola;
        }

        public long TipoCalendarioId { get; set; }
        public TipoEscola[] TiposEscola { get; set; }
    }

    public class ObterAulasReduzidaPorTipoCalendarioQueryValidator : AbstractValidator<ObterAulasReduzidaPorTipoCalendarioQuery>
    {
        public ObterAulasReduzidaPorTipoCalendarioQueryValidator()
        {
            RuleFor(c => c.TipoCalendarioId)
                .NotEmpty()
                .WithMessage("O tipo de calendário deve ser informado.");
        }
    }
}
