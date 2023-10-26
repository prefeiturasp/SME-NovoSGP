using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class AlterarSenhaUsuarioCommand : IRequest<AlterarSenhaRespostaDto>
    {
        public AlterarSenhaUsuarioCommand(string login, string senha)
        {
            Login = login;
            Senha = senha;
        }

        public string Login { get; }
        public string Senha { get; }
    }

    public class AlterarSenhaUsuarioCommandValidator : AbstractValidator<AlterarSenhaUsuarioCommand>
    {
        public AlterarSenhaUsuarioCommandValidator()
        {
            RuleFor(a => a.Login)
                .NotEmpty()
                .WithMessage("O login deve ser informado para alteração do e-mail");

            RuleFor(a => a.Senha)
                .NotEmpty()
                .WithMessage("A senha deve ser informado para alteração do e-mail");

        }
    }
}
