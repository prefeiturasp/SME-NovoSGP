using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class AlterarPendenciaCommand : IRequest<long>
    {
        public AlterarPendenciaCommand(Pendencia pendencia)
        {
            Pendencia = pendencia;
        }

        public Pendencia Pendencia { get; set; }
    }

    public class AlterarPendenciaCommandValidator : AbstractValidator<AlterarPendenciaCommand>
    {
        public AlterarPendenciaCommandValidator()
        {
            RuleFor(c => c.Pendencia)
            .NotEmpty()
            .WithMessage("A pendência deve ser informado para alteração da pendência.");

        }
    }
}
