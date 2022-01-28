using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulaIdPorComponenteCurricularIdTurmaIdEDataProfessorQueryHandler : IRequestHandler<ObterAulaIdPorComponenteCurricularIdTurmaIdEDataProfessorQuery, long?>
    {
        private readonly IRepositorioAulaConsulta repositorioAula;
        public ObterAulaIdPorComponenteCurricularIdTurmaIdEDataProfessorQueryHandler(IRepositorioAulaConsulta repositorioAula)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
        }
        public async Task<long?> Handle(ObterAulaIdPorComponenteCurricularIdTurmaIdEDataProfessorQuery request, CancellationToken cancellationToken)
            => await repositorioAula.ObterAulaIdPorComponenteCurricularIdTurmaIdEDataProfessor(request.ComponenteCurricularId, request.TurmaId, request.Data,request.ProfessorRf);
    }
}
