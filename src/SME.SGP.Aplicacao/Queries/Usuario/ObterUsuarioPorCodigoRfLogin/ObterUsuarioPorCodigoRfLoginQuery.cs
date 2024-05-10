using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPorCodigoRfLoginQuery : IRequest<Usuario>
    {
        public string CodigoRf { get; set; }
        public string Login { get; set; }

        public ObterUsuarioPorCodigoRfLoginQuery(string codigoRf, string login)
        {
            CodigoRf = codigoRf;
            Login = login;
        }
    }

    public class ObterUsuarioPorCodigoRfLoginQueryValidator : AbstractValidator<ObterUsuarioPorCodigoRfLoginQuery>
    {
        public ObterUsuarioPorCodigoRfLoginQueryValidator()
        {
            
        }
    }
}