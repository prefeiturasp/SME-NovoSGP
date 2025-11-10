using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarFluenciaLeitora
{
    public class PainelEducacionalSalvarFluenciaLeitoraCommand : IRequest<bool>
    {
        public PainelEducacionalSalvarFluenciaLeitoraCommand(IEnumerable<PainelEducacionalRegistroFluenciaLeitoraAgrupamentoFluenciaDto> fluenciaLeitora)
        {
            FluenciaLeitora = fluenciaLeitora;
        }

        public IEnumerable<PainelEducacionalRegistroFluenciaLeitoraAgrupamentoFluenciaDto> FluenciaLeitora { get; set; }
    }

    public class PainelEducacionalSalvarFluenciaLeitoraCommandCommandValidator : AbstractValidator<PainelEducacionalSalvarFluenciaLeitoraCommand>
    {
        public PainelEducacionalSalvarFluenciaLeitoraCommandCommandValidator()
        {
            RuleFor(c => c.FluenciaLeitora)
                .Must(a => a.Any())
                .WithMessage("Informe um registro de fluencia leitora");
        }
    }
}
