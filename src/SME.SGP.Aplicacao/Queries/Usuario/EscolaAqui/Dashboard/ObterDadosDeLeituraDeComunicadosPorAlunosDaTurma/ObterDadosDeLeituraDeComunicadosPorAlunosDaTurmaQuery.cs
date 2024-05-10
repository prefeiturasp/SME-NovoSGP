using MediatR;
using SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDeLeituraDeComunicadosPorAlunosDaTurmaQuery : IRequest<IEnumerable<DadosLeituraAlunosComunicadoPorTurmaDto>>
    {
        public long ComunicadoId { get; set; }
        public long CodigoTurma { get; set; }

        public ObterDadosDeLeituraDeComunicadosPorAlunosDaTurmaQuery(long comunicadoId, long codigoTurma)
        {
            ComunicadoId = comunicadoId;
            CodigoTurma = codigoTurma;
        }
    }
}