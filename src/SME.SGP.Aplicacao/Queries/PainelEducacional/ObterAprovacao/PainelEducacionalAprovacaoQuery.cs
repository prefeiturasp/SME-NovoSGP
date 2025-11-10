using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAprovacao
{
    public class PainelEducacionalAprovacaoQuery : IRequest<IEnumerable<PainelEducacionalAprovacaoDto>>
    {
        public PainelEducacionalAprovacaoQuery(int anoLetivo, string codigoDre)
        {
            AnoLetivo = anoLetivo;
            CodigoDre = codigoDre;
        }

        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
    }
}
