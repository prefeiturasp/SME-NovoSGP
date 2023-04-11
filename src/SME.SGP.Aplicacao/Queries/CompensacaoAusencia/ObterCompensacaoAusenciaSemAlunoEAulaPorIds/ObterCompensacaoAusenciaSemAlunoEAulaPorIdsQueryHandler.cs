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
        private readonly IRepositorioCompensacaoAusenciaConsulta repositorioCompensacaoAusenciaConsulta;

        public ObterCompensacaoAusenciaSemAlunoEAulaPorIdsQueryHandler(IRepositorioCompensacaoAusenciaConsulta compensacaoAusenciaConsulta)
        {
            repositorioCompensacaoAusenciaConsulta = compensacaoAusenciaConsulta ?? throw new ArgumentNullException(nameof(compensacaoAusenciaConsulta));
        }

        public Task<IEnumerable<long>> Handle(ObterCompensacaoAusenciaSemAlunoEAulaPorIdsQuery request, CancellationToken cancellationToken)
        {
            return repositorioCompensacaoAusenciaConsulta.ObterPorIds(request.CompensacaoAusenciaIds);
        }
    }
}