using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConselhoDeClasseNotaBimestresCacheQueryHandler : ObterCache<ConselhoClasseAlunoNotasConceitosRetornoDto>, IRequestHandler<ConselhoDeClasseNotaBimestresCacheQuery, ValorCache<ConselhoClasseAlunoNotasConceitosRetornoDto>>
    {
        private ConselhoDeClasseNotaBimestresCacheQuery request;

        public ConselhoDeClasseNotaBimestresCacheQueryHandler(IRepositorioCache repositorioCache) : base(repositorioCache)
        {
        }

        public Task<ValorCache<ConselhoClasseAlunoNotasConceitosRetornoDto>> Handle(ConselhoDeClasseNotaBimestresCacheQuery request, CancellationToken cancellationToken)
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
