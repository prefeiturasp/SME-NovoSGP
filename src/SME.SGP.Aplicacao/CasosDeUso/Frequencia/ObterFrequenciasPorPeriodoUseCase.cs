﻿using Elasticsearch.Net.Specification.CatApi;
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
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            var turma = await ObterTurma(param.TurmaId);
            var alunosDaTurma = await mediator.Send(new ObterAlunosDentroPeriodoQuery(turma.CodigoTurma, (param.DataInicio, param.DataFim)));
            var componenteCurricular = await mediator.Send(new ObterComponenteCurricularPorIdQuery(componenteCurricularId));

            if (componenteCurricular.EhNulo())
                throw new NegocioException("Componente curricular não localizado");

            var disciplinaAula = componenteCurricular.Regencia && componenteCurricular.CdComponenteCurricularPai.NaoEhNulo() ?
                componenteCurricular.CdComponenteCurricularPai.ToString() :
                componenteCurricular.CodigoComponenteCurricular.ToString();

            var codigosComponentesBusca = new List<string>() { componenteCurricular.Regencia && componenteCurricular.CdComponenteCurricularPai.HasValue && componenteCurricular.CdComponenteCurricularPai.Value > 0 ? componenteCurricular.CdComponenteCurricularPai.ToString() : param.DisciplinaId };
          
            var aulas = await ObterAulas(param.DataInicio, param.DataFim, param.TurmaId, codigosComponentesBusca.ToArray(), usuarioLogado.EhSomenteProfessorCj());

            var tipoCalendarioId = await mediator.Send(new ObterTipoCalendarioIdPorTurmaQuery(turma));
            var periodoEscolar = await ObterPeriodoEscolar(tipoCalendarioId, param.DataInicio);

            var percentualCritico = await ObterParametro(TipoParametroSistema.PercentualFrequenciaCritico, turma.AnoLetivo);
            var percentualAlerta = await ObterParametro(TipoParametroSistema.PercentualFrequenciaAlerta, turma.AnoLetivo);
                        
            var registraFrequencia = await ObterComponenteRegistraFrequencia(codigosComponentesBusca.OrderBy(c => c.Length).Last());
            var frequenciaAlunos = await mediator.Send(new ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQuery(turma, codigosComponentesBusca.Select(c => long.Parse(c)).ToArray(), periodoEscolar.Id));
            var turmaPossuiFrequenciaRegistrada = await mediator.Send(new ExisteFrequenciaRegistradaPorTurmaComponenteCurricularQuery(turma.CodigoTurma, codigosComponentesBusca.ToArray(), periodoEscolar.Id));

            var registrosFrequenciaAlunos = await mediator.Send(new ObterRegistrosFrequenciaAlunosPorPeriodoQuery(param.TurmaId,
                                                                                                                  codigosComponentesBusca.ToArray(),
                                                                                                                  alunosDaTurma.Select(a => a.CodigoAluno).ToArray(),
                                                                                                                  param.DataInicio,
                                                                                                                  param.DataFim));

            var compensacaoAusenciaAlunoAulas =
                (registrosFrequenciaAlunos.Any() ? await mediator.Send(new ObterCompensacaoAusenciaAlunoAulaSimplificadoPorAulaIdsQuery(registrosFrequenciaAlunos.Select(t => t.AulaId).Distinct().ToArray())) : null) ??
                new List<CompensacaoAusenciaAlunoAulaSimplificadoDto>();

            var anotacoesTurma = await mediator.Send(new ObterAlunosComAnotacaoPorPeriodoQuery(param.TurmaId, param.DataInicio, param.DataFim));
            var frequenciaPreDefinida = await mediator.Send(new ObterFrequenciaPreDefinidaPorTurmaComponenteQuery(turma.Id, componenteCurricularId));

            
            return await mediator.Send(new ObterListaFrequenciaAulasQuery(turma,
                                                                          alunosDaTurma.OrderBy(a => a.NomeSocialAluno ?? a.NomeAluno),
                                                                          aulas,
                                                                          frequenciaAlunos.ToList(),
                                                                          registrosFrequenciaAlunos,
                                                                          anotacoesTurma,
                                                                          frequenciaPreDefinida,
                                                                          compensacaoAusenciaAlunoAulas,
                                                                          periodoEscolar,
                                                                          registraFrequencia,
                                                                          turmaPossuiFrequenciaRegistrada,
                                                                          param.DataInicio,
                                                                          param.DataFim,
                                                                          percentualAlerta,
                                                                          percentualCritico));
        }

        private async Task<bool> ObterComponenteRegistraFrequencia(string disciplinaId)
            => await mediator.Send(new ObterComponenteRegistraFrequenciaQuery(long.Parse(disciplinaId)));

        private async Task<int> ObterParametro(TipoParametroSistema parametro, int anoLetivo)
        {
            var parametroPercentual = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(parametro, anoLetivo));
            if (parametroPercentual.EhNulo() || string.IsNullOrEmpty(parametroPercentual.Valor))
                throw new NegocioException("Parâmetro de percentual de frequência em nível crítico/alerta não encontrado.");

            return int.Parse(parametroPercentual.Valor);
        }

        private async Task<PeriodoEscolar> ObterPeriodoEscolar(long tipoCalendarioId, DateTime dataInicio)
        {
            var periodoEscolar = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdEDataQuery(tipoCalendarioId, dataInicio));
            if (periodoEscolar.EhNulo())
                throw new NegocioException("Ocorreu um erro, esta aula está fora do período escolar.");

            return periodoEscolar;
        }

        private async Task<Turma> ObterTurma(string turmaId)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(turmaId));
            if (turma.EhNulo())
                throw new NegocioException("Não foi encontrada a turma informada.");

            return turma;
        }

        private async Task<IEnumerable<Aula>> ObterAulas(DateTime dataInicio, DateTime dataFim, string turmaId, string[] disciplinasId, bool aulaCJ)
        {
            var aulas = await mediator.Send(new ObterAulasPorDataPeriodoQuery(dataInicio, dataFim, turmaId, disciplinasId, aulaCJ));
            if (aulas.EhNulo() || !aulas.Any())
                throw new NegocioException("Aulas não encontradas para a turma no Período.");

            return aulas;
        }
    }
}
