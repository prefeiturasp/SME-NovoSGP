using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasFuturasPorDataBaseTurmaComponentesCurricularesQueryHandler : IRequestHandler<ObterAulasFuturasPorDataBaseTurmaComponentesCurricularesQuery, IEnumerable<AulaConsultaDto>>
    {
        private readonly IRepositorioAulaConsulta repositorioAula;
        public ObterAulasFuturasPorDataBaseTurmaComponentesCurricularesQueryHandler(IRepositorioAulaConsulta repositorioAula)
        {
            this.repositorioAula = repositorioAula ?? throw new System.ArgumentNullException(nameof(repositorioAula));
        }

        public async Task<IEnumerable<AulaConsultaDto>> Handle(ObterAulasFuturasPorDataBaseTurmaComponentesCurricularesQuery request, CancellationToken cancellationToken)
            => await repositorioAula.ObterAulasFuturasPorDataTurmaComponentesCurriculares(request.DataBase, request.CodigoTurma, request.CodigosComponentesCurriculares);
    }
}
