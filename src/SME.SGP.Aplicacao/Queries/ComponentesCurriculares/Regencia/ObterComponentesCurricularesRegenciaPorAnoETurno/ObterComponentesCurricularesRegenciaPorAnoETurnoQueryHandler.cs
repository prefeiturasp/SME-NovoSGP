using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesRegenciaPorAnoETurnoQueryHandler : IRequestHandler<ObterComponentesCurricularesRegenciaPorAnoETurnoQuery, IEnumerable<DisciplinaDto>>
    {
        private readonly IRepositorioComponenteCurricular repositorioComponenteCurricular;
        public ObterComponentesCurricularesRegenciaPorAnoETurnoQueryHandler(IRepositorioComponenteCurricular repositorioComponenteCurricular)
        {
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
        }

        public async Task<IEnumerable<DisciplinaDto>> Handle(ObterComponentesCurricularesRegenciaPorAnoETurnoQuery request, CancellationToken cancellationToken)
        {
            return (await repositorioComponenteCurricular.ObterComponentesCurricularesRegenciaPorAnoETurno(request.Ano, request.Turno)).OrderBy(c => c.Nome);
        }
    }
}
