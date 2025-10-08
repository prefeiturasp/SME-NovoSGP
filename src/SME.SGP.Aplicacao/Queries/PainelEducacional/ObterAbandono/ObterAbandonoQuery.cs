using MediatR;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAbandono
{
    public class ObterAbandonoVisaoSmeDreQuery : IRequest<IEnumerable<PainelEducacionalAbandono>>
    {
        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public ObterAbandonoVisaoSmeDreQuery(int anoLetivo, string codigoDre, string codigoUe)
        {
            AnoLetivo = anoLetivo;
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
        }
    }
}
