using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaPorIdsQueryHandler : IRequestHandler<ObterTurmaPorIdsQuery, IEnumerable<Turma>>
    {
        private readonly IRepositorioTurma repositorioTurma;

        public ObterTurmaPorIdsQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }
        public async Task<IEnumerable<Turma>> Handle(ObterTurmaPorIdsQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTurma.ObterPorIdsAsync(request.Ids);
        }
    }
}
