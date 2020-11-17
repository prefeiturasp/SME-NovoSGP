using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasParametroEventoPorPendenciaCalendarioUeQuery : IRequest<IEnumerable<PendenciaParametroEvento>>
    {
        public ObterPendenciasParametroEventoPorPendenciaCalendarioUeQuery(long pendenciaCalendarioUeId)
        {
            PendenciaCalendarioUeId = pendenciaCalendarioUeId;
        }

        public long PendenciaCalendarioUeId { get; set; }
    }

    public class ObterPendenciasParametroEventoPorPendenciaCalendarioUeQueryValidator : AbstractValidator<ObterPendenciasParametroEventoPorPendenciaCalendarioUeQuery>
    {
        public ObterPendenciasParametroEventoPorPendenciaCalendarioUeQueryValidator()
        {
            RuleFor(c => c.PendenciaCalendarioUeId)
               .NotEmpty()
               .WithMessage("O id da pendência calendario ser informado para consulta de pendências parâmetros.");
        }
    }
}
