using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarAgrupamentoEscola
{
    public class PainelEducacionalSalvarAgrupamentoGlobalEscolaCommand : IRequest<bool>
    {
        public IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoEscola> RegistroFrequencia { get; set; }

        public PainelEducacionalSalvarAgrupamentoGlobalEscolaCommand(IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoEscola> registroFrequencia)
        {
            RegistroFrequencia = registroFrequencia;
        }
    }

    public class PainelEducacionalSalvarAgrupamentoGlobalEscolaCommandValidator : AbstractValidator<PainelEducacionalSalvarAgrupamentoGlobalEscolaCommand>
    {
        public PainelEducacionalSalvarAgrupamentoGlobalEscolaCommandValidator()
        {
            RuleFor(c => c.RegistroFrequencia)
                .Must(a => a.Any())
                .WithMessage("Informe um registro de frequencia");
        }
    }
}
