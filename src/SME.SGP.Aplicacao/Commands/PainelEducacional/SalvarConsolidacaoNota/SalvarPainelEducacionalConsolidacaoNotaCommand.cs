using MediatR;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoNota
{
    public class SalvarPainelEducacionalConsolidacaoNotaCommand : IRequest<bool>
    {
        public SalvarPainelEducacionalConsolidacaoNotaCommand(IList<PainelEducacionalConsolidacaoNota> notasConsolidadasDre)
        {
            NotasConsolidadasDre = notasConsolidadasDre;
        }
        public IList<PainelEducacionalConsolidacaoNota> NotasConsolidadasDre { get; set; }
    }
}
