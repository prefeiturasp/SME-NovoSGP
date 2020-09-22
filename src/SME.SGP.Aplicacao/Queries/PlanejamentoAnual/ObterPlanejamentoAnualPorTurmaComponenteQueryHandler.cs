using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanejamentoAnualPorTurmaComponenteQueryHandler : IRequestHandler<ObterPlanejamentoAnualPorTurmaComponenteQuery, PlanejamentoAnualPeriodoEscolarDto>
    {
        private readonly IRepositorioPlanejamentoAnualPeriodoEscolar repositorioPlanejamentoAnualPeriodoEscolar;

        public ObterPlanejamentoAnualPorTurmaComponenteQueryHandler(IRepositorioPlanejamentoAnualPeriodoEscolar repositorioPlanejamentoAnualPeriodoEscolar)
        {
            this.repositorioPlanejamentoAnualPeriodoEscolar = repositorioPlanejamentoAnualPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualPeriodoEscolar));
        }

        public async Task<PlanejamentoAnualPeriodoEscolarDto> Handle(ObterPlanejamentoAnualPorTurmaComponenteQuery request, CancellationToken cancellationToken)
        {
            var periodo = await repositorioPlanejamentoAnualPeriodoEscolar.ObterPlanejamentoAnualPeriodoEscolarPorTurmaEComponenteCurricular(request.TurmaId, request.ComponenteCurricularId, request.PeriodoEscolarId);

            if (periodo == null)
                return null;

            return new PlanejamentoAnualPeriodoEscolarDto
            {
                Bimestre = periodo.PeriodoEscolar.Bimestre,
                Componentes = periodo.ComponentesCurriculares.Select(c => new PlanejamentoAnualComponenteDto
                {
                    ComponenteCurricularId = c.ComponenteCurricularId,
                    Descricao = c.Descricao,
                    PlanejamentoAnualComponenteCurricularId = c.Id,
                    PlanejamentoAnualPeriodoEscolarId = c.PlanejamentoAnualPeriodoEscolarId,
                    ObjetivosAprendizagemId = c.ObjetivosAprendizagem.Select(o => o.ObjetivoAprendizagemId)
                }),
                Id = periodo.Id,
                PeriodoEscolarId = periodo.PeriodoEscolar.Id
            };
        }
    }
}
