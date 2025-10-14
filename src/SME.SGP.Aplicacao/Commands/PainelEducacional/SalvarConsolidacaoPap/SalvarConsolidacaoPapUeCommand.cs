using MediatR;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoPap
{
    public class SalvarConsolidacaoPapUeCommand : IRequest<bool>
    {
        public IList<PainelEducacionalConsolidacaoPapUe> Consolidacao { get; set; }

        public SalvarConsolidacaoPapUeCommand(IList<PainelEducacionalConsolidacaoPapUe> consolidacao)
        {
            Consolidacao = consolidacao;
        }
    }
}
