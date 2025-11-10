using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAprovacaoUe
{
    public class PainelEducacionalAprovacaoUeQuery : IRequest<IEnumerable<PainelEducacionalAprovacaoUeDto>>
    {
        public PainelEducacionalAprovacaoUeQuery(int anoLetivo, string codigoUe)
        {
            AnoLetivo = anoLetivo;
            CodigoUe = codigoUe;
        }

        public int AnoLetivo { get; set; }
        public string CodigoUe { get; set; }
    }
}
