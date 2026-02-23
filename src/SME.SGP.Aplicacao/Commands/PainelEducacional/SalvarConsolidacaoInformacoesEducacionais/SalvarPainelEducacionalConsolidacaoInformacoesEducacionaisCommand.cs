using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional.InformacoesEducacionais;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoInformacoesEducacionais
{
    public class SalvarPainelEducacionalConsolidacaoInformacoesEducacionaisCommand : IRequest<bool>
    {

        public SalvarPainelEducacionalConsolidacaoInformacoesEducacionaisCommand(IEnumerable<DadosParaConsolidarInformacoesEducacionaisDto> indicadores)
        {
            Indicadores = indicadores;
        }
        public IEnumerable<DadosParaConsolidarInformacoesEducacionaisDto> Indicadores { get; set; }
    }
}
