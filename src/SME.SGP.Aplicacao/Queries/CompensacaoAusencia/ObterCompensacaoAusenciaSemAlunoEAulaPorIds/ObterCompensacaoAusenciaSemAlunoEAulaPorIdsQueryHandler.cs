using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterCompensacaoAusenciaSemAlunoEAulaPorIdsQueryHandler : IRequestHandler<ObterCompensacaoAusenciaSemAlunoEAulaPorIdsQuery,IEnumerable<long>>
    {
        private readonly IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia;

        public ObterCompensacaoAusenciaSemAlunoEAulaPorIdsQueryHandler(IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia)
        {
            repositorioCompensacaoAusencia = repositorioCompensacaoAusencia ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusencia));
        }

        public Task<IEnumerable<long>> Handle(ObterCompensacaoAusenciaSemAlunoEAulaPorIdsQuery request, CancellationToken cancellationToken)
        {
            return repositorioCompensacaoAusencia.ObterSemAlunoPorIds(request.CompensacaoAusenciaIds);
        }
    }
}