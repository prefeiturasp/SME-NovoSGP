using MediatR;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoAprovacao
{
    public class SalvarConsolidacaoAprovacaoUeCommand : IRequest<bool>
    {
        public SalvarConsolidacaoAprovacaoUeCommand(IEnumerable<PainelEducacionalConsolidacaoAprovacaoUe> indicadores)
        {
            Indicadores = indicadores;
        }

        public IEnumerable<PainelEducacionalConsolidacaoAprovacaoUe> Indicadores { get; set; }
    }
}
