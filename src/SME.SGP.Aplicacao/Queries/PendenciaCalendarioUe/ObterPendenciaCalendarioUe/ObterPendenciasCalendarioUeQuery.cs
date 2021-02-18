using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasCalendarioUeQuery : IRequest<IEnumerable<PendenciaCalendarioUe>>
    {
        public ObterPendenciasCalendarioUeQuery(long tipoCalendarioId, long ueId, TipoPendencia tipoPendencia)
        {
            TipoCalendarioId = tipoCalendarioId;
            UeId = ueId;
            TipoPendencia = tipoPendencia;
        }

        public long TipoCalendarioId { get; set; }
        public long UeId { get; set; }
        public TipoPendencia TipoPendencia { get; set; }
    }

    public class ObterPendenciaCalendarioUeQueryValidator : AbstractValidator<ObterPendenciasCalendarioUeQuery>
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
