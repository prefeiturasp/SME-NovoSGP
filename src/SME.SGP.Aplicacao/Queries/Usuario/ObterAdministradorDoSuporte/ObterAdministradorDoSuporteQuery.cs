using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public  class ObterAdministradorDoSuporteQuery : IRequest<AdministradorSuporte>
    {
        private static ObterAdministradorDoSuporteQuery _instance;
        public static ObterAdministradorDoSuporteQuery Instance => _instance ??= new();
    }
}
