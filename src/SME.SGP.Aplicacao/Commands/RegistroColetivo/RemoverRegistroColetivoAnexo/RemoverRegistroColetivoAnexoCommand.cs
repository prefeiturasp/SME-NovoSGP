using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class RemoverRegistroColetivoAnexoCommand : IRequest<bool>
    {
        public RemoverRegistroColetivoAnexoCommand(long registroColetivoId)
        {
            RegistroColetivoId = registroColetivoId;
        }

        public long RegistroColetivoId { get; set; }
    }

    public class RemoverRegistroColetivoAnexoCommandValidator : AbstractValidator<RemoverRegistroColetivoAnexoCommand>
    {
        public RemoverRegistroColetivoAnexoCommandValidator()
        {
            RuleFor(a => a.RegistroColetivoId)
                .NotEmpty()
                .WithMessage("O Id do registro coletivo deve ser informado!");
        }
    }
}
