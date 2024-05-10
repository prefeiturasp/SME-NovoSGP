using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRepositorioAulaPorAulaRecorrenteQueryHandler : IRequestHandler<ObterRepositorioAulaPorAulaRecorrenteQuery, IEnumerable<Aula>>
    {
        private readonly IRepositorioAulaConsulta repositorioAulaConsulta;

        public ObterRepositorioAulaPorAulaRecorrenteQueryHandler(IRepositorioAulaConsulta repositorio)
        {
            this.repositorioAulaConsulta = repositorio;
        }

        public async Task<IEnumerable<Aula>> Handle(ObterRepositorioAulaPorAulaRecorrenteQuery request, CancellationToken cancellationToken)
            => await repositorioAulaConsulta.ObterAulasRecorrencia(request.AulaPaiIdOrigem, request.AulaOrigemId, request.FimRecorrencia);
    }
}

