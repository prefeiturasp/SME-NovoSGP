using MediatR;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoNota
{
    public class SalvarPainelEducacionalConsolidacaoNotaCommand : IRequest<bool>
    {
        public SalvarPainelEducacionalConsolidacaoNotaCommand(
            IList<PainelEducacionalConsolidacaoNota> notasConsolidadasDre,
            IList<PainelEducacionalConsolidacaoNotaUe> notasConsolidadasUe)
        {
            NotasConsolidadasDre = notasConsolidadasDre;
            NotasConsolidadasUe = notasConsolidadasUe;
        }
        public IList<PainelEducacionalConsolidacaoNota> NotasConsolidadasDre { get; set; }
        public IList<PainelEducacionalConsolidacaoNotaUe> NotasConsolidadasUe { get; set; }
    }
}
