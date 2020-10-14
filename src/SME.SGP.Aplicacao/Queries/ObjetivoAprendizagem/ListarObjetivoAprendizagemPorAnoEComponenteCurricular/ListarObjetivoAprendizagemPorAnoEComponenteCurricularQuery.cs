using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery : IRequest<IEnumerable<ObjetivoAprendizagemDto>>
    {
    
        public ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery(long ano, long[] juremaIds)
        {
            Ano = ano;
            JuremaIds = juremaIds;
        }

        public long Ano { get; set; }
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
