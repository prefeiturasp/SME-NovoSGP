using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDisciplinasPorIdsQueryHandler : IRequestHandler<ObterDisciplinasPorIdsQuery, IEnumerable<DisciplinaDto>>
    {
        private readonly IRepositorioComponenteCurricular repositorioComponenteCurricular;

        public ObterDisciplinasPorIdsQueryHandler(IRepositorioComponenteCurricular repositorioComponenteCurricular)
        {
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
        }

        public async Task<IEnumerable<DisciplinaDto>> Handle(ObterDisciplinasPorIdsQuery request, CancellationToken cancellationToken)
        {
            return await repositorioComponenteCurricular.ObterDisciplinasPorIds(request.Ids);
        }

    }
}
