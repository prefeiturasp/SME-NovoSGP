using MediatR;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterProficienciaIdepParaConsolidacao
{
    public class ObterProficienciaIdepParaConsolidacaoQuery : IRequest<IEnumerable<PainelEducacionalConsolidacaoProficienciaIdepUe>>
    {
        public ObterProficienciaIdepParaConsolidacaoQuery(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }
        public int AnoLetivo { get; set; }
    }
}