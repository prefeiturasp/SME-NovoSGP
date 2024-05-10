using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExisteConsolidacaoMatriculaTurmaPorAnoQuery : IRequest<bool>
    {
        public ExisteConsolidacaoMatriculaTurmaPorAnoQuery(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }

        public int AnoLetivo { get; set; }
    }
}
