using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarAgrupamentoGlobal
{
    public class PainelEducacionalSalvarAgrupamentoGlobalCommand : IRequest<bool>
    {
        public PainelEducacionalSalvarAgrupamentoGlobalCommand(IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoGlobal> registroFrequencia)
        {
            RegistroFrequencia = registroFrequencia;
        }
        public IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoGlobal> RegistroFrequencia { get; set; }
    }

    public class PainelEducacionalSalvarAgrupamentoGlobalCommandValidator : AbstractValidator<PainelEducacionalSalvarAgrupamentoGlobalCommand>
    {
        public PainelEducacionalSalvarAgrupamentoGlobalCommandValidator()
        {
            RuleFor(c => c.RegistroFrequencia)
                .Must(a => a.Any())
                .WithMessage("Informe um registro de frequencia");
        }
    }
}
