using MediatR;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SolicitarConsolidacaoProficienciaIdeb
{
    public class SolicitarConsolidacaoProficienciaIdebCommand : IRequest<bool>
    {
        public int AnoLetivo { get; set; }
        public SolicitarConsolidacaoProficienciaIdebCommand(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }
    }
}