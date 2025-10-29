using MediatR;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SolicitarConsolidacaoProficienciaIdep
{
    public class SolicitarConsolidacaoProficienciaIdepCommand : IRequest<bool>
    {
        public int AnoLetivo { get; set; }
        public SolicitarConsolidacaoProficienciaIdepCommand(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }
    }
}