using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPAAIPorUeQueryHandler : IRequestHandler<ObterPAAIPorUeQuery, IEnumerable<SupervisorDto>>
    {
        private IRepositorioSupervisorEscolaDre repositorioSupervisorEscola;
        public ObterPAAIPorUeQueryHandler(IRepositorioSupervisorEscolaDre repositorioSupervisorEscola)
        {
            this.repositorioSupervisorEscola = repositorioSupervisorEscola;
        }
        public async Task<IEnumerable<SupervisorDto>> Handle(ObterPAAIPorUeQuery request, CancellationToken cancellationToken)
        {
            return await repositorioSupervisorEscola.ObtemSupervisoresPorUeAsync(request.CodigoUe, request.TipoResponsavel);
        }
    }
}
