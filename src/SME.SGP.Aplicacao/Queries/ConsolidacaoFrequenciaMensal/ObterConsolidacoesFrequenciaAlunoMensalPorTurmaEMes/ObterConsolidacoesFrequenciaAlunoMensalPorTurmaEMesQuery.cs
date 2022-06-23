using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterConsolidacoesFrequenciaAlunoMensalPorTurmaEMesQuery : IRequest<IEnumerable<ConsolidacaoFrequenciaAlunoMensalDto>>
    {
        public ObterConsolidacoesFrequenciaAlunoMensalPorTurmaEMesQuery(long turmaId, int mes)
        {
            TurmaId = turmaId;
            Mes = mes;
        }

        public long TurmaId { get; set; }
        public int Mes { get; set; }
    }
}
