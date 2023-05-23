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
        private readonly IConsultasPeriodoFechamento consultaPeriodoFechamento;

        public ObterAcompanhamentoAlunoUseCase(IMediator mediator, IConsultasPeriodoFechamento consultaPeriodoFechamento) : base(mediator)
        {
            this.consultaPeriodoFechamento = consultaPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultaPeriodoFechamento));
        }

        public async Task<AcompanhamentoAlunoTurmaSemestreDto> Executar(FiltroAcompanhamentoTurmaAlunoSemestreDto filtro)
        {
            var turmaCodigo = await mediator.Send(new ObterTurmaCodigoPorIdQuery(filtro.TurmaId));
            var turma = await ObterTurma(turmaCodigo);

            var acompanhamentoAlunoTurmaSemestre = await ObterAcompanhamentoSemestre(filtro.AlunoId, turma.Id, filtro.Semestre);

            var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery(turma.ModalidadeCodigo, turma.AnoLetivo, turma.Semestre));

            int bimestre = filtro.Semestre == 1 ? 2 : 4;

            acompanhamentoAlunoTurmaSemestre.PodeEditar = await PodeEditarTurma(turma, bimestre);

            TratamentoSemestre(acompanhamentoAlunoTurmaSemestre, periodosEscolares, filtro.Semestre, turma.ModalidadeCodigo);
            await TratamentoPercursoIndividual(acompanhamentoAlunoTurmaSemestre, filtro.TurmaId, filtro.AlunoId, filtro.ComponenteCurricularId);
            await ParametroQuantidadeFotosAluno(acompanhamentoAlunoTurmaSemestre, turma.AnoLetivo);

            return acompanhamentoAlunoTurmaSemestre;
        }

        private async Task<bool> PodeEditarTurma(Turma turma, int bimestre)
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia().Date;
            var tipoCalendarioId = await mediator.Send(new ObterTipoCalendarioIdPorTurmaQuery(turma));
            if(tipoCalendarioId == 0)
                throw new NegocioException($"Não foi possível obter o id do tipo calêndario para a turma : {turma.CodigoTurma}");
            
            var turmaEmPeriodoAberto = await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTime.Today, bimestre, false, tipoCalendarioId));

            if (turma.EhTurmaInfantil)
            {
                var periodosFechamento = await mediator.Send(new ObterPeriodosFechamentoTurmaInfantilCalendarioIdBimestreQuery(tipoCalendarioId, bimestre));
                if (periodosFechamento == null || !periodosFechamento.Any())
                    throw new NegocioException($"Não foi possível obter os periodos de fechamento do bimestre : {bimestre}");

                return (dataReferencia >= periodosFechamento.LastOrDefault().InicioDoFechamento.Date &&
                       dataReferencia <= periodosFechamento.LastOrDefault().FinalDoFechamento.Date)
                       || await consultaPeriodoFechamento.TurmaEmPeriodoDeFechamento(turma.CodigoTurma, dataReferencia, bimestre)
                       || turmaEmPeriodoAberto;
            }
            return await consultaPeriodoFechamento.TurmaEmPeriodoDeFechamento(turma.CodigoTurma, dataReferencia, bimestre);
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
            foreach (var registroIndividual in registrosIndividuais.OrderBy(a => a.DataRegistro))
            {
                percursoIndividual.AppendLine(registroIndividual.Registro);
            }

            acompanhamentoAlunoTurmaSemestre.TextoSugerido = true;
            acompanhamentoAlunoTurmaSemestre.PercursoIndividual = percursoIndividual.ToString();
        }

        private async Task ParametroQuantidadeFotosAluno(AcompanhamentoAlunoTurmaSemestreDto acompanhamentoAlunoTurmaSemestre, int anoLetivo)
        {
            var parametroQuantidade = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.QuantidadeImagensPercursoIndividualCrianca, anoLetivo));

            acompanhamentoAlunoTurmaSemestre.QuantidadeFotos = parametroQuantidade == null ? 3 :
                int.Parse(parametroQuantidade.Valor);
        }

        private void TratamentoSemestre(AcompanhamentoAlunoTurmaSemestreDto acompanhamentosAlunoTurmaSemestre, IEnumerable<PeriodoEscolar> periodosEscolares, int semestre, Modalidade modalidadeCodigo)
        {


            var periodosSemestre = modalidadeCodigo == Modalidade.EJA ?
                periodosEscolares :
                    semestre == 1 ?
                    periodosEscolares.Where(c => new int[] { 1, 2 }.Contains(c.Bimestre)) :
                    periodosEscolares.Where(c => new int[] { 3, 4 }.Contains(c.Bimestre));

            acompanhamentosAlunoTurmaSemestre.PeriodoInicio = periodosSemestre.Min(a => a.PeriodoInicio);
            acompanhamentosAlunoTurmaSemestre.PeriodoFim = periodosSemestre.Max(a => a.PeriodoFim);
        }

        private async Task<Turma> ObterTurma(string turmaCodigo)
        {            
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaCodigo));

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
    }
}
