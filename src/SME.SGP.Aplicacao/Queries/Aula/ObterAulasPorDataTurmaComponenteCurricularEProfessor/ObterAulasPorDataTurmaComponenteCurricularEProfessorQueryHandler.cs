using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
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
            return await repositorioAula.ObterAulasPorDataTurmaComponenteCurricularProfessorRf(request.DataAula, request.CodigoTurma, request.CodigoComponenteCurricular.ToString(), request.CodigoRfProfessor);
        }
    }
}
