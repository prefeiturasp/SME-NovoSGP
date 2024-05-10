using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigoComponentePaiQuery : IRequest<string>
    {
        public long ComponenteCurricularId { get; set; }

        public ObterCodigoComponentePaiQuery(long componenteCurricularId)
        {
            ComponenteCurricularId = componenteCurricularId;
        }
    }

    public class ObterCodigoComponentePaiQueryValidator : AbstractValidator<ObterCodigoComponentePaiQuery>
    {
        public ObterCodigoComponentePaiQueryValidator()
        {
            RuleFor(a => a.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("É necessário informar o código do componente curricular para buscar o código do componente pai");
        }
    }
}
