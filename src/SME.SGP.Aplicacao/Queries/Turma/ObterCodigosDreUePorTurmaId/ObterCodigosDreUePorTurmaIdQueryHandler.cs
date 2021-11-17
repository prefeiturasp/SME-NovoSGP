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
    public class ObterCodigosDreUePorTurmaIdQueryHandler : IRequestHandler<ObterCodigosDreUePorTurmaIdQuery, DreUeDto>
    {
        private readonly IRepositorioTurma repositorioTurma;

        public ObterCodigosDreUePorTurmaIdQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<DreUeDto> Handle(ObterCodigosDreUePorTurmaIdQuery request, CancellationToken cancellationToken)
            => await repositorioTurma.ObterCodigosDreUePorId(request.TurmaId);
    }
}
