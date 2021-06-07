using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class InserirRegistroFrequenciaCommand : IRequest<long>
    {
        public InserirRegistroFrequenciaCommand(RegistroFrequencia registroFrequencia)
        {
            RegistroFrequencia = registroFrequencia;
        }

        public RegistroFrequencia RegistroFrequencia { get; set; }
    }

    public class SalvarFrequenciaCommandValidator : AbstractValidator<InserirRegistroFrequenciaCommand>
    {
        public SalvarFrequenciaCommandValidator()
        {
            RuleFor(x => x.RegistroFrequencia)
               .NotEmpty()
               .WithMessage("O id da aula deve ser informado.");
        }
    }
}
