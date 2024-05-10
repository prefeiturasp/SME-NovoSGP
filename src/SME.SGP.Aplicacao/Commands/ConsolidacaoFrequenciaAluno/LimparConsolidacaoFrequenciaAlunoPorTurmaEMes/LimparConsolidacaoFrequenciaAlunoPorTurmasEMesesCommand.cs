using MediatR;

namespace SME.SGP.Aplicacao
{
    public class LimparConsolidacaoFrequenciaAlunoPorTurmasEMesesCommand : IRequest<bool>
    {
        public LimparConsolidacaoFrequenciaAlunoPorTurmasEMesesCommand(long[] turmasIds, int[] meses)
        {
            TurmasIds = turmasIds;
            Meses = meses;
        }

        public long[] TurmasIds { get; set; }
        public int[] Meses { get; set; }
    }
}
