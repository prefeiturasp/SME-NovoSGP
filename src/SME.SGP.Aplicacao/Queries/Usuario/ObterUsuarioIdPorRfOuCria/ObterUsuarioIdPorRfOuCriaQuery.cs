using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioIdPorRfOuCriaQuery : IRequest<long>
    {
        public ObterUsuarioIdPorRfOuCriaQuery(string usuarioRf, string usuarioNome = "")
        {
            UsuarioRf = usuarioRf;
            UsuarioNome = usuarioNome;
        }

        public string UsuarioRf { get; set; }

        public string UsuarioNome { get; set; }
    }

    public class ObterUsuarioIdPorRfOuCriaQueryValidator : AbstractValidator<ObterUsuarioIdPorRfOuCriaQuery>
    {
        public ObterUsuarioIdPorRfOuCriaQueryValidator()
        {
            RuleFor(c => c.UsuarioRf)
               .NotEmpty()
               .WithMessage("O RF do usuário deve ser informado para consulta.");
        }
    }
}
