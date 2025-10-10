using MediatR;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoPap
{
    public class SalvarConsolidacaoPapDreCommand : IRequest<bool>
    {
        public IList<PainelEducacionalConsolidacaoPapDre> Consolidacao { get; set; }

        public SalvarConsolidacaoPapDreCommand(IList<PainelEducacionalConsolidacaoPapDre> consolidacao)
        {
            Consolidacao = consolidacao;
        }
    }
}