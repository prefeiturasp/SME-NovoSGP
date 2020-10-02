using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPlanejamentoAnualCommandHandler : AbstractUseCase, IRequestHandler<SalvarPlanejamentoAnualCommand, PlanejamentoAnualAuditoriaDto>, 
    {
        private readonly IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual;
        private readonly IRepositorioPlanejamentoAnualPeriodoEscolar repositorioPlanejamentoAnualPeriodoEscolar;
        private readonly IRepositorioPlanejamentoAnualComponente repositorioPlanejamentoAnualComponente;
        private readonly IRepositorioPlanejamentoAnualObjetivosAprendizagem repositorioPlanejamentoAnualObjetivosAprendizagem;

        public SalvarPlanejamentoAnualCommandHandler(IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual,
                                                     IRepositorioPlanejamentoAnualPeriodoEscolar repositorioPlanejamentoAnualPeriodoEscolar,
                                                     IRepositorioPlanejamentoAnualComponente repositorioPlanejamentoAnualComponente,
                                                     IRepositorioPlanejamentoAnualObjetivosAprendizagem repositorioPlanejamentoAnualObjetivosAprendizagem, IMediator mediator) : base(mediator)
        {
            this.repositorioPlanejamentoAnual = repositorioPlanejamentoAnual ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnual));
            this.repositorioPlanejamentoAnualPeriodoEscolar = repositorioPlanejamentoAnualPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualPeriodoEscolar));
            this.repositorioPlanejamentoAnualComponente = repositorioPlanejamentoAnualComponente ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualComponente));
            this.repositorioPlanejamentoAnualObjetivosAprendizagem = repositorioPlanejamentoAnualObjetivosAprendizagem ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualObjetivosAprendizagem));
        }
        public async Task<PlanejamentoAnualAuditoriaDto> Handle(SalvarPlanejamentoAnualCommand comando, CancellationToken cancellationToken)
        {
            PlanejamentoAnualAuditoriaDto auditorias = new PlanejamentoAnualAuditoriaDto();
            var planejamentoAnual = await repositorioPlanejamentoAnual.ObterPlanejamentoSimplificadoPorTurmaEComponenteCurricular(comando.TurmaId, comando.ComponenteCurricularId);
            if (planejamentoAnual == null)
            {

                planejamentoAnual = new PlanejamentoAnual(comando.TurmaId, comando.ComponenteCurricularId);
                await repositorioPlanejamentoAnual.SalvarAsync(planejamentoAnual);
            }

            List<PeriodoEscolar> excecoesAtribuicao = new List<PeriodoEscolar>();
            List<PeriodoEscolar> excecoesEmAberto = new List<PeriodoEscolar>();
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(comando.TurmaId));
            if (turma == null)
                throw new NegocioException($"Turma de id [{turma.Id}] não localizada!");

            foreach (var periodoEscolar in comando.PeriodosEscolares)
            {
                var periodo = await mediator.Send(new ObterPeriodoEscolarePorIdQuery(periodoEscolar.PeriodoEscolarId));
                var temAtribuicao = await mediator.Send(new ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaNoPeriodoQuery(comando.ComponenteCurricularId, turma.CodigoTurma, usuario.CodigoRf, periodo.PeriodoInicio.Date, periodo.PeriodoFim.Date));
                if (!temAtribuicao)
                    excecoesAtribuicao.Add(periodo);

                var periodoEmAberto = mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTime.Today, periodo.Bimestre, turma.AnoLetivo == DateTime.Today.Year)).Result;
                if (!periodoEmAberto)
                    excecoesEmAberto.Add(periodo);
            }

            if (excecoesAtribuicao.Any())
                throw new NegocioException($"Você não possui atribuição.");

            if (excecoesEmAberto.Any())
                throw new NegocioException($"Algum bimestre não está com o período aberto.");

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
                else
                {
                    await repositorioPlanejamentoAnualObjetivosAprendizagem.RemoverTodosPorPlanejamentoAnualPeriodoEscolarId(planejamentoAnualPeriodoEscolar.Id);
                }

                var auditoria = new PlanejamentoAnualPeriodoEscolarDto
                {
                    PeriodoEscolarId = periodo.PeriodoEscolarId,
                };

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

                if (componentes != null)
                {
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
                        auditoria.Componentes.Add(new PlanejamentoAnualComponenteDto
                        {
                            Auditoria = (AuditoriaDto)planejamentoAnualComponente,
                            ComponenteCurricularId = componente.ComponenteCurricularId,
                        });

                        await Task.Run(() => repositorioPlanejamentoAnualObjetivosAprendizagem.SalvarVarios(componente.ObjetivosAprendizagem, planejamentoAnualComponente.Id));
                    }
                }
                auditorias.PeriodosEscolares.Add(auditoria);
            }
            return auditorias;
        }
    }
}
