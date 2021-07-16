using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            var acompanhamentoAlunoTurmaSemestre = await ObterAcompanhamentoSemestre(filtro.AlunoId, turma.Id, filtro.Semestre);

            var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery(turma.ModalidadeCodigo, turma.AnoLetivo, turma.Semestre));

            TratamentoSemestre(acompanhamentoAlunoTurmaSemestre, periodosEscolares, filtro.Semestre, turma.ModalidadeCodigo);
            await TratamentoPercursoIndividual(acompanhamentoAlunoTurmaSemestre, filtro.TurmaId, filtro.AlunoId, filtro.ComponenteCurricularId);
            await ParametroQuantidadeFotosAluno(acompanhamentoAlunoTurmaSemestre, turma.AnoLetivo);

            return acompanhamentoAlunoTurmaSemestre;
        }

        private async Task TratamentoPercursoIndividual(AcompanhamentoAlunoTurmaSemestreDto acompanhamentoAlunoTurmaSemestre, long turmaId, string alunoCodigo, long componenteCurricularId)
        {
            if (string.IsNullOrEmpty(acompanhamentoAlunoTurmaSemestre.PercursoIndividual))
                await CarregarSugestaoPercursoIndividual(acompanhamentoAlunoTurmaSemestre, turmaId, alunoCodigo, componenteCurricularId);
        }

        private async Task CarregarSugestaoPercursoIndividual(AcompanhamentoAlunoTurmaSemestreDto acompanhamentoAlunoTurmaSemestre, long turmaId, string alunoCodigo, long componenteCurricularId)
        {
            var percursoIndividual = new StringBuilder();
            var registrosIndividuais = await mediator.Send(new ObterDescricoesRegistrosIndividuaisPorPeriodoQuery(turmaId, long.Parse(alunoCodigo), componenteCurricularId, acompanhamentoAlunoTurmaSemestre.PeriodoInicio, acompanhamentoAlunoTurmaSemestre.PeriodoFim));
            foreach(var registroIndividual in registrosIndividuais.OrderBy(a => a.DataRegistro))
            {
                percursoIndividual.AppendLine(registroIndividual.Registro);
            }

            acompanhamentoAlunoTurmaSemestre.PercursoIndividual = percursoIndividual.ToString();
        }

        private async Task ParametroQuantidadeFotosAluno(AcompanhamentoAlunoTurmaSemestreDto acompanhamentoAlunoTurmaSemestre, int anoLetivo)
        {
            var parametroQuantidade = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.QuantidadeFotosAcompanhamentoAluno, anoLetivo));

            acompanhamentoAlunoTurmaSemestre.QuantidadeFotos = parametroQuantidade == null ? 3 :
                int.Parse(parametroQuantidade.Valor);
        }

        private void TratamentoSemestre(AcompanhamentoAlunoTurmaSemestreDto acompanhamentosAlunoTurmaSemestre, IEnumerable<PeriodoEscolar> periodosEscolares, int semestre, Modalidade modalidadeCodigo)
        {
            acompanhamentosAlunoTurmaSemestre.PodeEditar = VerificaSePodeEditarAcompanhamentoAluno(periodosEscolares);

            var periodosSemestre = modalidadeCodigo == Modalidade.EJA ?
                periodosEscolares :
                    semestre == 1 ?
                    periodosEscolares.Where(c => new int[] { 1, 2 }.Contains(c.Bimestre)) :
                    periodosEscolares.Where(c => new int[] { 3, 4 }.Contains(c.Bimestre));

            acompanhamentosAlunoTurmaSemestre.PeriodoInicio = periodosSemestre.Min(a => a.PeriodoInicio);
            acompanhamentosAlunoTurmaSemestre.PeriodoFim = periodosSemestre.Max(a => a.PeriodoFim);
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
                PercursoIndividual = acompanhamentoSemestre?.PercursoIndividual,
                Auditoria = (AuditoriaDto)acompanhamentoSemestre
            };
        }

        private async Task<long> ObterAcompanhamentoAluno(long turmaId, string alunoId)
            => await mediator.Send(new ObterAcompanhamentoAlunoIDPorTurmaQuery(turmaId, alunoId));
         
        private bool VerificaSePodeEditarAcompanhamentoAluno(IEnumerable<PeriodoEscolar> periodosEscolares)
            => periodosEscolares.Any(a => a.PeriodoInicio <= DateTime.Today && a.PeriodoFim >= DateTime.Today);
    }
}
