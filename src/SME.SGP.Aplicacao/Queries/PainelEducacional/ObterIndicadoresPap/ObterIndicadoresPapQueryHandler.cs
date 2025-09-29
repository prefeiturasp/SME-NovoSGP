using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIndicadoresPap
{
    public class ObterIndicadoresPapQueryHandler : IRequestHandler<ObterIndicadoresPapQuery, IEnumerable<PainelEducacionalInformacoesPapDto>>
    {
        private readonly IRepositorioPainelEducacionalPap repositorioPainelEducacionalPap;
        public ObterIndicadoresPapQueryHandler(IRepositorioPainelEducacionalPap repositorioPainelEducacionalPap)
        {
            this.repositorioPainelEducacionalPap = repositorioPainelEducacionalPap;
        }
        public async Task<IEnumerable<PainelEducacionalInformacoesPapDto>> Handle(ObterIndicadoresPapQuery request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.CodigoUe))
                return await repositorioPainelEducacionalPap.ObterConsolidacoesUePorAno(request.AnoLetivo, request.CodigoDre, request.CodigoUe);
            if (!string.IsNullOrEmpty(request.CodigoDre))
                return await repositorioPainelEducacionalPap.ObterConsolidacoesDrePorAno(request.AnoLetivo, request.CodigoDre);
            return await repositorioPainelEducacionalPap.ObterConsolidacoesSmePorAno(request.AnoLetivo);
        }
    }
}