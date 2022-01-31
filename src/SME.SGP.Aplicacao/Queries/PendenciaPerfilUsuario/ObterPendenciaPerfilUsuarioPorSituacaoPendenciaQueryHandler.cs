using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaPerfilUsuarioPorSituacaoPendenciaQueryHandler : IRequestHandler<ObterPendenciaPerfilUsuarioPorSituacaoPendenciaQuery, IEnumerable<PendenciaPerfilUsuarioDto>>
    {
        private readonly IRepositorioPendenciaPerfilUsuario repositorio;

        public ObterPendenciaPerfilUsuarioPorSituacaoPendenciaQueryHandler(IRepositorioPendenciaPerfilUsuario repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<PendenciaPerfilUsuarioDto>> Handle(ObterPendenciaPerfilUsuarioPorSituacaoPendenciaQuery request, CancellationToken cancellationToken)
            => await repositorio.ObterPorSituacao(request.SituacaoPendencia);
    }
}
