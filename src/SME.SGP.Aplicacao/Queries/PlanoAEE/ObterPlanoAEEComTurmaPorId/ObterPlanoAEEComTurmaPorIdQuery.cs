using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanoAEEComTurmaPorIdQuery : IRequest<PlanoAEE>
    {
        public ObterPlanoAEEComTurmaPorIdQuery(long planoAEEId)
        {
            PlanoAEEId = planoAEEId;
        }

        public long PlanoAEEId { get; }
    }

    public class ObterPlanoAEEComTurmaPorIdQueryValidator : AbstractValidator<ObterPlanoAEEComTurmaPorIdQuery>
    {
        public ObterPlanoAEEComTurmaPorIdQueryValidator()
        {
            RuleFor(a => a.PlanoAEEId)
                .NotEmpty()
                .WithMessage("O id do plano AEE deve ser informado para consulta");
        }
    }
}
