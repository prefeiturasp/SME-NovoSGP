using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasAulaPorAulaIdTipoQuery : IRequest<IEnumerable<long>>
    {
        public ObterPendenciasAulaPorAulaIdTipoQuery(long aulaId, TipoPendencia tipoPendencia)
        {
            AulaId = aulaId;
            TipoPendencia = tipoPendencia;
        }

        public long AulaId { get; set; }
        public TipoPendencia TipoPendencia { get; set; }
    }

    public class ObterPendenciasAulaPorAulaIdTipoQueryValidator : AbstractValidator<ObterPendenciasAulaPorAulaIdTipoQuery>
    {
        public ObterPendenciasAulaPorAulaIdTipoQueryValidator()
        {
            RuleFor(c => c.AulaId)
               .NotEmpty()
               .WithMessage("O Id aula deve ser informado.");

            RuleFor(c => c.TipoPendencia)
            .NotEmpty()
            .WithMessage("O tipo de pendência deve ser informado para geração de pendência.");
        }
    }
}
