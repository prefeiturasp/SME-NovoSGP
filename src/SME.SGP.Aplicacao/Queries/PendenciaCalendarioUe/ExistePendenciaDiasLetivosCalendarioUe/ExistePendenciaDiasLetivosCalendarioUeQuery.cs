using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExistePendenciaDiasLetivosCalendarioUeQuery : IRequest<bool>
    {
        public ExistePendenciaDiasLetivosCalendarioUeQuery(long tipoCalendarioId, long ueId)
        {
            TipoCalendarioId = tipoCalendarioId;
            UeId = ueId;
        }

        public long TipoCalendarioId { get; set; }
        public long UeId { get; set; }
    }

    public class ExistePendenciaDiasLetivosCalendarioUeQueryValidator : AbstractValidator<ExistePendenciaDiasLetivosCalendarioUeQuery>
    {
        public ExistePendenciaDiasLetivosCalendarioUeQueryValidator()
        {
            RuleFor(c => c.TipoCalendarioId)
            .Must(a => a > 0)
            .WithMessage("O id do tipo de calendário deve ser informado para verificar a existência de pendências.");

            RuleFor(c => c.UeId)
            .Must(a => a > 0)
            .WithMessage("O id do tipo de calendário deve ser informado para verificar a existência de pendências.");
        }
    }
}
