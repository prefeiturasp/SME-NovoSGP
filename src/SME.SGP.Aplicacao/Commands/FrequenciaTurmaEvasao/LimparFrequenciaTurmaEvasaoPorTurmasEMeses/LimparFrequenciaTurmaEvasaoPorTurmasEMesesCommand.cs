using MediatR;

namespace SME.SGP.Aplicacao
{
    public class LimparFrequenciaTurmaEvasaoPorTurmasEMesesCommand : IRequest<bool>
    {
        public LimparFrequenciaTurmaEvasaoPorTurmasEMesesCommand(long[] turmasIds, int[] meses)
        {
            TurmasIds = turmasIds;
            Meses = meses;
        }

        public long[] TurmasIds { get; set; }
        public int[] Meses { get; set; }
    }
}
