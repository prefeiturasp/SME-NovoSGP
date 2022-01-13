using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

            if (periodo == null)
            {
                var mensagemErro = $"Não encontrado período escolar da aula [{request.AulaId}]";
                var aula = await mediator.Send(new ObterAulaPorIdQuery(request.AulaId));

                if (aula.TipoAula != TipoAula.Reposicao)
                    throw new NegocioException(mensagemErro);

                var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(request.TurmaCodigo));
                var eventoReposicaoAulaNoDia = await repositorioEvento
                    .EventosNosDiasETipo(aula.DataAula, aula.DataAula, TipoEvento.ReposicaoDoDia, aula.TipoCalendarioId, turma.Ue.CodigoUe, string.Empty);

                var eventoReposicaoDeAula = await repositorioEvento
                    .EventosNosDiasETipo(aula.DataAula, aula.DataAula, TipoEvento.ReposicaoDeAula, aula.TipoCalendarioId, turma.Ue.CodigoUe, string.Empty);

                if (eventoReposicaoAulaNoDia == null && eventoReposicaoDeAula == null)
                    throw new NegocioException(mensagemErro);

                var periodos = (await mediator.Send(new ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery(turma.ModalidadeCodigo, turma.AnoLetivo, turma.Semestre))).OrderBy(p => p.Bimestre);
                var periodoDeAcordo = aula.DataAula.Date < periodos.First().PeriodoInicio.Date ? periodos.First() : periodos.Last();

                periodo = new Infra.PeriodoEscolarInicioFimDto()
                {
                    Id = periodoDeAcordo.Id,
                    DataInicio = periodoDeAcordo.PeriodoInicio,
                    DataFim = periodoDeAcordo.PeriodoFim,
                    Bimestre = periodoDeAcordo.Bimestre
                };
            }

            return await mediator.Send(new IncluirFilaConciliacaoFrequenciaTurmaCommand(request.TurmaCodigo, periodo.Bimestre, request.ComponenteCurricularId, periodo.DataInicio, periodo.DataFim));
        }
    }
}
