using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAutenticacaoSemSenhaQuery : IRequest<AutenticacaoApiEolDto>
    {
        public ObterAutenticacaoSemSenhaQuery(string login)
        {
            Login = login;
        }
        public string Login { get; set; }
    }

    public class ObterAutenticacaoSemSenhaQueryValidator : AbstractValidator<ObterAutenticacaoSemSenhaQuery>
    {
        public ObterAutenticacaoSemSenhaQueryValidator()
        {
            RuleFor(c => c.Login)
                .NotEmpty()
                .WithMessage("O login deve ser informado para autenticação sem senha.");
        }
    }
}
