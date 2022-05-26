using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ConciliacaoFrequenciaAnoCommand : IRequest<bool>
    {
        public ConciliacaoFrequenciaAnoCommand(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }

        public int AnoLetivo { get; set; }
    }
}
