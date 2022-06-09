using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ExisteResponsavelAtribuidoUePorUeTipoQuery : IRequest<bool>
    {
        public ExisteResponsavelAtribuidoUePorUeTipoQuery(string codigoUe, TipoResponsavelAtribuicao tipoResponsavelAtribuicao)
        {
            CodigoUe = codigoUe;
            TipoResponsavelAtribuicao = tipoResponsavelAtribuicao;
        }

        public string CodigoUe { get; set; }
        public TipoResponsavelAtribuicao TipoResponsavelAtribuicao { get; set; }
    }
}
