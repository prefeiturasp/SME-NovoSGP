using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosDreUePorTurmaQueryHandler : IRequestHandler<ObterCodigosDreUePorTurmaQuery, DreUeDto>
    {
        private readonly IRepositorioTurma repositorioTurma;

        public ObterCodigosDreUePorTurmaQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<DreUeDto> Handle(ObterCodigosDreUePorTurmaQuery request, CancellationToken cancellationToken)
            => await repositorioTurma.ObterCodigosDreUe(request.TurmaCodigo);
    }
}
