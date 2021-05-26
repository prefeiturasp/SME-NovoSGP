using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaIdentificadoresPorUeAnosLetivosQueryHandler : IRequestHandler<ObterTurmaIdentificadoresPorUeAnosLetivosQuery, IEnumerable<IdentificadoresTurmaDto>>
    {
        private readonly IRepositorioTurma repositorioTurma;

        public ObterTurmaIdentificadoresPorUeAnosLetivosQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new System.ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<IEnumerable<IdentificadoresTurmaDto>> Handle(ObterTurmaIdentificadoresPorUeAnosLetivosQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTurma.ObterTurmaIdentificadoresPorUeAnosLetivosAsync(request.UeId, request.AnosLetivos);
        }
    }
}
