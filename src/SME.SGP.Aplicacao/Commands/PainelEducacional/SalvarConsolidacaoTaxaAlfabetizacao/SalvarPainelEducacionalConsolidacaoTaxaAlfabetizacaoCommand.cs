using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoTaxaAlfabetizacao
{
    public class SalvarPainelEducacionalConsolidacaoTaxaAlfabetizacaoCommand : IRequest<bool>
    {
        public SalvarPainelEducacionalConsolidacaoTaxaAlfabetizacaoCommand(IEnumerable<Dominio.Entidades.PainelEducacionalConsolidacaoTaxaAlfabetizacao> indicadores) => Indicadores = indicadores;
        public IEnumerable<Dominio.Entidades.PainelEducacionalConsolidacaoTaxaAlfabetizacao> Indicadores { get; set; }
    }
}
