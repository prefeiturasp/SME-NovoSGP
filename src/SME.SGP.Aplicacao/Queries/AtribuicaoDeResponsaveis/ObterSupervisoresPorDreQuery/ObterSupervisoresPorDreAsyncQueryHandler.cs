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
    public class ObterSupervisoresPorDreAsyncQueryHandler : IRequestHandler<ObterSupervisoresPorDreAsyncQuery, IEnumerable<SupervisorEscolasDreDto>>
    {
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;
        public ObterSupervisoresPorDreAsyncQueryHandler(IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre)
        {
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
        }
        public async Task<IEnumerable<SupervisorEscolasDreDto>> Handle(ObterSupervisoresPorDreAsyncQuery request, CancellationToken cancellationToken)
        {
            return await repositorioSupervisorEscolaDre.ObtemSupervisoresPorDreAsync(request.CodigoDre, request.TipoResponsavel);
        }
    }
}
