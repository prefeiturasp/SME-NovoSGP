using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNotaUltimoAnoConsolidado
{
    public class ObterNotaUltimoAnoConsolidadoQueryHandler : IRequestHandler<ObterNotaUltimoAnoConsolidadoQuery, int>
    {
        private readonly IRepositorioPainelEducacionalConsolidacaoNotaConsulta _repositorio;

        public ObterNotaUltimoAnoConsolidadoQueryHandler(IRepositorioPainelEducacionalConsolidacaoNotaConsulta repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<int> Handle(ObterNotaUltimoAnoConsolidadoQuery request, CancellationToken cancellationToken)
        {
            var ultimoAno = await _repositorio.ObterUltimoAnoConsolidadoAsync();
            return ultimoAno ?? 0;
        }
    }
}
