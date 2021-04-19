using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery : IRequest<IEnumerable<ObjetivoAprendizagemDto>>
    {
    
        public ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery(string[] anos, long[] juremaIds)
        {
            Anos = anos;
            JuremaIds = juremaIds;
        }

        public string[] Anos { get; set; }
        public long[] JuremaIds { get; set; }
        
    }

    public class ObterObjetivoAprendizagemPorAnoEComponenteCurricularQueryValidator : AbstractValidator<ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery>
    {
        public ObterObjetivoAprendizagemPorAnoEComponenteCurricularQueryValidator()
        {
            RuleFor(a => a.Anos)
                .NotEmpty()
                .WithMessage("Os anos precisam ser informados.");
            RuleFor(a => a.JuremaIds)
                .NotEmpty()
                .WithMessage("Os ids do jurema precisam ser informados.");
        }
    }
}
