using MediatR;
using SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDeLeituraDeComunicadosPorModalidadeQuery : IRequest<IEnumerable<DadosDeLeituraDoComunicadoPorModalidadeETurmaDto>>
    {
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public long ComunicadoId { get; set; }
        public int ModoVisualizacao { get; set; }

        public ObterDadosDeLeituraDeComunicadosPorModalidadeQuery(string codigoDre, string codigoUe, long comunicadoId, int modoVisualizacao)
        {
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
            ComunicadoId = comunicadoId;
            ModoVisualizacao = modoVisualizacao;
        }
    }
}