using MediatR;
using SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDeLeituraDeComunicadosPorModalidadePorTurmaQuery : IRequest<IEnumerable<DadosDeLeituraDoComunicadoPorModalidadeETurmaDto>>
    {
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public long ComunicadoId { get; set; }
        public short[] Modalidades { get; set; }
        public long[] CodigosTurmas { get; set; }
        public int ModoVisualizacao { get; set; }

        public ObterDadosDeLeituraDeComunicadosPorModalidadePorTurmaQuery(string codigoDre, string codigoUe, long comunicadoId, short[] modalidades, long[] codigosTurmas, int modoVisualizacao)
        {
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
            ComunicadoId = comunicadoId;
            Modalidades = modalidades;
            CodigosTurmas = codigosTurmas;
            ModoVisualizacao = modoVisualizacao;
        }
    }
}