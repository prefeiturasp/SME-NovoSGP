using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIndicadoresPap
{
    public class ObterIndicadoresPapSgpConsolidadoQuery : IRequest<IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto>>
    {
        public int AnoLetivo { get; set; }

        public ObterIndicadoresPapSgpConsolidadoQuery(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }
    }
}