using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaParametroEventoPorCalendarioUeParametroQuery : IRequest<PendenciaParametroEvento>
    {
        public ObterPendenciaParametroEventoPorCalendarioUeParametroQuery(long tipoCalendarioId, long ueId, long parametroSistemaId)
        {
            TipoCalendarioId = tipoCalendarioId;
            UeId = ueId;
            ParametroSistemaId = parametroSistemaId;
        }

        public long TipoCalendarioId { get; set; }
        public long UeId { get; set; }
        public long ParametroSistemaId { get; set; }
    }

    public class ObterPendenciaParametroEventoPorCalendarioUeParametroQueryValidator : AbstractValidator<ObterPendenciaParametroEventoPorCalendarioUeParametroQuery>
    {
        public ObterPendenciaParametroEventoPorCalendarioUeParametroQueryValidator()
        {
            RuleFor(c => c.TipoCalendarioId)
               .Must(a => a > 0)
               .WithMessage("O id do tipo de calendário deve ser informado para consulta de pendência de eventos.");

            RuleFor(c => c.UeId)
               .Must(a => a > 0)
               .WithMessage("O id da UE deve ser informado para consulta de pendência de eventos.");

            RuleFor(c => c.ParametroSistemaId)
               .Must(a => a > 0)
               .WithMessage("O id do parametro do sistema deve ser informado para consulta de pendência de eventos.");
        }
    }
}
