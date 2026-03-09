using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoFluenciaLeitoraUe;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoFluenciaLeitoraUe
{
    public class SalvarPainelEducacionalConsolidacaoFluenciaLeitoraUeCommand : IRequest<bool>
    {

        public SalvarPainelEducacionalConsolidacaoFluenciaLeitoraUeCommand(
            int anoLetivo,
            IEnumerable<ConsolidacaoFluenciaLeitoraUeDto> indicadores)
        {
            AnoLetivo = anoLetivo;
            Indicadores = indicadores;
        }
        public int AnoLetivo { get; set; }
        public IEnumerable<ConsolidacaoFluenciaLeitoraUeDto> Indicadores { get; set; }
    }
}
