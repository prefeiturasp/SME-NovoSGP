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
    
        public ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery(long ano, long componenteCurricularId, bool ensinoEspecial)
        {
            Ano = ano;
            ComponenteCurricularId = componenteCurricularId;
            EnsinoEspecial = ensinoEspecial;
        }

        public long Ano { get; set; }
        public long ComponenteCurricularId { get; set; }
        public bool EnsinoEspecial { get; set; }

        
    }

    public class ObterObjetivoAprendizagemPorAnoEComponenteCurricularQueryValidator : AbstractValidator<ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery>
    {
        public ObterObjetivoAprendizagemPorAnoEComponenteCurricularQueryValidator()
        {
            RuleFor(a => a.Ano)
                .NotEmpty()
                .WithMessage("O ano precisa ser informado.");
            RuleFor(a => a.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O componente curriular precisa ser informado.");
        }
    }
}
