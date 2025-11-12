using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoEducacaoIntegral;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoEducacaoIntegral
{
    public class SalvarPainelEducacionalConsolidacaoEducacaoIntegralCommand : IRequest<bool>
    {
        public SalvarPainelEducacionalConsolidacaoEducacaoIntegralCommand(
            int anoLetivo, IEnumerable<DadosParaConsolidarEducacaoIntegralDto> indicadores)
        {
            AnoLetivo = anoLetivo;
            Indicadores = indicadores;
        }
        public int AnoLetivo { get; set; }
        public IEnumerable<DadosParaConsolidarEducacaoIntegralDto> Indicadores { get; set; }
    }
}
