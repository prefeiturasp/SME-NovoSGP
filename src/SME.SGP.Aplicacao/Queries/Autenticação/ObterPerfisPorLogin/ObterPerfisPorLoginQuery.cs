using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterPerfisPorLoginQuery : IRequest<PerfisApiEolDto>
    {
        public ObterPerfisPorLoginQuery(string login)
        {
            Login = login;
        }
        public string Login { get; set; }
    }

    public class ObterPerfisPorLoginQueryValidator : AbstractValidator<ObterPerfisPorLoginQuery>
    {
        public ObterPerfisPorLoginQueryValidator()
        {
            RuleFor(c => c.Login)
                .NotEmpty()
                .WithMessage("O login deve ser informado para validação se e-mail já está em uso por outro usuário");
        }
    }
}
