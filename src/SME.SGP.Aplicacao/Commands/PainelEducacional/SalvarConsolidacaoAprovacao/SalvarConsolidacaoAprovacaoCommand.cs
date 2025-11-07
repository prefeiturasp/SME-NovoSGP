using MediatR;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoAprovacao
{
    public class SalvarConsolidacaoAprovacaoCommand : IRequest<bool>
    {
        public SalvarConsolidacaoAprovacaoCommand(IEnumerable<PainelEducacionalConsolidacaoAprovacao> indicadores)
        {
            this.indicadores = indicadores;
        }

        public IEnumerable<PainelEducacionalConsolidacaoAprovacao> indicadores { get; set; }
    }
}
