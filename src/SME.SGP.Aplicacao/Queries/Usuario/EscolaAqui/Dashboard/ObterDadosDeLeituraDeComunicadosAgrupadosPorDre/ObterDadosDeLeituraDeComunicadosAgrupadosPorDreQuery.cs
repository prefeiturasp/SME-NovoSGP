using MediatR;
using SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDeLeituraDeComunicadosAgrupadosPorDreQuery : IRequest<IEnumerable<DadosDeLeituraDoComunicadoDto>>
    {
        public long ComunicadoId { get; set; }
        public int ModoVisualizacao { get; set; }

        public ObterDadosDeLeituraDeComunicadosAgrupadosPorDreQuery(long comunicadoId, int modoVisualizacao)
        {
            ComunicadoId = comunicadoId;
            ModoVisualizacao = modoVisualizacao;
        }
    }
}