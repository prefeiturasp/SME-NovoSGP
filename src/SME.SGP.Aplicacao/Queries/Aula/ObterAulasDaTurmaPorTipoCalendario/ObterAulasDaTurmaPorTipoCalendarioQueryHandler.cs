using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasDaTurmaPorTipoCalendarioQueryHandler : IRequestHandler<ObterAulasDaTurmaPorTipoCalendarioQuery, IEnumerable<Dominio.Aula>>
    {
        private readonly IRepositorioAula repositorioAula;

        public ObterAulasDaTurmaPorTipoCalendarioQueryHandler(IRepositorioAula repositorioAula)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
        }

        public async Task<IEnumerable<Dominio.Aula>> Handle(ObterAulasDaTurmaPorTipoCalendarioQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAula.ObterAulasPorTurmaETipoCalendario(request.TipoCalendarioId, request.TurmaId);
        }
    }
}
