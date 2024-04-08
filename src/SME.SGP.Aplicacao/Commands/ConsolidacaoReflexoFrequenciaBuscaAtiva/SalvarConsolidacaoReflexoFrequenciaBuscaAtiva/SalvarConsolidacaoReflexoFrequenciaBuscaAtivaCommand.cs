using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class SalvarConsolidacaoReflexoFrequenciaBuscaAtivaCommand : IRequest<long>
    {
        public SalvarConsolidacaoReflexoFrequenciaBuscaAtivaCommand(ConsolidacaoReflexoFrequenciaBuscaAtivaAluno consolidacaoReflexoFrequencia)
        {
            this.ConsolidacaoReflexoFrequencia = consolidacaoReflexoFrequencia ?? throw new ArgumentNullException(nameof(consolidacaoReflexoFrequencia));
        }

        public ConsolidacaoReflexoFrequenciaBuscaAtivaAluno ConsolidacaoReflexoFrequencia { get; set; }
    }

    public class SalvarConsolidacaoReflexoFrequenciaBuscaAtivaCommandValidator : AbstractValidator<SalvarConsolidacaoReflexoFrequenciaBuscaAtivaCommand>
    {
        public SalvarConsolidacaoReflexoFrequenciaBuscaAtivaCommandValidator()
        {
            RuleFor(c => c.ConsolidacaoReflexoFrequencia)
               .NotNull()
               .WithMessage("O consolidado do reflexo de frequência de busca ativa deve ser informado para efetuar a gravação.");
        }
    }
}
