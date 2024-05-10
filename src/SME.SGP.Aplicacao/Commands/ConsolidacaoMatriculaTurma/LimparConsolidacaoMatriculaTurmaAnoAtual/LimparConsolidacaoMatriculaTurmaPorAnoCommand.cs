using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class LimparConsolidacaoMatriculaTurmaPorAnoCommand : IRequest<bool>
    {
        public LimparConsolidacaoMatriculaTurmaPorAnoCommand(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }

        public int AnoLetivo { get; set; }
    }

    public class LimparConsolidacaoMatriculaTurmaPorAnoCommandValidator : AbstractValidator<LimparConsolidacaoMatriculaTurmaPorAnoCommand>
    {
        public LimparConsolidacaoMatriculaTurmaPorAnoCommandValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para limpar a consolidação de matrículas das turmas");
        }
    }
}
