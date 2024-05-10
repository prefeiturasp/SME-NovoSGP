using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanoAulaEObjetivosAprendizagemQuery : IRequest<PlanoAulaObjetivosAprendizagemDto>
    {
        public ObterPlanoAulaEObjetivosAprendizagemQuery(long aulaId)
        {
            AulaId = aulaId;
        }
        public long AulaId { get; set; }
    }

    public class ObterPlanoAulaEObjetivosAprendizagemQueryValidator : AbstractValidator<ObterPlanoAulaEObjetivosAprendizagemQuery>
    {
        public ObterPlanoAulaEObjetivosAprendizagemQueryValidator()
        {
            RuleFor(a => a.AulaId)
                .NotEmpty()
                .WithMessage("O Id da aula deve ser informado.");
        }
    }
}
