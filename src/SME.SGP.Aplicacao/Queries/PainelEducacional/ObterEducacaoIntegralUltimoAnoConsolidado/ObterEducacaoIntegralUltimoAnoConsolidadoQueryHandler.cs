using MediatR;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterEducacaoIntegralUltimoAnoConsolidado;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterInformacoesEducacaoIntegralUltimoAnoConsolidado
{
    public class ObterInformacoesEducacaoIntegralUltimoAnoConsolidadoQueryHandler : IRequestHandler<ObterEducacaoIntegralUltimoAnoConsolidadoQuery, int>
    {
        private readonly IRepositorioPainelEducacionalConsolidacaoEducacaoIntegral repositorio;
        public ObterInformacoesEducacaoIntegralUltimoAnoConsolidadoQueryHandler(IRepositorioPainelEducacionalConsolidacaoEducacaoIntegral repositorio)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }
        public async Task<int> Handle(ObterEducacaoIntegralUltimoAnoConsolidadoQuery request, CancellationToken cancellationToken)
        {
            var ano = await repositorio.ObterUltimoAnoConsolidado();
            if (ano.HasValue)
                return ano.Value;
            return 0;
        }
    }
}
