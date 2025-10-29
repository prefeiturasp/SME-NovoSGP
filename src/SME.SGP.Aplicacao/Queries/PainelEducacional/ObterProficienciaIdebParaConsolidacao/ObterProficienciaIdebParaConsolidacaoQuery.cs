using MediatR;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterProficienciaIdebParaConsolidacao
{
    public class ObterProficienciaIdebParaConsolidacaoQuery : IRequest<IEnumerable<PainelEducacionalConsolidacaoProficienciaIdebUe>>
    {
        public ObterProficienciaIdebParaConsolidacaoQuery(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }
        public int AnoLetivo { get; set; }
    }
}