using MediatR;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarConsolidacaoTurmaFechamentoComponenteUseCase : AbstractUseCase, IExecutarConsolidacaoTurmaFechamentoComponenteUseCase
    {
        private readonly IRepositorioFechamentoConsolidado repositorioFechamentoConsolidado;

        public ExecutarConsolidacaoTurmaFechamentoComponenteUseCase(IMediator mediator, IRepositorioFechamentoConsolidado repositorioFechamentoConsolidado) : base(mediator)
        {
            this.repositorioFechamentoConsolidado = repositorioFechamentoConsolidado ?? throw new ArgumentNullException(nameof(repositorioFechamentoConsolidado));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<FechamentoConsolidacaoTurmaComponenteBimestreDto>();

            if (filtro == null)
            {
                SentrySdk.CaptureMessage($"Não foi possível iniciar a consolidação do fechamento da turma -> componente. O id da turma bimestre componente curricular não foram informados", SentryLevel.Error);
                return false;
            }

            var consolidadoTurmaComponente = await repositorioFechamentoConsolidado.ObterFechamentoConsolidadoPorTurmaBimestreComponenteCurricularAsync(filtro.TurmaId, filtro.ComponenteCurricularId, filtro.Bimestre);

            var fechamentos = await mediator.Send(new ObterFechamentosTurmaComponentesQuery(filtro.TurmaId, new long[] { filtro.ComponenteCurricularId }, filtro.Bimestre));

            var professoresDaTurma = await mediator.Send(new ObterProfessoresTitularesPorTurmaIdQuery(filtro.TurmaId));

            var fechamento = fechamentos?.FirstOrDefault();

            var atualizarConsolidado = false;

            if (fechamento != null && consolidadoTurmaComponente != null) 
                atualizarConsolidado = consolidadoTurmaComponente.Status != fechamento.Situacao;

            consolidadoTurmaComponente = MapearFechamentoConsolidado(filtro, consolidadoTurmaComponente, fechamento, professoresDaTurma);

            if (consolidadoTurmaComponente.Id == 0 || atualizarConsolidado)
                await repositorioFechamentoConsolidado.SalvarAsync(consolidadoTurmaComponente);

            return true;
        }

        private FechamentoConsolidadoComponenteTurma MapearFechamentoConsolidado(FechamentoConsolidacaoTurmaComponenteBimestreDto filtro, FechamentoConsolidadoComponenteTurma consolidadoTurmaComponente, FechamentoTurmaDisciplina fechamento, IEnumerable<Infra.ProfessorTitularDisciplinaEol> professoresDaTurma)
        {
            var statusFechamento = fechamento != null ? fechamento.Situacao : SituacaoFechamento.NaoIniciado;

            if (consolidadoTurmaComponente == null)
            {
                var professorComponente = professoresDaTurma.FirstOrDefault(p => p.DisciplinaId == filtro.ComponenteCurricularId);
               
                consolidadoTurmaComponente = new FechamentoConsolidadoComponenteTurma()
                {
                    Bimestre = filtro.Bimestre,
                    ComponenteCurricularCodigo = filtro.ComponenteCurricularId,
                    TurmaId = filtro.TurmaId,
                    ProfessorNome = professorComponente != null ? professorComponente.ProfessorNome : "Sem professor titular",
                    ProfessorRf = professorComponente?.ProfessorRf
                };
            }

            consolidadoTurmaComponente.DataAtualizacao = DateTime.Now;
            consolidadoTurmaComponente.Status = statusFechamento;

            return consolidadoTurmaComponente;
        }
    }
}
