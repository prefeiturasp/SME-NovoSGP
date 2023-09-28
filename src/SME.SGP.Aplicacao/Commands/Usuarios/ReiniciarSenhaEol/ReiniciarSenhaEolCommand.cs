using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ReiniciarSenhaEolCommand : IRequest
    {
        public ReiniciarSenhaEolCommand(string login)
        {
            Login = login;
        }
        
        public string Login { get; set; }
    }

    public class ReiniciarSenhaEolCommandValidator : AbstractValidator<ReiniciarSenhaEolCommand>
    {
        public ReiniciarSenhaEolCommandValidator()
        {
            RuleFor(c => c.Login)
                .NotEmpty()
                .WithMessage("O login deve ser informado para reiniciar a senha.");
        }
    }
}
