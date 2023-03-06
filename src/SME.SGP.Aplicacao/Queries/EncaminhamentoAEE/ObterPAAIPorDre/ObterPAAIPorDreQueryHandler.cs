using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPAAIPorDreQueryHandler : IRequestHandler<ObterPAAIPorDreQuery, IEnumerable<SupervisorEscolasDreDto>>
    {
        private IRepositorioSupervisorEscolaDre repositorioSupervisorEscola;
        public ObterPAAIPorDreQueryHandler(IRepositorioSupervisorEscolaDre repositorioSupervisorEscola)
        {
            this.repositorioSupervisorEscola = repositorioSupervisorEscola;
        }
        public async Task<IEnumerable<SupervisorEscolasDreDto>> Handle(ObterPAAIPorDreQuery request, CancellationToken cancellationToken)
        {
            return await repositorioSupervisorEscola.ObtemSupervisoresPorDreAsync(request.CodigoDre, request.TipoResponsavel);
        }
    }
}
