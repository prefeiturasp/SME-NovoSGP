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
    public class MigrarPlanejamentoAnualCommandHandler : AbstractUseCase, IRequestHandler<MigrarPlanejamentoAnualCommand, bool>
    {
        private readonly IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IRepositorioPlanejamentoAnualPeriodoEscolar repositorioPlanejamentoAnualPeriodoEscolar;
        private readonly IRepositorioPlanejamentoAnualComponente repositorioPlanejamentoAnualComponente;
        private readonly IRepositorioPlanejamentoAnualObjetivosAprendizagem repositorioPlanejamentoAnualObjetivosAprendizagem;
        public MigrarPlanejamentoAnualCommandHandler(IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual, IMediator mediator,
            IRepositorioPlanejamentoAnualPeriodoEscolar repositorioPlanejamentoAnualPeriodoEscolar,
            IRepositorioTurma repositorioTurma,
            IRepositorioPlanejamentoAnualComponente repositorioPlanejamentoAnualComponente,
            IRepositorioPlanejamentoAnualObjetivosAprendizagem repositorioPlanejamentoAnualObjetivosAprendizagem
            ) : base(mediator)
        {
            this.repositorioPlanejamentoAnual = repositorioPlanejamentoAnual ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnual));
            this.repositorioTurma = repositorioTurma ?? throw new System.ArgumentNullException(nameof(repositorioTurma));
            this.repositorioPlanejamentoAnualPeriodoEscolar = repositorioPlanejamentoAnualPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualPeriodoEscolar));
            this.repositorioPlanejamentoAnualComponente = repositorioPlanejamentoAnualComponente ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualComponente));
            this.repositorioPlanejamentoAnualObjetivosAprendizagem = repositorioPlanejamentoAnualObjetivosAprendizagem ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualObjetivosAprendizagem));
        }

        public async Task<bool> Handle(MigrarPlanejamentoAnualCommand comando, CancellationToken cancellationToken)
        {
            var ehRegencia = await mediator.Send(new VerificarComponenteCurriculareSeERegenciaPorIdQuery(comando.Planejamento.ComponenteCurricularId));

            // Validando as turmas
            foreach (var turma in comando.Planejamento.TurmasDestinoIds)
            {
                var checarTurma = await repositorioTurma.ObterPorId(turma);
                if (checarTurma == null)
                    throw new NegocioException($"Turma não encontrada");

                foreach (var periodo in comando.Planejamento.PlanejamentoPeriodosEscolaresIds)
                {
                    var planejamentoAnualPeriodoEscolar = await repositorioPlanejamentoAnualPeriodoEscolar.ObterPorIdAsync(periodo);
                    if (planejamentoAnualPeriodoEscolar == null)
                        throw new NegocioException($"Período escolar não encontrado");

                    var planejamentoAnualComponente = await repositorioPlanejamentoAnualComponente.ObterPorPlanejamentoAnualPeriodoEscolarId(comando.Planejamento.ComponenteCurricularId, periodo);
                    if (planejamentoAnualComponente == null)
                        throw new NegocioException($"Componente não encontrado");

                    if (ehRegencia)
                    {
                        var turno = checarTurma.ModalidadeCodigo == Modalidade.Fundamental ? checarTurma.QuantidadeDuracaoAula : 0;

                        var componentes = await mediator.Send(new ObterComponentesCurricularesRegenciaPorAnoETurnoQuery(Convert.ToInt64(checarTurma.Ano), turno));

                        if (componentes.Any())
                        {
                            foreach (var componente in componentes)
                            {
                                var objetivos = await ObterObjetivos(componente.Id, planejamentoAnualPeriodoEscolar.Id);
                                await MigrarPlano(checarTurma, planejamentoAnualPeriodoEscolar, objetivos, planejamentoAnualComponente);
                            }
                        }
                    } 
                    else
                    {
                        var objetivos = await ObterObjetivos(comando.Planejamento.ComponenteCurricularId, planejamentoAnualPeriodoEscolar.Id);
                        await MigrarPlano(checarTurma, planejamentoAnualPeriodoEscolar, objetivos, planejamentoAnualComponente);
                    }
                }
            }
            return true;
        }

        private async Task MigrarPlano(Turma turma, PlanejamentoAnualPeriodoEscolar periodo, IEnumerable<PlanejamentoAnualObjetivoAprendizagem> objetivos, PlanejamentoAnualComponente planejamentoAnualComponente)
        {
            PlanejamentoAnual planejamentoAnualNovo = new PlanejamentoAnual(turma.Id, planejamentoAnualComponente.ComponenteCurricularId);
            var planejamentoAnualNovoId = await repositorioPlanejamentoAnual.SalvarAsync(planejamentoAnualNovo);

            PlanejamentoAnualPeriodoEscolar planejamentoAnualPeriodoEscolarNovo = new PlanejamentoAnualPeriodoEscolar(periodo.PeriodoEscolarId, planejamentoAnualNovoId);
            var planejamentoAnualPeriodoEscolarNovoId = await repositorioPlanejamentoAnualPeriodoEscolar.SalvarAsync(planejamentoAnualPeriodoEscolarNovo);

            PlanejamentoAnualComponente planejamentoAnualComponenteNovo = new PlanejamentoAnualComponente(planejamentoAnualComponente.ComponenteCurricularId, 
                planejamentoAnualComponente.Descricao, planejamentoAnualPeriodoEscolarNovoId);
            var planejamentoAnualComponenteNovoId = await repositorioPlanejamentoAnualComponente.SalvarAsync(planejamentoAnualComponenteNovo);

            if(objetivos.Any())
            {               
                repositorioPlanejamentoAnualObjetivosAprendizagem.SalvarVarios(objetivos, planejamentoAnualComponenteNovoId);
            }
        }


        private async Task<IEnumerable<PlanejamentoAnualObjetivoAprendizagem>> ObterObjetivos (long componenteCurricularId, long periodoId)
        {
            var planejamentoAnualComponente = await repositorioPlanejamentoAnualComponente.ObterPorPlanejamentoAnualPeriodoEscolarId(componenteCurricularId, periodoId);

            if (planejamentoAnualComponente != null)
            {
                var objetivos = await repositorioPlanejamentoAnualObjetivosAprendizagem.ObterPorPlanejamentoAnualComponenteId(planejamentoAnualComponente.Id);
                if (objetivos.Any())
                    return objetivos;
            }
            return null;
        }
    }
}
