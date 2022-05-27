using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ProcessarPendenciaDevolutivaQuery: IRequest<bool>
    {
        public int AnoLetivo { get; }

        public ProcessarPendenciaDevolutivaQuery(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }

        public class ProcessarPendenciaDevolutivaQueryValidator : AbstractValidator<ProcessarPendenciaDevolutivaQuery>
        {
            public ProcessarPendenciaDevolutivaQueryValidator()
            {
                RuleFor(a => a.AnoLetivo)
                        .NotEmpty()
                        .WithMessage("O ano letivo deve ser informado para consulta de turmas na modalidade.");
            }
        }
    }
}
