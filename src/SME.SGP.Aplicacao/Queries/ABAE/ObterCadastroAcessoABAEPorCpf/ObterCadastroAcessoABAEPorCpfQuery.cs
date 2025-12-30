using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterCadastroAcessoABAEPorCpfQuery : IRequest<CadastroAcessoABAE>
    {
        public ObterCadastroAcessoABAEPorCpfQuery(string cpf)
        {
            Cpf = cpf;
        }
        public string Cpf { get; set; }
    }

    public class ObterCadastroAcessoABAEPorCpfUsuarioQueryValidator : AbstractValidator<ObterCadastroAcessoABAEPorCpfQuery>
    {
        public ObterCadastroAcessoABAEPorCpfUsuarioQueryValidator()
        {
            RuleFor(a => a.Cpf)
                .NotEmpty()
                .WithMessage("É necessário informar o cpf do usuário para a busca de cadastro de acesso ABAE");
        }
    }
}
