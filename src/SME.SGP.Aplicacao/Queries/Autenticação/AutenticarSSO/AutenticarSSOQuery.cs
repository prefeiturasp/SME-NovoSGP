using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao.Queries.Autenticação.AutenticarSSO
{
    public class AutenticarSSOQuery : IRequest<AutenticacaoSSODto>
    {
        public AutenticarSSOQuery(string login, string senha)
        {
            Login = login;
            Senha = senha;
        }

        public string Login { get; set; }
        public string Senha { get; set; }
    }

    public class AutenticarSSOQueryValidator : AbstractValidator<AutenticarSSOQuery>
    {
        public AutenticarSSOQueryValidator()
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
