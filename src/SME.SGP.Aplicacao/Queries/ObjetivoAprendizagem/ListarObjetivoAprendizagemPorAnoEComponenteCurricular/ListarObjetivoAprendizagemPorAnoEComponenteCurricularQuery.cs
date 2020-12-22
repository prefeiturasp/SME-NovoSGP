using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery : IRequest<IEnumerable<ObjetivoAprendizagemDto>>
    {
    
        public ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery(string ano, long[] juremaIds)
        {
            Ano = ano;
            JuremaIds = juremaIds;
        }

        public string Ano { get; set; }
        public long[] JuremaIds { get; set; }
        
    }

    public class ObterObjetivoAprendizagemPorAnoEComponenteCurricularQueryValidator : AbstractValidator<ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery>
    {
        public ObterObjetivoAprendizagemPorAnoEComponenteCurricularQueryValidator()
        {
            RuleFor(a => a.Ano)
                .NotEmpty()
                .WithMessage("O ano precisa ser informado.");
            RuleFor(a => a.JuremaIds)
                .NotEmpty()
                .WithMessage("Os ids do jurema precisam ser informados.");
        }
    }
}
