using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PlanoAEE;

namespace SME.SGP.Aplicacao.Queries.PlanoAEE.VerificarExistenciaPlanoAEEPorTurma
{
    public class VerificarExistenciaPlanoAEEPorTurmaQueryHandler : IRequestHandler<VerificarExistenciaPlanoAEEPorTurmaQuery, IEnumerable<PlanoAEEResumoIntegracaoDto>>
    {
        private readonly IRepositorioPlanoAEEConsulta repositorioPlanoAEE;

        public VerificarExistenciaPlanoAEEPorTurmaQueryHandler(IRepositorioPlanoAEEConsulta repositorioPlanoAee)
        {
            repositorioPlanoAEE = repositorioPlanoAee ?? throw new ArgumentNullException(nameof(repositorioPlanoAee));
        }


        public async Task<IEnumerable<PlanoAEEResumoIntegracaoDto>> Handle(VerificarExistenciaPlanoAEEPorTurmaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPlanoAEE.ObterPlanoPorTurma(request.Filtro);
        }
    }
}