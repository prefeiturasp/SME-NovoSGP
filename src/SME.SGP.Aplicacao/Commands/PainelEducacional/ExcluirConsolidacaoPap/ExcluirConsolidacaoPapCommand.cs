using MediatR;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.ExcluirConsolidacaoPap
{
    public class ExcluirConsolidacaoPapCommand : IRequest<bool>
    {
        public int AnoLetivo { get; set; }
        public ExcluirConsolidacaoPapCommand(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }
    }
}