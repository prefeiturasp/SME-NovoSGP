using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Consts;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIndicadoresPap
{
    public class ObterInformacoesPapUltimoAnoConsolidadoQueryHandler : IRequestHandler<ObterInformacoesPapUltimoAnoConsolidadoQuery, int>
    {
        private readonly IRepositorioPainelEducacionalConsolidacaoIndicadoresPap repositorioPainelEducacionalPap;
        public ObterInformacoesPapUltimoAnoConsolidadoQueryHandler(IRepositorioPainelEducacionalConsolidacaoIndicadoresPap repositorioPainelEducacionalPap)
        {
            this.repositorioPainelEducacionalPap = repositorioPainelEducacionalPap ?? throw new System.ArgumentNullException(nameof(repositorioPainelEducacionalPap));
        }
        public async Task<int> Handle(ObterInformacoesPapUltimoAnoConsolidadoQuery request, CancellationToken cancellationToken)
        {
            var ano = await repositorioPainelEducacionalPap.ObterUltimoAnoConsolidado();
            if (ano.HasValue)
                return ano.Value;
            return 0;
        }
    }
}
