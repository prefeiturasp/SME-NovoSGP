using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaCargaQueryHandler : IRequestHandler<ObterPendenciaCargaQuery, IEnumerable<PendenciaPendenteDto>>
    {
        private readonly IRepositorioPendencia repositorioPendencia;

        public ObterPendenciaCargaQueryHandler(IRepositorioPendencia repositorioPendencia)
        {
            this.repositorioPendencia = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
        }

        public async Task<IEnumerable<PendenciaPendenteDto>> Handle(ObterPendenciaCargaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPendencia
                .ObterPendenciasPendentes();

        }
    }
}
