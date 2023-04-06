using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterCompensacaoAusenciaAlunoEAulaPorAulaIdQueryHandler : IRequestHandler<ObterCompensacaoAusenciaAlunoEAulaPorAulaIdQuery, IEnumerable<CompensacaoAusenciaAlunoEDataDto>>
    {
        private readonly IRepositorioCompensacaoAusenciaAlunoConsulta repositorioCompensacaoAusenciaAlunoConsulta;

        public ObterCompensacaoAusenciaAlunoEAulaPorAulaIdQueryHandler(IRepositorioCompensacaoAusenciaAlunoConsulta repositorio)
        {
            this.repositorioCompensacaoAusenciaAlunoConsulta = repositorio;
        }

        public async Task<IEnumerable<CompensacaoAusenciaAlunoEDataDto>> Handle(ObterCompensacaoAusenciaAlunoEAulaPorAulaIdQuery request, CancellationToken cancellationToken)
            => await repositorioCompensacaoAusenciaAlunoConsulta.ObterCompensacaoAusenciaAlunoEAulaPorAulaId(request.AulaId);
    }
}