using MediatR;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class InserirConsolidacaoIdepCommand : IRequest<bool>
    {
        public IEnumerable<PainelEducacionalConsolidacaoIdep> DadosIdep { get; }

        public InserirConsolidacaoIdepCommand(IEnumerable<PainelEducacionalConsolidacaoIdep> dadosIdep)
        {
            DadosIdep = dadosIdep;
        }
    }
}