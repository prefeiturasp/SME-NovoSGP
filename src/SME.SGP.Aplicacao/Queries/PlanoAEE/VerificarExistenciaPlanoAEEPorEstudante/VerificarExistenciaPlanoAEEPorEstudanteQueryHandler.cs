using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificarExistenciaPlanoAEEPorEstudanteQueryHandler : IRequestHandler<VerificarExistenciaPlanoAEEPorEstudanteQuery, PlanoAEEResumoDto>
    {
        private readonly IRepositorioPlanoAEEConsulta repositorioPlanoAEE;

        public VerificarExistenciaPlanoAEEPorEstudanteQueryHandler(IRepositorioPlanoAEEConsulta repositorioPlanoAEE)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
        }

        public async Task<PlanoAEEResumoDto> Handle(VerificarExistenciaPlanoAEEPorEstudanteQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPlanoAEE.ObterPlanoPorEstudante(request.Filtro);
        }
    }
}
