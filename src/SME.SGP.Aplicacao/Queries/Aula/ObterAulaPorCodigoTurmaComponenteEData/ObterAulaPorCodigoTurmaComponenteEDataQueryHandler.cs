using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulaPorCodigoTurmaComponenteEDataQueryHandler : IRequestHandler<ObterAulaPorCodigoTurmaComponenteEDataQuery, DataAulaDto>
    {
        private readonly IRepositorioAula repositorio;

        public ObterAulaPorCodigoTurmaComponenteEDataQueryHandler(IRepositorioAula repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<DataAulaDto> Handle(ObterAulaPorCodigoTurmaComponenteEDataQuery request, CancellationToken cancellationToken)
            => await repositorio.ObterAulaPorCodigoTurmaComponenteEData(request.TurmaId, request.ComponenteCurricularId, request.DataCriacao);
    }
}
