using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanejamentoAnualComponentePorcompoenteCurricularIdEPeriodoQueryHandler : IRequestHandler<ObterPlanejamentoAnualComponentePorcompoenteCurricularIdEPeriodoQuery, PlanejamentoAnualComponente>
    {
        private readonly IRepositorioPlanejamentoAnualComponente repositorioPlanejamentoAnualComponente;

        public ObterPlanejamentoAnualComponentePorcompoenteCurricularIdEPeriodoQueryHandler(IRepositorioPlanejamentoAnualComponente repositorioPlanejamentoAnualComponente)
        {
            this.repositorioPlanejamentoAnualComponente = repositorioPlanejamentoAnualComponente ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualComponente));
        }

        public async Task<PlanejamentoAnualComponente> Handle(ObterPlanejamentoAnualComponentePorcompoenteCurricularIdEPeriodoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPlanejamentoAnualComponente.ObterPorPlanejamentoAnualPeriodoEscolarId(request.ComponenteCurricularId, request.PeriodoId);
        }
    }
}
