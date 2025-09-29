using MediatR;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoPap
{
    public class SalvarConsolidacaoPapSmeCommand : IRequest<bool>
    {
        public IList<PainelEducacionalConsolidacaoPapSme> Consolidacao { get; set; }

        public SalvarConsolidacaoPapSmeCommand(IList<PainelEducacionalConsolidacaoPapSme> consolidacao)
        {
            Consolidacao = consolidacao;
        }
    }
}
