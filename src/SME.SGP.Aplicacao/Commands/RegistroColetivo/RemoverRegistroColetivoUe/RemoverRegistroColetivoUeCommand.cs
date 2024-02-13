using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class RemoverRegistroColetivoUeCommand : IRequest<bool>
    {
        public RemoverRegistroColetivoUeCommand(long registroColetivoId)
        {
            RegistroColetivoId = registroColetivoId;
        }

        public long RegistroColetivoId { get; set; }
    }

    public class RemoverRegistroColetivoUeCommandValidator : AbstractValidator<RemoverRegistroColetivoUeCommand>
    {
        public RemoverRegistroColetivoUeCommandValidator()
        {
            RuleFor(a => a.RegistroColetivoId)
                .NotEmpty()
                .WithMessage("O Id do registro coletivo deve ser informado!");
        }
    }
}
