using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPorCodigoRfLoginOuAdicionaQuery : IRequest<Usuario>
    {
        public ObterUsuarioPorCodigoRfLoginOuAdicionaQuery(string codigoRf, string login = "", string nome = "", string email = "", bool buscaLogin = false)
        {
            CodigoRf = codigoRf;
            Login = login;
            Nome = nome;
            Email = email;
            BuscaLogin = buscaLogin;
        }

        public bool BuscaLogin { get; set; }

        public string Email { get; set; }

        public string Nome { get; set; }

        public string Login { get; set; }

        public string CodigoRf { get; set; }
    }

    public class ObterUsuarioPorCodigoRfLoginOuAdicionaQueryValidator : AbstractValidator<ObterUsuarioPorCodigoRfLoginOuAdicionaQuery>
    {
        public ObterUsuarioPorCodigoRfLoginOuAdicionaQueryValidator()
        {
            RuleFor(x => x.CodigoRf).NotEmpty().WithMessage("Informe o Código RF do Usuário para Obter o Usuario Por Codigo RF Login Ou Adiciona");
        }
    }
}