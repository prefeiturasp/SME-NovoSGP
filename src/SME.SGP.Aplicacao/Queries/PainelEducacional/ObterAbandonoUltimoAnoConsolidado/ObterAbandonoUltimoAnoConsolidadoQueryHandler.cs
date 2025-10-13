using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAbandonoUltimoAnoConsolidado
{
    public class ObterAbandonoUltimoAnoConsolidadoQueryHandler : IRequestHandler<ObterAbandonoUltimoAnoConsolidadoQuery, int>
    {
        private readonly IRepositorioPainelEducacionalConsolidacaoAbandono _repositorio;

        public ObterAbandonoUltimoAnoConsolidadoQueryHandler(IRepositorioPainelEducacionalConsolidacaoAbandono repositorio)
        {
            _repositorio = repositorio;
        }
        public async Task<int> Handle(ObterAbandonoUltimoAnoConsolidadoQuery request, CancellationToken cancellationToken)
        {
            var ultimoAno = await _repositorio.ObterUltimoAnoConsolidadoAsync();
            return ultimoAno ?? 0;
        }
    }
}