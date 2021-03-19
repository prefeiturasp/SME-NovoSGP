using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAcompanhamentoAlunoUseCase : AbstractUseCase, IObterAcompanhamentoAlunoUseCase
    {
        public ObterAcompanhamentoAlunoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AcompanhamentoAlunoTurmaSemestreDto> Executar(FiltroAcompanhamentoTurmaAlunoSemestreDto filtro)
        {
            var turma = await ObterTurma(filtro.TurmaId);

            var acompanhamentosAlunoTurmaSemestre = await ObterAcompanhamentoSemestre(filtro.AlunoId, turma.Id, filtro.Semestre);

            var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery(turma.ModalidadeCodigo, turma.AnoLetivo, turma.Semestre));

            acompanhamentosAlunoTurmaSemestre.PodeEditar = VerificaSePodeEditarAcompanhamentoAluno(periodosEscolares);

            return acompanhamentosAlunoTurmaSemestre;
        }

        private async Task<Turma> ObterTurma(long turmaId)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(turmaId));

            if (turma == null)
                throw new NegocioException("Não foi possível localizar a turma informada!");

            return turma;
        }

        private async Task<AcompanhamentoAlunoTurmaSemestreDto> ObterAcompanhamentoSemestre(string alunoId, long turmaId, int semestre)
        {
            var acompanhamentoSemestre = await mediator.Send(new ObterAcompanhamentoPorAlunoTurmaESemestreQuery(alunoId, turmaId, semestre));

            return new AcompanhamentoAlunoTurmaSemestreDto()
            {
                AcompanhamentoAlunoId = acompanhamentoSemestre?.AcompanhamentoAlunoId ?? await ObterAcompanhamentoAluno(turmaId, alunoId),
                AcompanhamentoAlunoSemestreId = acompanhamentoSemestre?.Id ?? 0,
                Observacoes = acompanhamentoSemestre?.Observacoes,
                Auditoria = (AuditoriaDto)acompanhamentoSemestre
            };
        }

        private async Task<long> ObterAcompanhamentoAluno(long turmaId, string alunoId)
            => await mediator.Send(new ObterAcompanhamentoAlunoIDPorTurmaQuery(turmaId, alunoId));
         
        private bool VerificaSePodeEditarAcompanhamentoAluno(IEnumerable<PeriodoEscolar> periodosEscolares)
            => periodosEscolares.Any(a => a.PeriodoInicio <= DateTime.Today && a.PeriodoFim >= DateTime.Today);
    }
}
