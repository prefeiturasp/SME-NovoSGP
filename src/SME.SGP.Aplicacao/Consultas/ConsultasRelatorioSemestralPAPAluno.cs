using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using SME.SGP.Dominio.Interfaces;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ConsultasRelatorioSemestralPAPAluno : ConsultasBase, IConsultasRelatorioSemestralPAPAluno
    {
        private readonly IConsultasTurma consultasTurma;
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private readonly IConsultasTipoCalendario consultasTipoCalendario;
        private readonly IRepositorioRelatorioSemestralPAPAluno repositorioRelatorioSemestralAluno;
        private readonly IMediator mediator;

        public ConsultasRelatorioSemestralPAPAluno(IContextoAplicacao contextoAplicacao,
                                                IConsultasTurma consultasTurma,
                                                IConsultasPeriodoEscolar consultasPeriodoEscolar,
                                                IConsultasTipoCalendario consultasTipoCalendario,
                                                IRepositorioRelatorioSemestralPAPAluno repositorioRelatorioSemestralAluno,
                                                IMediator mediator) : base(contextoAplicacao)
        {
            this.consultasTurma = consultasTurma ?? throw new ArgumentNullException(nameof(consultasTurma));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.consultasTipoCalendario = consultasTipoCalendario ?? throw new ArgumentNullException(nameof(consultasTipoCalendario));
            this.repositorioRelatorioSemestralAluno = repositorioRelatorioSemestralAluno ?? throw new ArgumentNullException(nameof(repositorioRelatorioSemestralAluno));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<AlunoDadosBasicosDto>> ObterListaAlunosAsync(string turmaCodigo, int anoLetivo, int semestre)
        {
            var turma = await consultasTurma.ObterPorCodigo(turmaCodigo);
            var periodoEscolar = await ObterPeriodoEscolar(turma, semestre);

            // Obtem lista de alunos da turma com dados basicos
            var dadosAlunos = await consultasTurma.ObterDadosAlunos(turmaCodigo, anoLetivo, periodoEscolar);
            // Carrega lista de alunos com relatório já preenchido
            var alunosComRelatorio = await ObterRelatoriosAlunosPorTurmaAsync(turma.Id, semestre);
            
            await VerificaEstudantePossuiAtendimentoAEE(turma, dadosAlunos);

            // Atuliza flag de processo concluido do aluno
            foreach (var dadosAluno in dadosAlunos.Where(d => alunosComRelatorio.Any(a => a.AlunoCodigo == d.CodigoEOL)))
                dadosAluno.ProcessoConcluido = true;

            return dadosAlunos.OrderBy(w => w.Nome);
        }

        private async Task VerificaEstudantePossuiAtendimentoAEE(Turma turma, IEnumerable<AlunoDadosBasicosDto> dadosAlunos)
        {
            foreach (var dadosAluno in dadosAlunos)
            {
                dadosAluno.EhAtendidoAEE = await mediator.Send(new VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery(dadosAluno.CodigoEOL, turma.AnoLetivo));
            }
        }

        private async Task<PeriodoEscolar> ObterPeriodoEscolar(Turma turma, int semestre)
        {
            var tipoCalendario = await consultasTipoCalendario.BuscarPorAnoLetivoEModalidade(turma.AnoLetivo, turma.ModalidadeTipoCalendario, turma.Semestre);
            if (tipoCalendario == null)
                throw new NegocioException("Tipo de Calendario não localizado para a turma!");

            var periodosEscolares = await consultasPeriodoEscolar.ObterPeriodosEscolares(tipoCalendario.Id);
            if (periodosEscolares == null || !periodosEscolares.Any())
                throw new NegocioException("Não localizados periodos escolares para o calendario da turma!");

            var periodoAtual = periodosEscolares.FirstOrDefault(c => c.Bimestre == semestre * 2);
            if (periodoAtual == null)
                throw new NegocioException($"Não localizado periodo escolar para o semestre {semestre}!");

            return periodoAtual;
        }

        public Task<RelatorioSemestralPAPAluno> ObterPorTurmaAlunoAsync(long relatorioSemestralId, string alunoCodigo)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<RelatorioSemestralPAPAluno>> ObterRelatoriosAlunosPorTurmaAsync(long turmaId, int semestre)
            => await repositorioRelatorioSemestralAluno.ObterRelatoriosAlunosPorTurmaAsync(turmaId, semestre);

    }   
}
