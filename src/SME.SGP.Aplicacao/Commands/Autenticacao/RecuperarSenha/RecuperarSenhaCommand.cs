using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class RecuperarSenhaCommand : IRequest<string>
    {
        public RecuperarSenhaCommand(string login)
        {
            Login = login;
        }

        public string Login { get; }
    }

    public class RecuperarSenhaCommandValidator : AbstractValidator<RecuperarSenhaCommand>
    {
        public RecuperarSenhaCommandValidator()
        {
            RuleFor(x => x.Login)
                .NotEmpty()
                .WithMessage("o login de usuário deve ser informado para recuperação de senha");
        }
    }
}
