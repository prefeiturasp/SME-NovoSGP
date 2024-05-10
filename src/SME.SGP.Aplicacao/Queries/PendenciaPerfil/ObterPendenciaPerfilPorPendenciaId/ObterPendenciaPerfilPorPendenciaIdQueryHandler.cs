using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaPerfilPorPendenciaIdQueryHandler : IRequestHandler<ObterPendenciaPerfilPorPendenciaIdQuery, IEnumerable<PendenciaPerfil>>
    {
        private readonly IRepositorioPendenciaPerfil repositorio;

        public ObterPendenciaPerfilPorPendenciaIdQueryHandler(IRepositorioPendenciaPerfil repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<PendenciaPerfil>> Handle(ObterPendenciaPerfilPorPendenciaIdQuery request, CancellationToken cancellationToken)
            => await repositorio.ObterPorPendenciaId(request.PendenciaId);
    }
}
