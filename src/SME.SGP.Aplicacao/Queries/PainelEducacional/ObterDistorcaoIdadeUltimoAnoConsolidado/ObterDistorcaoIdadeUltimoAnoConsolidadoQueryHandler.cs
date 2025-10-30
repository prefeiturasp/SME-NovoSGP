using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterDistorcaoIdadeUltimoAnoConsolidado
{
    public class ObterDistorcaoIdadeUltimoAnoConsolidadoQueryHandler : IRequestHandler<ObterDistorcaoIdadeUltimoAnoConsolidadoQuery, int>
    {
        private readonly IRepositorioPainelEducacionalConsolidacaoDistorcaoIdade _repositorio;

        public ObterDistorcaoIdadeUltimoAnoConsolidadoQueryHandler(IRepositorioPainelEducacionalConsolidacaoDistorcaoIdade repositorio)
        {
            _repositorio = repositorio;
        }
        public async Task<int> Handle(ObterDistorcaoIdadeUltimoAnoConsolidadoQuery request, CancellationToken cancellationToken)
        {
            var ultimoAno = await _repositorio.ObterUltimoAnoConsolidadoAsync();
            return ultimoAno ?? 0;
        }
    }
}