using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasReduzidaPorTipoCalendarioQueryHandler : IRequestHandler<ObterAulasReduzidaPorTipoCalendarioQuery, IEnumerable<AulaReduzidaDto>>
    {
        private readonly IRepositorioAula repositorioAula;

        public ObterAulasReduzidaPorTipoCalendarioQueryHandler(IRepositorioAula repositorioAula)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
        }

        public async Task<IEnumerable<AulaReduzidaDto>> Handle(ObterAulasReduzidaPorTipoCalendarioQuery request, CancellationToken cancellationToken)
                => await repositorioAula.ObterAulasReduzidasParaPendenciasAulaDiasNaoLetivos(request.TipoCalendarioId, request.TiposEscola);
    }
}
