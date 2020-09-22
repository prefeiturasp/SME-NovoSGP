using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPlanejamentoAnualCommandHandler : IRequestHandler<SalvarPlanejamentoAnualCommand, AuditoriaDto>
    {
        private readonly IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual;
        private readonly IRepositorioPlanejamentoAnualPeriodoEscolar repositorioPlanejamentoAnualPeriodoEscolar;
        private readonly IRepositorioPlanejamentoAnualComponente repositorioPlanejamentoAnualComponente;
        private readonly IRepositorioPlanejamentoAnualObjetivosAprendizagem repositorioPlanejamentoAnualObjetivosAprendizagem;

        public SalvarPlanejamentoAnualCommandHandler(IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual,
                                                     IRepositorioPlanejamentoAnualPeriodoEscolar repositorioPlanejamentoAnualPeriodoEscolar,
                                                     IRepositorioPlanejamentoAnualComponente repositorioPlanejamentoAnualComponente,
                                                     IRepositorioPlanejamentoAnualObjetivosAprendizagem repositorioPlanejamentoAnualObjetivosAprendizagem)
        {
            this.repositorioPlanejamentoAnual = repositorioPlanejamentoAnual ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnual));
            this.repositorioPlanejamentoAnualPeriodoEscolar = repositorioPlanejamentoAnualPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualPeriodoEscolar));
            this.repositorioPlanejamentoAnualComponente = repositorioPlanejamentoAnualComponente ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualComponente));
            this.repositorioPlanejamentoAnualObjetivosAprendizagem = repositorioPlanejamentoAnualObjetivosAprendizagem ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualObjetivosAprendizagem));
        }
        public async Task<AuditoriaDto> Handle(SalvarPlanejamentoAnualCommand comando, CancellationToken cancellationToken)
        {
            var planejamentoAnual = await repositorioPlanejamentoAnual.ObterPlanejamentoSimplificadoPorTurmaEComponenteCurricular(comando.TurmaId, comando.ComponenteCurricularId);
            if (planejamentoAnual == null)
            {
                planejamentoAnual = new PlanejamentoAnual(comando.TurmaId, comando.ComponenteCurricularId);
                await repositorioPlanejamentoAnual.SalvarAsync(planejamentoAnual);

                foreach (var periodo in comando.PeriodosEscolares)
                {
                    var planejamentoPeriodoEscolar = new PlanejamentoAnualPeriodoEscolar(periodo.PeriodoEscolarId)
                    {
                        PlanejamentoAnualId = planejamentoAnual.Id
                    };

                    await repositorioPlanejamentoAnualPeriodoEscolar.SalvarAsync(planejamentoPeriodoEscolar);

                    var planejamentoAnualComponentes = new List<PlanejamentoAnualComponente>();
                    foreach (var componente in periodo.Componentes)
                    {
                        var planejamentoAnualComponente = new PlanejamentoAnualComponente
                        {
                            ComponenteCurricularId = componente.ComponenteCurricularId,
                            Descricao = componente.Descricao,
                            PlanejamentoAnualPeriodoEscolarId = planejamentoPeriodoEscolar.Id
                        };
                        //SALVAR COMPONENTES

                        var objetivos = componente.ObjetivosAprendizagemId.Select(c => new PlanejamentoAnualObjetivoAprendizagem
                        {
                            ObjetivoAprendizagemId = c,
                            PlanejamentoAnualComponenteId = planejamentoAnualComponente.Id
                        });

                        await repositorioPlanejamentoAnualComponente.SalvarAsync(planejamentoAnualComponente);
                        await Task.Run(() => repositorioPlanejamentoAnualObjetivosAprendizagem.SalvarVarios(objetivos, planejamentoAnualComponente.Id));
                    }
                }
            }
            else
            {
                foreach (var periodo in comando.PeriodosEscolares)
                {
                    var planejamentoAnualPeriodoEscolar = await repositorioPlanejamentoAnualPeriodoEscolar.ObterPorPlanejamentoAnualId(planejamentoAnual.Id, periodo.PeriodoEscolarId);
                    if (planejamentoAnualPeriodoEscolar == null)
                    {
                        planejamentoAnualPeriodoEscolar = new PlanejamentoAnualPeriodoEscolar(periodo.PeriodoEscolarId)
                        {
                            PlanejamentoAnualId = planejamentoAnual.Id
                        };

                        await repositorioPlanejamentoAnualPeriodoEscolar.SalvarAsync(planejamentoAnualPeriodoEscolar);
                    }

                    await repositorioPlanejamentoAnualObjetivosAprendizagem.RemoverTodosPorPlanejamentoAnualPeriodoEscolarId(planejamentoAnualPeriodoEscolar.Id);

                    var componentes = periodo.Componentes.Select(c => new PlanejamentoAnualComponente
                    {
                        ComponenteCurricularId = c.ComponenteCurricularId,
                        Descricao = c.Descricao,
                        PlanejamentoAnualPeriodoEscolarId = planejamentoAnualPeriodoEscolar.Id,
                        ObjetivosAprendizagem = c.ObjetivosAprendizagemId.Select(o => new PlanejamentoAnualObjetivoAprendizagem
                        {
                            ObjetivoAprendizagemId = o
                        })?.ToList()
                    })?.ToList();

                    foreach (var componente in componentes)
                    {
                        var planejamentoAnualComponente = await repositorioPlanejamentoAnualComponente.ObterPorPlanejamentoAnualPeriodoEscolarId(componente.ComponenteCurricularId, planejamentoAnualPeriodoEscolar.Id);
                        if (planejamentoAnualComponente == null)
                        {
                            planejamentoAnualComponente = new PlanejamentoAnualComponente
                            {
                                ComponenteCurricularId = componente.ComponenteCurricularId,
                                PlanejamentoAnualPeriodoEscolarId = planejamentoAnualPeriodoEscolar.Id,
                            };
                        }

                        planejamentoAnualComponente.Descricao = componente.Descricao;
                        await repositorioPlanejamentoAnualComponente.SalvarAsync(planejamentoAnualComponente);
                        await Task.Run(() => repositorioPlanejamentoAnualObjetivosAprendizagem.SalvarVarios(componente.ObjetivosAprendizagem, planejamentoAnualComponente.Id));
                    }

                }
            }

            return (AuditoriaDto)planejamentoAnual;
        }
    }
}
