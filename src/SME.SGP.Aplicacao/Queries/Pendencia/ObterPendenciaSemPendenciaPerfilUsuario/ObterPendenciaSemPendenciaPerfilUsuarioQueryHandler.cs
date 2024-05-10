using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao 
{
    public class ObterPendenciaSemPendenciaPerfilUsuarioQueryHandler : IRequestHandler<ObterPendenciaSemPendenciaPerfilUsuarioQuery, IEnumerable<PendenciaPendenteDto>>
    {
        private readonly IRepositorioPendencia repositorioPendencia;

        public ObterPendenciaSemPendenciaPerfilUsuarioQueryHandler(IRepositorioPendencia repositorioPendencia)
        {
            this.repositorioPendencia = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
        }

        public Task<IEnumerable<PendenciaPendenteDto>> Handle(ObterPendenciaSemPendenciaPerfilUsuarioQuery request, CancellationToken cancellationToken)
            => repositorioPendencia.ObterPendenciasSemPendenciaPerfilUsuario();
    }
}
