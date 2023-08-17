using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class RecalcularFrequenciaPorTurmaCommandHandler : IRequestHandler<RecalcularFrequenciaPorTurmaCommand, bool>
    {
        private readonly IRepositorioAulaConsulta repositorioAula;
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IMediator mediator;

        public RecalcularFrequenciaPorTurmaCommandHandler(IRepositorioAulaConsulta repositorioAula, IRepositorioEvento repositorioEvento, IMediator mediator)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(RecalcularFrequenciaPorTurmaCommand request, CancellationToken cancellationToken)
        {
            var periodo = await repositorioAula.ObterPeriodoEscolarDaAula(request.AulaId);
            var aula = await mediator.Send(new ObterAulaPorIdQuery(request.AulaId), cancellationToken);
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(request.TurmaCodigo), cancellationToken);

            if (periodo == null)
            {
                var mensagemErro = $"Não encontrado período escolar da aula [{request.AulaId}]";

                if (aula.TipoAula != TipoAula.Reposicao)
                    throw new NegocioException(mensagemErro);

                
                var eventoReposicaoAulaNoDia = await repositorioEvento
                    .EventosNosDiasETipo(aula.DataAula, aula.DataAula, TipoEvento.ReposicaoDoDia, aula.TipoCalendarioId, turma.Ue.CodigoUe, string.Empty);

                var eventoReposicaoDeAula = await repositorioEvento
                    .EventosNosDiasETipo(aula.DataAula, aula.DataAula, TipoEvento.ReposicaoDeAula, aula.TipoCalendarioId, turma.Ue.CodigoUe, string.Empty);

                if (eventoReposicaoAulaNoDia == null && eventoReposicaoDeAula == null)
                    throw new NegocioException(mensagemErro);

                var periodos = (await mediator.Send(new ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery(turma.ModalidadeCodigo, turma.AnoLetivo, turma.Semestre), cancellationToken)).OrderBy(p => p.Bimestre);
                var periodoDeAcordo = aula.DataAula.Date < periodos.First().PeriodoInicio.Date ? periodos.First() : periodos.Last();

                periodo = new Infra.PeriodoEscolarInicioFimDto()
                {
                    Id = periodoDeAcordo.Id,
                    DataInicio = periodoDeAcordo.PeriodoInicio,
                    DataFim = periodoDeAcordo.PeriodoFim,
                    Bimestre = periodoDeAcordo.Bimestre
                };
            }

            var alunos = (await mediator.Send(new ObterAlunosPorTurmaQuery(request.TurmaCodigo, true), cancellationToken)).Select(c => c.CodigoAluno).Distinct();
            
            await mediator.Send(new IncluirFilaCalcularFrequenciaPorTurmaCommand(alunos, aula.DataAula, request.TurmaCodigo, request.ComponenteCurricularId, request.Meses), cancellationToken);
            
            await mediator.Send(new IncluirFilaConsolidacaoDiariaDashBoardFrequenciaCommand(turma.Id, aula.DataAula));
            
            await mediator.Send(new IncluirFilaConsolidacaoSemanalMensalDashBoardFrequenciaCommand(turma.Id, turma.CodigoTurma, turma.ModalidadeCodigo == Modalidade.EducacaoInfantil, turma.AnoLetivo, aula.DataAula));

            return await mediator.Send(new IncluirFilaConciliacaoFrequenciaTurmaCommand(request.TurmaCodigo, periodo.Bimestre, request.ComponenteCurricularId, periodo.DataInicio, periodo.DataFim), cancellationToken);
        }
    }
}
