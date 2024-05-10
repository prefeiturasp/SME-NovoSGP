using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class VerificaPossuiObjetivosAprendizagemPorComponenteCurricularIdQuery : IRequest<bool>
    {
    
        public VerificaPossuiObjetivosAprendizagemPorComponenteCurricularIdQuery(long componenteCurricularId)
        {
            ComponenteCurricularId = componenteCurricularId;
        }

        public long ComponenteCurricularId { get; set; }
        
    }

    public class VerificaPossuiObjetivosAprendizagemPorComponenteCurricularIdQueryValidator : AbstractValidator<VerificaPossuiObjetivosAprendizagemPorComponenteCurricularIdQuery>
    {
        public VerificaPossuiObjetivosAprendizagemPorComponenteCurricularIdQueryValidator()
        {
            RuleFor(a => a.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O ID do componente curricular precisa ser informado.");
        }
    }
}
