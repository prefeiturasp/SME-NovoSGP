using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanoAEEComTurmaUeEDrePorIdQuery : IRequest<PlanoAEE>
    {
        public ObterPlanoAEEComTurmaUeEDrePorIdQuery(long planoId)
        {
            PlanoId = planoId;
        }

        public long PlanoId { get; }
    }

    public class ObterPlanoAEEComTurmaUeEDrePorIdQueryValidator : AbstractValidator<ObterPlanoAEEComTurmaUeEDrePorIdQuery>
    {
        public ObterPlanoAEEComTurmaUeEDrePorIdQueryValidator()
        {
            RuleFor(a => a.PlanoId)
                .NotEmpty()
                .WithMessage("O id do plano AEE deve ser informado para consulta de seus dados");
        }
    }
}
