using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanosAulaEObjetivosAprendizagemQuery : IRequest<IEnumerable<PlanoAulaObjetivosAprendizagemDto>>
    {
        public ObterPlanosAulaEObjetivosAprendizagemQuery(IEnumerable<long> aulasId)
        {
            AulasId = aulasId;
        }
        public IEnumerable<long> AulasId { get; set; }
    }

    public class ObterPlanosAulaEObjetivosAprendizagemQueryValidator : AbstractValidator<ObterPlanosAulaEObjetivosAprendizagemQuery>
    {
        public ObterPlanosAulaEObjetivosAprendizagemQueryValidator()
        {
            RuleForEach(a => a.AulasId)
                .NotEmpty()
                .WithMessage("Os Ids das aulas devem ser informados.");
        }
    }
}
