using MediatR;

namespace SME.SGP.Aplicacao
{
    public class InserirConsolidacaoDiariaDashBoardFrequenciaCommand : IRequest<bool>
    {
        public InserirConsolidacaoDiariaDashBoardFrequenciaCommand(int anoLetivo, int mes, long turmaId)
        {
            AnoLetivo = anoLetivo;
            Mes = mes;
            TurmaId = turmaId;
        }

        public int AnoLetivo { get; set; }
        public int Mes { get; set; }
        public long TurmaId { get; set; }
    }
}
