using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNotasParaConsolidacao
{
    public class ObterNotasParaConsolidacaoQueryHandler : IRequestHandler<ObterNotasParaConsolidacaoQuery, IEnumerable<PainelEducacionalConsolidacaoNotaDadosBrutos>>
    {
        private readonly IRepositorioPainelEducacionalConsolidacaoNotaConsulta repositorioPainelEducacionalConsolidacaoNotaConsulta;
        public ObterNotasParaConsolidacaoQueryHandler(IRepositorioPainelEducacionalConsolidacaoNotaConsulta repositorioPainelEducacionalConsolidacaoNotaConsulta)
        {
            this.repositorioPainelEducacionalConsolidacaoNotaConsulta = repositorioPainelEducacionalConsolidacaoNotaConsulta;
        }
        public async Task<IEnumerable<PainelEducacionalConsolidacaoNotaDadosBrutos>> Handle(ObterNotasParaConsolidacaoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPainelEducacionalConsolidacaoNotaConsulta.ObterDadosBrutosPorAnoLetivoAsync(request.AnoLetivo);
        }
    }
}
