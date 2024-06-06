using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ExcluirConsolidacoesProdutividadeFrequenciaUeAnoLetivoCommand : IRequest<bool>
    {
        public ExcluirConsolidacoesProdutividadeFrequenciaUeAnoLetivoCommand(string ueCodigo, int anoLetivo)
        {
            UeCodigo = ueCodigo;
            AnoLetivo = anoLetivo;
        }

        public int AnoLetivo { get; set; }
        public string UeCodigo { get; set; }
    }

    public class ExcluirConsolidacaoProdutividadeFrequenciaUeAnoLetivoCommandValidator : AbstractValidator<ExcluirConsolidacoesProdutividadeFrequenciaUeAnoLetivoCommand>
    {
        public ExcluirConsolidacaoProdutividadeFrequenciaUeAnoLetivoCommandValidator()
        {
            RuleFor(c => c.UeCodigo)
                .NotNull()
                .NotEmpty()
                .WithMessage("O código da Ue deve ser informado");

            RuleFor(c => c.AnoLetivo)
                .NotNull()
                .NotEmpty()
                .WithMessage("O ano letivo precisa ser informado");
        }
    }
}
