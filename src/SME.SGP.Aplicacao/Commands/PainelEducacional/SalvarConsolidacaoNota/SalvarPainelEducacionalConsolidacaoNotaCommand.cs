using MediatR;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoNota
{
    public class SalvarPainelEducacionalConsolidacaoNotaCommand : IRequest<bool>
    {
        public SalvarPainelEducacionalConsolidacaoNotaCommand(IEnumerable<PainelEducacionalConsolidacaoNota> notasConsolidadasDre)
        {
            NotasConsolidadasDre = notasConsolidadasDre;
        }
        public IEnumerable<PainelEducacionalConsolidacaoNota> NotasConsolidadasDre { get; set; }
    }
}
