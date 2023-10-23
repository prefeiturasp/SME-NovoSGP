using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class AutenticarQuery : IRequest<AutenticacaoApiEolDto>
    {
        public AutenticarQuery(string login, string senha)
        {
            Login = login;
            Senha = senha;
        }
        public string Login { get; set; }
        public string Senha { get; set; }
    }

    public class AutenticarQueryValidator : AbstractValidator<AutenticarQuery>
    {
        public AutenticarQueryValidator()
        {
            RuleFor(c => c.Login)
                .NotEmpty()
                .WithMessage("O login deve ser informado para autenticação do usuário.");

            RuleFor(c => c.Senha)
              .NotEmpty()
              .WithMessage("A senha deve ser informada para autenticação do usuário.");
        }
    }
}
