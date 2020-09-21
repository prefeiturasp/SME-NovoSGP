using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulaPorComponenteCurricularIdTurmaIdEDataQueryHandler : IRequestHandler<ObterAulaPorComponenteCurricularIdTurmaIdEDataQuery, Aula>
    {
        private readonly IRepositorioAula repositorioAula;

        public ObterAulaPorComponenteCurricularIdTurmaIdEDataQueryHandler(IRepositorioAula repositorioAula)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
        }

        public async Task<Aula> Handle(ObterAulaPorComponenteCurricularIdTurmaIdEDataQuery request, CancellationToken cancellationToken)
            => await repositorioAula.ObterAulaPorComponenteCurricularIdTurmaIdEData(request.ComponenteCurricularId, request.TurmaId, request.Data);
    }
}
