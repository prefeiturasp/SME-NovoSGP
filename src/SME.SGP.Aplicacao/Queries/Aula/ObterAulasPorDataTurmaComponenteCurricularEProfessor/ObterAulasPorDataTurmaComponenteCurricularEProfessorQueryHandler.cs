using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasPorDataTurmaComponenteCurricularEProfessorQueryHandler : IRequestHandler<ObterAulasPorDataTurmaComponenteCurricularEProfessorQuery, IEnumerable<AulaConsultaDto>>
    {
        private readonly IRepositorioAulaConsulta repositorioAula;
        public ObterAulasPorDataTurmaComponenteCurricularEProfessorQueryHandler(IRepositorioAulaConsulta repositorioAula)
        {
            this.repositorioAula = repositorioAula ?? throw new System.ArgumentNullException(nameof(repositorioAula));
        }

        public async Task<IEnumerable<AulaConsultaDto>> Handle(ObterAulasPorDataTurmaComponenteCurricularEProfessorQuery request, CancellationToken cancellationToken)
        {
            var codigosComponentes = request.CodigosComponentesCurriculares
                .Select(cc => cc.ToString()).ToArray();

            return await repositorioAula
                .ObterAulasPorDataTurmaComponenteCurricularProfessorRf(request.DataAula, request.CodigoTurma, codigosComponentes, request.CodigoRfProfessor);
        }
    }
}
