using MediatR;

namespace SME.SGP.Aplicacao
{
    public class BloquearConsolidacaoFrequenciaAlunoMensalPorTurmaEMesCommand : IRequest<bool>
    {
        public BloquearConsolidacaoFrequenciaAlunoMensalPorTurmaEMesCommand(long turmaId, int mes)
        {
            TurmaId = turmaId;
            Mes = mes;
        }

        public long TurmaId { get; }
        public int Mes { get; }
    }
}
