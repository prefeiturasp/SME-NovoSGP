using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoSondagemEscrita
{
    public class SalvarPainelEducacionalConsolidacaoSondagemEscritaUeCommand : IRequest<bool>
    {
        public SalvarPainelEducacionalConsolidacaoSondagemEscritaUeCommand(IEnumerable<Dominio.Entidades.PainelEducacionalConsolidacaoSondagemEscritaUe> indicadores) => Indicadores = indicadores;
        public IEnumerable<Dominio.Entidades.PainelEducacionalConsolidacaoSondagemEscritaUe> Indicadores { get; set; }
    }
}
