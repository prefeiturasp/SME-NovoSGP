using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ConciliacaoFrequenciaTurmaUeCommand : IRequest<bool>
    {
        public ConciliacaoFrequenciaTurmaUeCommand(string ueCodigo, int anoLetivo)
        {
            UeCodigo = ueCodigo;
            AnoLetivo = anoLetivo;
        }

        public string UeCodigo { get; set; }
        public int AnoLetivo { get; set; }
    }
}
