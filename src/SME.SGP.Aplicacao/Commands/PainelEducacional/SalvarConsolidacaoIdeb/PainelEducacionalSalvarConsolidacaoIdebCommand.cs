using MediatR;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoIdeb
{
    public class PainelEducacionalSalvarConsolidacaoIdebCommand : IRequest<bool>
    {
        public PainelEducacionalSalvarConsolidacaoIdebCommand(IEnumerable<PainelEducacionalConsolidacaoIdeb> ideb)
        {
            Ideb = ideb;
        }

        public IEnumerable<PainelEducacionalConsolidacaoIdeb> Ideb { get; set; }
    }
}
