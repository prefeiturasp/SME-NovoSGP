using MediatR;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoProficienciaIdep
{
    public class SalvarConsolidacaoProficienciaIdepCommand : IRequest<bool>
    {
        public IEnumerable<PainelEducacionalConsolidacaoProficienciaIdepUe> consolidacaoIdepUe { get; set; }

        public SalvarConsolidacaoProficienciaIdepCommand(IEnumerable<PainelEducacionalConsolidacaoProficienciaIdepUe> consolidacaoIdepUe)
        {
            this.consolidacaoIdepUe = consolidacaoIdepUe;
        }
    }
}