using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class SalvarConsolidacaoProdutividadeFrequenciaCommand : IRequest<long>
    {
        public SalvarConsolidacaoProdutividadeFrequenciaCommand(ConsolidacaoProdutividadeFrequencia consolidacao)
        {
            this.Consolidacao = consolidacao ?? throw new ArgumentNullException(nameof(consolidacao));
        }

        public ConsolidacaoProdutividadeFrequencia Consolidacao { get; set; }
    }

    public class SalvarConsolidacaoProdutividadeFrequenciaCommandValidator : AbstractValidator<SalvarConsolidacaoProdutividadeFrequenciaCommand>
    {
        public SalvarConsolidacaoProdutividadeFrequenciaCommandValidator()
        {
            RuleFor(c => c.Consolidacao)
               .NotNull()
               .WithMessage("O consolidado da produtividade de frequência deve ser informado para efetuar a gravação.");
        }
    }
}
