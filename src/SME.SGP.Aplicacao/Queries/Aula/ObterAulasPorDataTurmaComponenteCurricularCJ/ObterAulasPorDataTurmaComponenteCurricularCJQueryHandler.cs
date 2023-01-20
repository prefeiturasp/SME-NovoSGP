using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasPorDataTurmaComponenteCurricularCJQueryHandler : IRequestHandler<ObterAulasPorDataTurmaComponenteCurricularCJQuery, IEnumerable<AulaConsultaDto>>
    {
        private readonly IRepositorioAulaConsulta repositorioAula;
        public ObterAulasPorDataTurmaComponenteCurricularCJQueryHandler(IRepositorioAulaConsulta repositorioAula)
        {
            this.repositorioAula = repositorioAula ?? throw new System.ArgumentNullException(nameof(repositorioAula));
        }

        public async Task<IEnumerable<AulaConsultaDto>> Handle(ObterAulasPorDataTurmaComponenteCurricularCJQuery request, CancellationToken cancellationToken)
            => await repositorioAula.ObterAulasPorDataTurmaComponenteCurricularCJ(request.DataAula, request.CodigoTurma, request.CodigoComponenteCurricular.ToString(), request.AulaCJ);
    }
}
