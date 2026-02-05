using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoPlanoAEE;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoPlanoAEE
{
    public class SalvarPainelEducacionalConsolidacaoPlanoAEECommand : IRequest<bool>
    {

        public SalvarPainelEducacionalConsolidacaoPlanoAEECommand(
            IEnumerable<DadosParaConsolidarPlanosAEEDto> indicadores)
        {
            Indicadores = indicadores;
        }
        public IEnumerable<DadosParaConsolidarPlanosAEEDto> Indicadores { get; set; }
    }
}
