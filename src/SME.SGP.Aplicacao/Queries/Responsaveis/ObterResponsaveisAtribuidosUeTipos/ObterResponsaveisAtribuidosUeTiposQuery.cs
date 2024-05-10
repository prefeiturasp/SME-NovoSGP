using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterResponsaveisAtribuidosUeTiposQuery : IRequest<IEnumerable<SupervisorEscolasDreDto>>
    {
        public ObterResponsaveisAtribuidosUeTiposQuery(TipoResponsavelAtribuicao[] tiposResponsavelAtribuicao)
        {
            TiposResponsavelAtribuicao = tiposResponsavelAtribuicao;
        }

        public TipoResponsavelAtribuicao[] TiposResponsavelAtribuicao { get; set; }
    }
}
