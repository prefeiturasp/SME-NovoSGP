using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExistePendenciaPlanoAEEQueryHandler : IRequestHandler<ExistePendenciaPlanoAEEQuery, bool>
    {
        private readonly IRepositorioPendenciaPlanoAEE repositorioPendenciaPlanoAEE;

        public ExistePendenciaPlanoAEEQueryHandler(IRepositorioPendenciaPlanoAEE repositorioPendenciaPlanoAEE)
        {
            this.repositorioPendenciaPlanoAEE = repositorioPendenciaPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPendenciaPlanoAEE));
        }

        public async Task<bool> Handle(ExistePendenciaPlanoAEEQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPendenciaPlanoAEE
                .ExistePendenciaPorPlano(request.PlanoAeeId);

        }
    }
}
