using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosPorPeriodoPAPUseCase : AbstractUseCase, IObterAlunosPorPeriodoPAPUseCase
    {
        public ObterAlunosPorPeriodoPAPUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<AlunoDadosBasicosDto>> Executar(string codigoTurma, long periodoRelatorioPAPId)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(codigoTurma));
            var periodoEscolar = await ObterPeriodoEscolar(turma, periodoRelatorioPAPId);

            // Obtem lista de alunos da turma com dados basicos
            var dadosAlunos = await mediator.Send(new ObterDadosAlunosTurmaQuery(codigoTurma, turma.AnoLetivo, periodoEscolar));
            // Carrega lista de alunos com relatório já preenchido
            var alunosComRelatorio = await mediator.Send(new ObterCodigosDeAlunosComRelatorioPAPJaPreenchidoQuery(turma.Id, periodoRelatorioPAPId));
            var alunosCodigos = dadosAlunos.Select(x => x.CodigoEOL).ToArray();
            var matriculadosTurmaPAP = await mediator.Send(new ObterAlunosAtivosTurmaProgramaPapEolQuery(turma.AnoLetivo, alunosCodigos));
            await VerificaEstudantePossuiAtendimentoAEE(turma, dadosAlunos);
            VerificaEstudantePossuiMatriculaPap(matriculadosTurmaPAP, dadosAlunos);
            // Atuliza flag de processo concluido do aluno
            foreach (var dadosAluno in dadosAlunos.Where(d => alunosComRelatorio.Any(codigo => codigo == d.CodigoEOL)))
                dadosAluno.ProcessoConcluido = true;

            return dadosAlunos.OrderBy(w => w.Nome);
        }

        private async Task VerificaEstudantePossuiAtendimentoAEE(Dominio.Turma turma, IEnumerable<AlunoDadosBasicosDto> dadosAlunos)
        {
            foreach (var dadosAluno in dadosAlunos)
            {
                dadosAluno.EhAtendidoAEE = await mediator.Send(new VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery(dadosAluno.CodigoEOL, turma.AnoLetivo));
            }
        }

        private void VerificaEstudantePossuiMatriculaPap(IEnumerable<AlunosTurmaProgramaPapDto> matriculadosTurmaPap, IEnumerable<AlunoDadosBasicosDto> dadosAlunos)
        {
            foreach (var dadosAluno in dadosAlunos)
                dadosAluno.EhMatriculadoTurmaPAP = matriculadosTurmaPap.Any(x => x.CodigoAluno.ToString() == dadosAluno.CodigoEOL);
        }

        private int ObterBimestre(PeriodoRelatorioPAP periodoPAP)
        {
            const Char SEMESTRE = 'S';

            if (periodoPAP.Configuracao.TipoPeriocidade == SEMESTRE)
                return periodoPAP.Periodo * 2;

            return periodoPAP.Periodo;
        }

        private async Task<PeriodoEscolar> ObterPeriodoEscolar(Dominio.Turma turma, long periodoRelatorioPAPId)
        {
            var periodoPAP = await mediator.Send(new PeriodoRelatorioPAPQuery(periodoRelatorioPAPId));

            if (periodoPAP.EhNulo())
                throw new NegocioException("Período relatório PAP não localizado!");

            var bimestre = ObterBimestre(periodoPAP);

            var tipoCalendario = await mediator.Send(new ObterTipoCalendarioPorAnoLetivoEModalidadeQuery(turma.AnoLetivo, turma.ModalidadeTipoCalendario, periodoPAP.Periodo));
            if (tipoCalendario.EhNulo())
                throw new NegocioException("Tipo de Calendario não localizado para a turma!");

            var periodosEscolares = await mediator.Send(new ObterPeridosEscolaresPorTipoCalendarioIdQuery(tipoCalendario.Id));
            if (periodosEscolares.EhNulo() || !periodosEscolares.Any())
                throw new NegocioException("Não localizados periodos escolares para o calendario da turma!");

            var periodoAtual = periodosEscolares.FirstOrDefault(c => c.Bimestre == bimestre);
            if (periodoAtual.EhNulo())
                throw new NegocioException($"Não localizado periodo escolar!");

            return periodoAtual;
        }
    }
}
