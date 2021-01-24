using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaAulaPorAulaIdQuery : IRequest<long>
    {
        public ObterPendenciaAulaPorAulaIdQuery(long aulaId, TipoPendencia tipoPendencia)
        {
            AulaId = aulaId;
            TipoPendencia = tipoPendencia;
        }

        public long AulaId { get; set; }
        public TipoPendencia TipoPendencia { get; set; }
    }

    public class ObterPendenciaAulaPorAulaIdQueryValidator : AbstractValidator<ObterPendenciaAulaPorAulaIdQuery>
    {
        public ObterPendenciaAulaPorAulaIdQueryValidator()
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
