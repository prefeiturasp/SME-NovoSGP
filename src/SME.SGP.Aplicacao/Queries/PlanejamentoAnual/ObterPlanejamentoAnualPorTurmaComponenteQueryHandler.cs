using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanejamentoAnualPorTurmaComponenteQueryHandler : IRequestHandler<ObterPlanejamentoAnualPorTurmaComponenteQuery, PlanejamentoAnualDto>
    {
        private readonly IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;

        public ObterPlanejamentoAnualPorTurmaComponenteQueryHandler(IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual,
                                                                    IRepositorioPeriodoEscolar repositorioPeriodoEscolar)
        {
            this.repositorioPlanejamentoAnual = repositorioPlanejamentoAnual ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnual));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPeriodoEscolar));
        }
        public async Task<PlanejamentoAnualDto> Handle(ObterPlanejamentoAnualPorTurmaComponenteQuery request, CancellationToken cancellationToken)
        {
            var planejamentoAnual = await repositorioPlanejamentoAnual.ObterPorTurmaEComponenteCurricular(request.TurmaId, request.ComponenteCurricularId);
            return new PlanejamentoAnualDto
            {
                Id = planejamentoAnual.Id,
                ComponenteCurricularId = request.ComponenteCurricularId,
                TurmaId = request.TurmaId,
                PeriodosEscolares = planejamentoAnual.PeriodosEscolares.Select(p => new PlanejamentoAnualPeriodoEscolarDto
                {
                    Bimestre = p.PeriodoEscolar.Bimestre,
                    PeriodoEscolarId = p.PeriodoEscolarId,
                    Id = p.Id,
                    Componentes = p.ComponentesCurriculares.Select(c => new PlanejamentoAnualComponenteDto
                    {
                        PlanejamentoAnualComponenteCurricularId = c.Id,
                        ComponenteCurricularId = c.ComponenteCurricularId,
                        Descricao = c.Descricao,
                        PlanejamentoAnualPeriodoEscolarId = c.PlanejamentoAnualPeriodoEscolarId,
                        ObjetivosAprendizagemId = c.ObjetivosAprendizagem.Select(o => o.ObjetivoAprendizagemId)
                    })

                })
            };
        }
    }
}
