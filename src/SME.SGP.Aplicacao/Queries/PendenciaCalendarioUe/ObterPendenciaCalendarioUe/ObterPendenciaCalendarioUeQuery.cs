using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaCalendarioUeQuery : IRequest<PendenciaCalendarioUe>
    {
        public ObterPendenciaCalendarioUeQuery(long tipoCalendarioId, long ueId)
        {
            TipoCalendarioId = tipoCalendarioId;
            UeId = ueId;
        }

        public long TipoCalendarioId { get; set; }
        public long UeId { get; set; }
    }

    public class ObterPendenciaCalendarioUeQueryValidator : AbstractValidator<ObterPendenciaCalendarioUeQuery>
    {
        public ObterPendenciaCalendarioUeQueryValidator()
        {
            RuleFor(c => c.TipoCalendarioId)
            .NotEmpty()
            .WithMessage("O id do tipo de calendario deve ser informado para pesquisa de pendência.");

            RuleFor(c => c.UeId)
            .NotEmpty()
            .WithMessage("O id da Ue de calendario deve ser informado para pesquisa de pendência.");
        }
    }
}
