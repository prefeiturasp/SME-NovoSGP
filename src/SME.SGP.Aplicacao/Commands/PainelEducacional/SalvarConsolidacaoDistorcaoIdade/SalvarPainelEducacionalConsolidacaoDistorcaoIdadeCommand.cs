using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoDistorcaoIdade;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoDistorcaoIdade
{
    public class SalvarPainelEducacionalConsolidacaoDistorcaoIdadeCommand : IRequest<bool>
    {

        public SalvarPainelEducacionalConsolidacaoDistorcaoIdadeCommand(
            IEnumerable<ConsolidacaoDistorcaoIdadeDto> indicadores)
        {
            Indicadores = indicadores;
        }
        public IEnumerable<ConsolidacaoDistorcaoIdadeDto> Indicadores { get; set; }
    }
}
