using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ExisteUsuarioComMesmoEmailQuery : IRequest<bool>
    {
        public ExisteUsuarioComMesmoEmailQuery(string login, string email)
        {
            Login = login;
            Email = email;
        }
        public string Login { get; set; }
        public string Email { get; set; }
    }

    public class ExisteUsuarioComMesmoEmailQueryValidator : AbstractValidator<ExisteUsuarioComMesmoEmailQuery>
    {
        public ExisteUsuarioComMesmoEmailQueryValidator()
        {
            RuleFor(c => c.Login)
                .NotEmpty()
                .WithMessage("O login deve ser informado para validação se e-mail já está em uso por outro usuário");

            RuleFor(c => c.Email)
              .NotEmpty()
              .WithMessage("O e-mail deve ser informado para validação se e-mail já está em uso por outro usuário.");
        }
    }
}
