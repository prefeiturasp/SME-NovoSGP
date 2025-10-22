using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarReclassificacaoAnual
{
    public class PainelEducacionalSalvarReclassificacaoAnualCommand : IRequest<bool>
    {
        public PainelEducacionalSalvarReclassificacaoAnualCommand(IEnumerable<PainelEducacionalReclassificacao> reclassificacaoAnual)
        {
            ReclassificacaoAnual = reclassificacaoAnual;
        }

        public IEnumerable<PainelEducacionalReclassificacao> ReclassificacaoAnual { get; set; }
    }

    public class PainelEducacionalSalvarReclassificacaoAnualCommandValidator : AbstractValidator<PainelEducacionalSalvarReclassificacaoAnualCommand>
    {
        public PainelEducacionalSalvarReclassificacaoAnualCommandValidator()
        {
            RuleFor(c => c.ReclassificacaoAnual)
                .Must(a => a.Any())
                .WithMessage("Informe uma reclassificação");
        }
    }
}
