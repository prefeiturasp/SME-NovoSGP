using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterObjetivosAprendizagemAulaPorPlanoAulaIdQuery : IRequest<IEnumerable<ObjetivoAprendizagemAula>>
    {
        public ObterObjetivosAprendizagemAulaPorPlanoAulaIdQuery(long planoAulaId)
        {
            PlanoAulaId = planoAulaId;
        }

        public long PlanoAulaId { get; set; }
    }

    public class ObterObjetivosAprendizagemAulaPorPlanoAulaIdQueryValidator : AbstractValidator<ObterObjetivosAprendizagemAulaPorPlanoAulaIdQuery>
    {
        public ObterObjetivosAprendizagemAulaPorPlanoAulaIdQueryValidator()
        {
            RuleFor(c => c.PlanoAulaId)
            .NotEmpty()
            .WithMessage("O id do plano aula deve ser informado para consulta de objetivos da aprendizagem da aula.");

        }
    }
}
