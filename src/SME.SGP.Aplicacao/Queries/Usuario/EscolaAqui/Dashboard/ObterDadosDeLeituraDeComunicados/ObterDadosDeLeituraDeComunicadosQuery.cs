using MediatR;
using SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados;
using SME.SGP.Infra.Enumerados;
using System.Collections.Generic;


namespace SME.SGP.Aplicacao
{
    public class ObterDadosDeLeituraDeComunicadosQuery : IRequest<IEnumerable<DadosDeLeituraDoComunicadoDto>>
    {
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public long ComunicadoId { get; set; }

        public bool AgruparModalidade { get; set; }
        public int ModoVisualizacao { get; set; }

        public ObterDadosDeLeituraDeComunicadosQuery(string codigoDre, string codigoUe, long comunicadoId, int modoVisualizacao, bool agruparModalidade)
        {
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
            ComunicadoId = comunicadoId;
            ModoVisualizacao = modoVisualizacao;
            AgruparModalidade = agruparModalidade;
        }
    }
}