using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObteCodigosDreUePorTurmaQueryHandler : IRequestHandler<ObteCodigosDreUePorTurmaQuery, DreUeDaTurmaDto>
    {
        private readonly IRepositorioTurma repositorioTurma;

        public ObteCodigosDreUePorTurmaQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<DreUeDaTurmaDto> Handle(ObteCodigosDreUePorTurmaQuery request, CancellationToken cancellationToken)
            => await repositorioTurma.ObterCodigosDreUe(request.TurmaCodigo);
    }
}
