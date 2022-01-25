using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasPorDataPeriodoQueryHandler : IRequestHandler<ObterAulasPorDataPeriodoQuery, IEnumerable<Aula>>
    {
        private readonly IRepositorioAulaConsulta repositorioAula;

        public ObterAulasPorDataPeriodoQueryHandler(IRepositorioAulaConsulta repositorioAula)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
        }

        public async Task<IEnumerable<Aula>> Handle(ObterAulasPorDataPeriodoQuery request, CancellationToken cancellationToken)
            => await repositorioAula.ObterAulasPorDataPeriodo(request.DataInicio, request.DataFim, request.TurmaId, request.ComponenteCurricularId);
    }
}
