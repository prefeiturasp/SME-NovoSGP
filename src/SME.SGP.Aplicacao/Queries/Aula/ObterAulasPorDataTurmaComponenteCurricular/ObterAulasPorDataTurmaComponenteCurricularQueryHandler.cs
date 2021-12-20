using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasPorDataTurmaComponenteCurricularQueryHandler : IRequestHandler<ObterAulasPorDataTurmaComponenteCurricularQuery, IEnumerable<AulaConsultaDto>>
    {
        private readonly IRepositorioAulaConsulta repositorioAula;
        public ObterAulasPorDataTurmaComponenteCurricularQueryHandler(IRepositorioAulaConsulta repositorioAula)
        {
            this.repositorioAula = repositorioAula ?? throw new System.ArgumentNullException(nameof(repositorioAula));
        }

        public async Task<IEnumerable<AulaConsultaDto>> Handle(ObterAulasPorDataTurmaComponenteCurricularQuery request, CancellationToken cancellationToken)
            => await repositorioAula.ObterAulasPorDataTurmaComponenteCurricular(request.DataAula, request.CodigoTurma, request.CodigoComponenteCurricular.ToString(), request.AulaCJ);
    }
}
