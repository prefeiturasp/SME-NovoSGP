using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanoAulaPorAulaIdQuery : IRequest<PlanoAula>
    {
        public ObterPlanoAulaPorAulaIdQuery(long aulaId)
        {
            AulaId = aulaId;
        }

        public long AulaId { get; set; }
    }

    public class ObterPlanoAulaPorAulaIdQueryValidator : AbstractValidator<ObterPlanoAulaPorAulaIdQuery>
    {
        public ObterPlanoAulaPorAulaIdQueryValidator()
        {
            RuleFor(a => a.AulaId)
                .NotEmpty()
                .WithMessage("O Id da aula deve ser informado.");
        }
    }
}
