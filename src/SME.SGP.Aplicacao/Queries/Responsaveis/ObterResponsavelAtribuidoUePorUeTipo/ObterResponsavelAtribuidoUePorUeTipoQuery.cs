using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterResponsavelAtribuidoUePorUeTipoQuery : IRequest<IEnumerable<UsuarioEolRetornoDto>>
    {
        public ObterResponsavelAtribuidoUePorUeTipoQuery(string codigoUe, TipoResponsavelAtribuicao tipoResponsavelAtribuicao)
        {
            CodigoUe = codigoUe;
            TipoResponsavelAtribuicao = tipoResponsavelAtribuicao;
        }

        public string CodigoUe { get; set; }
        public TipoResponsavelAtribuicao TipoResponsavelAtribuicao { get; set; }
    }
}
