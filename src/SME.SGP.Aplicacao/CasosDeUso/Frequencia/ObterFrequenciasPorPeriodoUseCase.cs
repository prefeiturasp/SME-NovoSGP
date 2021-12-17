using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciasPorPeriodoUseCase : AbstractUseCase, IObterFrequenciasPorPeriodoUseCase
    {
        public ObterFrequenciasPorPeriodoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<RegistroFrequenciaPorDataPeriodoDto> Executar(FiltroFrequenciaPorPeriodoDto param)
        {
            var componenteCurricularId = long.Parse(param.DisciplinaId);

            var turma = await ObterTurma(param.TurmaId);
            var alunosDaTurma = await ObterAlunos(param.TurmaId.ToString(), param.DataInicio);
            var aulas = await ObterAulas(param.DataInicio, param.DataFim, param.TurmaId, param.DisciplinaId);

            var tipoCalendarioId = await mediator.Send(new ObterTipoCalendarioIdPorTurmaQuery(turma));
            var periodoEscolar = await ObterPeriodoEscolar(tipoCalendarioId, param.DataInicio);

            var percentualCritico = await ObterParametro(TipoParametroSistema.PercentualFrequenciaCritico, turma.AnoLetivo);
            var percentualAlerta = await ObterParametro(TipoParametroSistema.PercentualFrequenciaAlerta, turma.AnoLetivo);

            var mesmoAnoLetivo = DateTime.Today.Year == turma.AnoLetivo;

            var registraFrequencia = await OterComponenteRegistraFrequencia(param.DisciplinaId);
            var frequenciaAlunos = await mediator.Send(new ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQuery(turma, componenteCurricularId, periodoEscolar.Id));
            var turmaPossuiFrequenciaRegistrada = await mediator.Send(new ExisteFrequenciaRegistradaPorTurmaComponenteCurricularQuery(turma.CodigoTurma, param.DisciplinaId, periodoEscolar.Id));
            var registrosFrequenciaAlunos = await mediator.Send(new ObterRegistrosFrequenciaAlunosPorPeriodoQuery(param.TurmaId,
                                                                                                                  param.DisciplinaId,
                                                                                                                  alunosDaTurma.Select(a => a.CodigoAluno).ToArray(),
                                                                                                                  param.DataInicio,
                                                                                                                  param.DataFim));

            var anotacoesTurma = await mediator.Send(new ObterAlunosComAnotacaoPorPeriodoQuery(param.TurmaId, param.DataInicio, param.DataFim));
            var frequenciaPreDefinida = await mediator.Send(new ObterFrequenciaPreDefinidaPorTurmaComponenteQuery(turma.Id, componenteCurricularId));

            return await mediator.Send(new ObterListaFrequenciaAulasQuery(turma,
                                                                          alunosDaTurma,
                                                                          aulas,
                                                                          frequenciaAlunos,
                                                                          registrosFrequenciaAlunos,
                                                                          anotacoesTurma,
                                                                          frequenciaPreDefinida,
                                                                          periodoEscolar,
                                                                          registraFrequencia,
                                                                          turmaPossuiFrequenciaRegistrada,
                                                                          param.DataInicio,
                                                                          param.DataFim,
                                                                          percentualAlerta,
                                                                          percentualCritico));
        }

        private async Task<bool> OterComponenteRegistraFrequencia(string disciplinaId)
            => await mediator.Send(new ObterComponenteRegistraFrequenciaQuery(long.Parse(disciplinaId)));

        private async Task<int> ObterParametro(TipoParametroSistema parametro, int anoLetivo)
        {
            var parametroPercentual = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(parametro, anoLetivo));
            if (parametroPercentual == null || string.IsNullOrEmpty(parametroPercentual.Valor))
                throw new NegocioException("Parâmetro de percentual de frequência em nível crítico/alerta não encontrado.");

            return int.Parse(parametroPercentual.Valor);
        }

        private async Task<PeriodoEscolar> ObterPeriodoEscolar(long tipoCalendarioId, DateTime dataInicio)
        {
            var periodoEscolar = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdEDataQuery(tipoCalendarioId, dataInicio));
            if (periodoEscolar == null)
                throw new NegocioException("Ocorreu um erro, esta aula está fora do período escolar.");

            return periodoEscolar;
        }

        private async Task<IEnumerable<AlunoPorTurmaResposta>> ObterAlunos(string turmaId, DateTime dataInicio)
        {
            var alunosDaTurma = await mediator.Send(new ObterAlunosPorTurmaEDataMatriculaQuery(turmaId, dataInicio));

            if (alunosDaTurma == null || !alunosDaTurma.Any())
                throw new NegocioException("Não foram encontrados alunos para a turma informada.");

            return alunosDaTurma;
        }

        private async Task<Turma> ObterTurma(string turmaId)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(turmaId));
            if (turma == null)
                throw new NegocioException("Não foi encontrada a turma informada.");

            return turma;
        }

        private async Task<IEnumerable<Aula>> ObterAulas(DateTime dataInicio, DateTime dataFim, string turmaId, string disciplinaId)
        {
            var aulas = await mediator.Send(new ObterAulasPorDataPeriodoQuery(dataInicio, dataFim, turmaId, disciplinaId));
            if (aulas == null)
                throw new NegocioException("Aulas não encontradas para a turma no Período.");

            return aulas;
        }
    }
}
