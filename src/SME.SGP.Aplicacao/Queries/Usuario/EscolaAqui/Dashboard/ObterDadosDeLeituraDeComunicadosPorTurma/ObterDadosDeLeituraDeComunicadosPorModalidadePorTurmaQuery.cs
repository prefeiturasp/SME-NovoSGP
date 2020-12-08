using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDeLeituraDeComunicadosPorModalidadePorTurmaQuery : IRequest<IEnumerable<DadosDeLeituraDoComunicadoPorModalidadeETurmaDto>>
    {
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public long ComunicadoId { get; set; }
        public Modalidade Modalidade { get; set; }
        public int ModoVisualizacao { get; set; }

        public ObterDadosDeLeituraDeComunicadosPorModalidadePorTurmaQuery(string codigoDre, string codigoUe, long comunicadoId, int modoVisualizacao, Modalidade modalidade)
        {
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
            ComunicadoId = comunicadoId;
            ModoVisualizacao = modoVisualizacao;
            Modalidade = modalidade;
        }
    }
}