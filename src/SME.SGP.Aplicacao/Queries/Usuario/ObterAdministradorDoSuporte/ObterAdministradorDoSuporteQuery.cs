using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public  class ObterAdministradorDoSuporteQuery : IRequest<AdministradorSuporte>
    {
        public static ObterAdministradorDoSuporteQuery Instance => new();
    }
}
