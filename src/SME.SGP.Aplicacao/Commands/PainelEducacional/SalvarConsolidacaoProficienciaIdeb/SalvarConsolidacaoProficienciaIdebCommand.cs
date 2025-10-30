using MediatR;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoProficienciaIdeb
{
    public class SalvarConsolidacaoProficienciaIdebCommand : IRequest<bool>
    {
        public IEnumerable<PainelEducacionalConsolidacaoProficienciaIdebUe> consolidacaoIdebUe { get; set; }

        public SalvarConsolidacaoProficienciaIdebCommand(IEnumerable<PainelEducacionalConsolidacaoProficienciaIdebUe> consolidacaoIdebUe)
        {
            this.consolidacaoIdebUe = consolidacaoIdebUe;
        }
    }
}