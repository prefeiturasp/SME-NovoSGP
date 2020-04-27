using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasQueryHandler : IRequestHandler<ObterAulasQuery, IEnumerable<Aula>>
    {
        private readonly IRepositorioAula repositorioAula;

        public ObterAulasQueryHandler(IRepositorioAula repositorioAula)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
        }
        public async Task<IEnumerable<Aula>> Handle(ObterAulasQuery request, CancellationToken cancellationToken)
        {
            return await  repositorioAula.ObterAulas_v2(request.TipoCalendarioId, request.TurmaCodigo, request.UeCodigo, request.CriadorRF); 
        }
    }
}
