using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ConciliacaoFrequenciaTurmaDreCommand : IRequest<bool>
    {
        public ConciliacaoFrequenciaTurmaDreCommand(long dreId, int anoLetivo)
        {
            DreId = dreId;
            AnoLetivo = anoLetivo;
        }

        public long DreId { get; set; }
        public int AnoLetivo { get; set; }
    }
}
