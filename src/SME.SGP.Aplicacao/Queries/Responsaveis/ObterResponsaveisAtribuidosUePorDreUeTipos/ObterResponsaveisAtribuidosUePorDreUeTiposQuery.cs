using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterResponsaveisAtribuidosUePorDreUeTiposQuery : IRequest<IEnumerable<SupervisorEscolasDreDto>>
    {
        public ObterResponsaveisAtribuidosUePorDreUeTiposQuery(string codigoDre, string codigoUe, TipoResponsavelAtribuicao[] tiposResponsavelAtribuicao)
        {
            CodigoUe = codigoUe;
            CodigoDre = codigoDre;
            TiposResponsavelAtribuicao = tiposResponsavelAtribuicao;
        }

        public string CodigoUe { get; set; }
        public string CodigoDre { get; set; }
        public TipoResponsavelAtribuicao[] TiposResponsavelAtribuicao { get; set; }
    }
}
