using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigoUEDREPorIdQueryHandler : IRequestHandler<ObterCodigoUEDREPorIdQuery, DreUeDto>
    {
        private readonly IRepositorioUeConsulta repositorioUe;

        public ObterCodigoUEDREPorIdQueryHandler(IRepositorioUeConsulta repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<DreUeDto> Handle(ObterCodigoUEDREPorIdQuery request, CancellationToken cancellationToken)
            => await repositorioUe.ObterCodigosDreUePorId(request.UeId);
    }
}
