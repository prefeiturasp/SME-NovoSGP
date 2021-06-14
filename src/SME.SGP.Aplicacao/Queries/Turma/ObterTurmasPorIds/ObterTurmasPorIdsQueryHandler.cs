using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasPorIdsQueryHandler : IRequestHandler<ObterTurmasPorIdsQuery, IEnumerable<Turma>>
    {
        private readonly IRepositorioTurma repositorioTurma;

        public ObterTurmasPorIdsQueryHandler(IRepositorioTurma  repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new System.ArgumentNullException(nameof(repositorioTurma));
        }
        public async Task<IEnumerable<Turma>> Handle(ObterTurmasPorIdsQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTurma.ObterTurmasPorIds(request.TurmasIds);
        }
    }
}
