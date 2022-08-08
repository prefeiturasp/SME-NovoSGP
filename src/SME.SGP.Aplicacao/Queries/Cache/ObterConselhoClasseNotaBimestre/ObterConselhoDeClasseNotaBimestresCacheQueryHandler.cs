using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoDeClasseNotaBimestresCacheQueryHandler : ObterCache<ConselhoClasseAlunoNotasConceitosRetornoDto>, IRequestHandler<ObterConselhoDeClasseNotaBimestresCacheQuery, ValorCache<ConselhoClasseAlunoNotasConceitosRetornoDto>>
    {
        private ObterConselhoDeClasseNotaBimestresCacheQuery request;

        public ObterConselhoDeClasseNotaBimestresCacheQueryHandler(IRepositorioCache repositorioCache) : base(repositorioCache)
        {
        }

        public Task<ValorCache<ConselhoClasseAlunoNotasConceitosRetornoDto>> Handle(ObterConselhoDeClasseNotaBimestresCacheQuery request, CancellationToken cancellationToken)
        {
            this.request = request;

            return ObterDoCache();
        }

        protected override string ObterChave()
        {
            return $"NotaConceitoBimestre-{request.ConselhoClasseId}-{request.CodigoAluno}-{request.Bimestre}";
        }
    }
}
