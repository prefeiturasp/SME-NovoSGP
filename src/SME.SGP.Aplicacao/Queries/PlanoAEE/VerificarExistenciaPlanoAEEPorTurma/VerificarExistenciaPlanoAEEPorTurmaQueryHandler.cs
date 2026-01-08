using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Queries.PlanoAEE.VerificarExistenciaPlanoAEEPorTurma
{
    public class VerificarExistenciaPlanoAEEPorTurmaQueryHandler : IRequestHandler<VerificarExistenciaPlanoAEEPorTurmaQuery, PlanoAEEResumoDto>
    {
        private readonly IRepositorioPlanoAEEConsulta repositorioPlanoAEE;

        public VerificarExistenciaPlanoAEEPorTurmaQueryHandler(IRepositorioPlanoAEEConsulta repositorioPlanoAee)
        {
            repositorioPlanoAEE = repositorioPlanoAee ?? throw new ArgumentNullException(nameof(repositorioPlanoAee));
        }

        public async Task<PlanoAEEResumoDto> Handle(VerificarExistenciaPlanoAEEPorTurmaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPlanoAEE.ObterPlanoPorTurma(request.Filtro);
        }
    }
}