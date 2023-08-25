using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasReposicaoComPendenciaCriadaQueryHandler : IRequestHandler<ObterAulasReposicaoComPendenciaCriadaQuery, long[]>
    {
        private readonly IRepositorioPendenciaAulaReposicaoConsulta repositorioPendenciaAulaReposicaoConsulta;

        public ObterAulasReposicaoComPendenciaCriadaQueryHandler(IRepositorioPendenciaAulaReposicaoConsulta repositorioPendenciaAulaReposicaoConsulta)
        {
            this.repositorioPendenciaAulaReposicaoConsulta = repositorioPendenciaAulaReposicaoConsulta ?? throw new ArgumentNullException(nameof(repositorioPendenciaAulaReposicaoConsulta));
        }

        public async Task<long[]> Handle(ObterAulasReposicaoComPendenciaCriadaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPendenciaAulaReposicaoConsulta
                .ObterAulasReposicaoComPendenciaCriada(request.AulasId);
        }
    }
}
