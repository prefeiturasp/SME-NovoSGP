using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class RemoverRegistroColetivoCommand : IRequest<bool>
    {
        public RemoverRegistroColetivoCommand(long registroColetivoId)
        {
            RegistroColetivoId = registroColetivoId;
        }

        public long RegistroColetivoId { get; set; }
    }

    public class RemoverRegistroColetivoCommandValidator : AbstractValidator<RemoverRegistroColetivoCommand>
    {
        public RemoverRegistroColetivoCommandValidator()
        {
            RuleFor(a => a.RegistroColetivoId)
                .NotEmpty()
                .WithMessage("O Id do registro coletivo deve ser informado!");
        }
    }
}
