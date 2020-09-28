using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery : IRequest<IEnumerable<ObjetivoAprendizagem>>
    {
    
        public ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery(long ano, long componenteCurricularId)
        {
            Ano = ano;
            ComponenteCurricularId = componenteCurricularId;
        }

        public long Ano { get; set; }
        public long ComponenteCurricularId { get; set; }
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
