using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class PersistirRegistroFrequenciaCommand : IRequest<long>
    {
        public PersistirRegistroFrequenciaCommand(RegistroFrequencia registroFrequencia)
        {
            RegistroFrequencia = registroFrequencia;
        }

        public RegistroFrequencia RegistroFrequencia { get; set; }
    }

    public class SalvarFrequenciaCommandValidator : AbstractValidator<PersistirRegistroFrequenciaCommand>
    {
        public SalvarFrequenciaCommandValidator()
        {
            RuleFor(x => x.RegistroFrequencia)
               .NotEmpty()
               .WithMessage("O id da aula deve ser informado.");
        }
    }
}
