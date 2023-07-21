using MediatR;
using SME.SGP.Dominio.Enumerados;
using System;
using FluentValidation;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarConsolidacaoDashBoardFrequenciaCommand : IRequest<bool>
    {
        public SalvarConsolidacaoDashBoardFrequenciaCommand(ConsolidacaoDashBoardFrequencia consolidacaoDashBoardFrequencia)
        {
            ConsolidacaoDashBoardFrequencia = consolidacaoDashBoardFrequencia;
        }

        public ConsolidacaoDashBoardFrequencia ConsolidacaoDashBoardFrequencia { get; set; }
    }
    
    public class SalvarConsolidacaoDashBoardFrequenciaCommandValidator : AbstractValidator<SalvarConsolidacaoDashBoardFrequenciaCommand>
    {
        public SalvarConsolidacaoDashBoardFrequenciaCommandValidator()
        {
            RuleFor(a => a.ConsolidacaoDashBoardFrequencia)
                .NotEmpty()
                .WithMessage("A consolidação deve ser informada para a persistência da consolidação diária do dashboard frequência");
        }
    }
}
