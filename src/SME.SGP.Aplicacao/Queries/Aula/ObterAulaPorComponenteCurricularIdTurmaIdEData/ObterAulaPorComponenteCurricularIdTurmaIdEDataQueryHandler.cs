using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulaPorComponenteCurricularIdTurmaIdEDataQueryHandler : IRequestHandler<ObterAulaPorComponenteCurricularIdTurmaIdEDataQuery, Aula>
    {
        private readonly IRepositorioAulaConsulta repositorioAula;

        public ObterAulaPorComponenteCurricularIdTurmaIdEDataQueryHandler(IRepositorioAulaConsulta repositorioAula)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
        }

        public async Task<Aula> Handle(ObterAulaPorComponenteCurricularIdTurmaIdEDataQuery request, CancellationToken cancellationToken)
            => await repositorioAula.ObterAulaPorComponenteCurricularIdTurmaIdEData(request.ComponenteCurricularId, request.TurmaId, request.Data);
    }
}
