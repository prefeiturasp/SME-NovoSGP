using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterResponsaveisPorDreQuery : IRequest<IEnumerable<SupervisorEscolasDreDto>>
    {
        public ObterResponsaveisPorDreQuery(string codigoDre, TipoResponsavelAtribuicao? tipoResponsavelAtribuicao)
        {
            CodigoDre = codigoDre;
            TipoResponsavelAtribuicao = tipoResponsavelAtribuicao;
        }

        public string CodigoDre { get; set; }
        public TipoResponsavelAtribuicao? TipoResponsavelAtribuicao { get; set; }
    }
}
