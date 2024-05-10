using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosDreUePorTurmaQueryHandler : IRequestHandler<ObterCodigosDreUePorTurmaQuery, DreUeDaTurmaDto>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurma;

        public ObterCodigosDreUePorTurmaQueryHandler(IRepositorioTurmaConsulta repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<DreUeDaTurmaDto> Handle(ObterCodigosDreUePorTurmaQuery request, CancellationToken cancellationToken)
            => await repositorioTurma.ObterCodigosDreUe(request.TurmaCodigo);
    }
}
