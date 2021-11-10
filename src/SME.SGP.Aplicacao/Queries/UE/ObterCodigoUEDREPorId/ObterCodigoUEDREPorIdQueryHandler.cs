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
    public class ObterCodigoUEDREPorIdQueryHandler : IRequestHandler<ObterCodigoUEDREPorIdQuery, DreUeDto>
    {
        private readonly IRepositorioUe repositorioUe;

        public ObterCodigoUEDREPorIdQueryHandler(IRepositorioUe repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<DreUeDto> Handle(ObterCodigoUEDREPorIdQuery request, CancellationToken cancellationToken)
            => await repositorioUe.ObterCodigosDreUePorId(request.UeId);
    }
}
