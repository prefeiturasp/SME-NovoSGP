using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class AlterarEmailUsuarioCommand : IRequest
    {
        public AlterarEmailUsuarioCommand(string login, string email)
        {
            Login = login;
            Email = email;
        }

        public string Login { get; }
        public string Email { get; }
    }

    public class AlterarEmailCommandValidator : AbstractValidator<AlterarEmailUsuarioCommand>
    {
        public AlterarEmailCommandValidator()
        {
            RuleFor(a => a.Login)
                .NotEmpty()
                .WithMessage("O login deve ser informado para alteração do e-mail");

            RuleFor(a => a.Email)
                .NotEmpty()
                .WithMessage("O e-mail deve ser informada para alteração");

        }
    }
}
