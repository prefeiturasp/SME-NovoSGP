using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterProficienciaIdep
{
    public class ObterProficienciaIdepQuery : IRequest<IEnumerable<PainelEducacionalProficienciaIdepDto>>
    {
        public ObterProficienciaIdepQuery(int anoLetivo, string codigoUe)
        {
            AnoLetivo = anoLetivo;
            CodigoUe = codigoUe;
        }
        public int AnoLetivo { get; set; }
        public string CodigoUe { get; set; }
    }
}
