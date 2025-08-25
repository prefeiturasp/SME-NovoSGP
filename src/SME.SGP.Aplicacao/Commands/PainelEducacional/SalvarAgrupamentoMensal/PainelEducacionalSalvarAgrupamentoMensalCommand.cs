using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarAgrupamentoMensal
{
    public class PainelEducacionalSalvarAgrupamentoMensalCommand : IRequest<bool>
    {
        public PainelEducacionalSalvarAgrupamentoMensalCommand(IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoMensal> registroFrequencia)
        {
            RegistroFrequencia = registroFrequencia;
        }

        public IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoMensal> RegistroFrequencia { get; set; }
    }

    public class PainelEducacionalSalvarAgrupamentoMensalCommandValidator : AbstractValidator<PainelEducacionalSalvarAgrupamentoMensalCommand>
    {
        public PainelEducacionalSalvarAgrupamentoMensalCommandValidator()
        {
            RuleFor(c => c.RegistroFrequencia)
                .Must(a => a.Any())
                .WithMessage("Informe um registro de frequencia");
        }
    }
}
