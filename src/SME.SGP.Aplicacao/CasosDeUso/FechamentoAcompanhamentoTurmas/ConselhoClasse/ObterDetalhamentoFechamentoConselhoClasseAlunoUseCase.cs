using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.AreaDoConhecimento;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDetalhamentoFechamentoConselhoClasseAlunoUseCase : AbstractUseCase, IObterDetalhamentoFechamentoConselhoClasseAlunoUseCase
    {
        private readonly IConsultasFrequencia consultasFrequencia;
        private readonly IObterAreasConhecimentoUseCase obterAreasConhecimentoUseCase;
        private readonly IObterOrdenacaoAreasConhecimentoUseCase obterOrdenacaoAreasConhecimentoUseCase;
        private readonly IMapearAreasDoConhecimentoUseCase mapearAreasDoConhecimentoUseCase;
        private readonly IObterComponentesDasAreasDeConhecimentoUseCase obterComponentesDasAreasDeConhecimentoUseCase;

        public ObterDetalhamentoFechamentoConselhoClasseAlunoUseCase(IMediator mediator, 
                                                                     IConsultasFrequencia consultasFrequencia,
                                                                     IObterAreasConhecimentoUseCase obterAreasConhecimentoUseCase,
                                                                     IObterOrdenacaoAreasConhecimentoUseCase obterOrdenacaoAreasConhecimentoUseCase,
                                                                     IMapearAreasDoConhecimentoUseCase mapearAreasDoConhecimentoUseCase,
                                                                     IObterComponentesDasAreasDeConhecimentoUseCase obterComponentesDasAreasDeConhecimentoUseCase) : base(mediator)
        {
            this.consultasFrequencia = consultasFrequencia ?? throw new ArgumentNullException(nameof(consultasFrequencia));
            this.obterAreasConhecimentoUseCase = obterAreasConhecimentoUseCase ?? throw new ArgumentNullException(nameof(obterAreasConhecimentoUseCase));
            this.obterOrdenacaoAreasConhecimentoUseCase = obterOrdenacaoAreasConhecimentoUseCase ?? throw new ArgumentNullException(nameof(obterOrdenacaoAreasConhecimentoUseCase));
            this.mapearAreasDoConhecimentoUseCase = mapearAreasDoConhecimentoUseCase ?? throw new ArgumentNullException(nameof(mapearAreasDoConhecimentoUseCase));
            this.obterComponentesDasAreasDeConhecimentoUseCase = obterComponentesDasAreasDeConhecimentoUseCase ?? throw new ArgumentNullException(nameof(obterComponentesDasAreasDeConhecimentoUseCase));
        }

        public async Task<IEnumerable<DetalhamentoComponentesCurricularesAlunoDto>> Executar(FiltroConselhoClasseConsolidadoDto filtro)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(filtro.TurmaId));

            var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaCodigoBimestreQuery(turma.CodigoTurma, filtro.Bimestre));
            var conselhoClasse = fechamentoTurma != null ? await mediator.Send(new ObterConselhoClassePorFechamentoIdQuery(fechamentoTurma.Id)) : null;

            var bimestre = filtro.Bimestre;
            var periodoEscolar = await mediator.Send(new ObterPeriodoEscolarPorTurmaBimestreQuery(turma, bimestre));

            var tipoCalendarioId = await mediator.Send(new ObterTipoCalendarioIdPorTurmaQuery(turma));
            if (tipoCalendarioId <= 0) throw new NegocioException("Tipo de calendário não encontrado");

            string[] turmasCodigos;
            long[] conselhosClassesIds;

            if (turma.DeveVerificarRegraRegulares())
            {
                List<TipoTurma> turmasCodigosParaConsulta = new List<TipoTurma>() { turma.TipoTurma };
                turmasCodigosParaConsulta.AddRange(turma.ObterTiposRegularesDiferentes());
                turmasCodigos = await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(turma.AnoLetivo, filtro.AlunoCodigo, turmasCodigosParaConsulta));
                conselhosClassesIds = await mediator.Send(new ObterConselhoClasseIdsPorTurmaEPeriodoQuery(turmasCodigos, periodoEscolar?.Id));
            }
            else
            {
                turmasCodigos = new string[1] { turma.CodigoTurma };
                conselhosClassesIds = new long[1] { conselhoClasse.Id };
            }

            var notasConselhoClasseAluno = new List<NotaConceitoBimestreComponenteDto>();

            if (conselhosClassesIds != null)
            {
                foreach (var conselhosClassesId in conselhosClassesIds)
                {
                    var notasParaAdicionar = await mediator.Send(new ObterConselhoClasseNotasAlunoQuery(conselhosClassesId, filtro.AlunoCodigo));
                    notasConselhoClasseAluno.AddRange(notasParaAdicionar);
                }
            }

            List<Turma> turmas;

            if (turmasCodigos.Length > 0)
                turmas = (await mediator.Send(new ObterTurmasPorCodigosQuery(turmasCodigos))).ToList();
            else turmas = new List<Turma>() { turma };

            //Verificar as notas finais
            var notasFechamentoAluno = fechamentoTurma != null && fechamentoTurma.PeriodoEscolarId.HasValue ?
               await mediator.Send(new ObterNotasFechamentosPorTurmasCodigosBimestreQuery(turmasCodigos, filtro.AlunoCodigo, bimestre)) :
               await mediator.Send(new ObterNotasFinaisBimestresAlunoQuery(turmasCodigos, filtro.AlunoCodigo, bimestre));

            Usuario usuarioAtual = await mediator.Send(new ObterUsuarioLogadoQuery());

            var componentesCurricularesDaTurmaEol = await mediator.Send(new ObterComponentesCurricularesPorTurmasCodigoQuery(turmasCodigos, usuarioAtual.PerfilAtual, usuarioAtual.Login, turma.EnsinoEspecial, turma.TurnoParaComponentesCurriculares));

            var componentesCurricularesDaTurma = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(componentesCurricularesDaTurmaEol.Select(x => x.CodigoComponenteCurricular).Distinct().ToArray()));

            var retorno = new List<DetalhamentoComponentesCurricularesAlunoDto>();

            var areasDoConhecimento = await obterAreasConhecimentoUseCase.Executar(componentesCurricularesDaTurma);

            var ordenacaoGrupoArea = await obterOrdenacaoAreasConhecimentoUseCase.Executar((componentesCurricularesDaTurma, areasDoConhecimento));

            var frequenciasAluno = await ObterFrequenciaAlunoRefatorada(componentesCurricularesDaTurmaEol, periodoEscolar, filtro.AlunoCodigo, tipoCalendarioId, bimestre);

            var gruposMatrizes = componentesCurricularesDaTurma.Where(c => c.GrupoMatrizNome != null).OrderBy(d => d.GrupoMatrizId).GroupBy(c => c.GrupoMatrizId).ToList();

            foreach (var grupoComponentesCurricularesMatriz in gruposMatrizes)
            {
                var areasConhecimento = mapearAreasDoConhecimentoUseCase.MapearAreasDoConhecimento(grupoComponentesCurricularesMatriz, areasDoConhecimento, ordenacaoGrupoArea, grupoComponentesCurricularesMatriz.Key);

                foreach (var areaConhecimento in areasConhecimento)
                {
                    var componentes = obterComponentesDasAreasDeConhecimentoUseCase.ObterComponentesDasAreasDeConhecimento(grupoComponentesCurricularesMatriz, areaConhecimento);

                    foreach (var componenteCurricular in componentes.OrderBy(g => g.Nome))
                    {
                        var frequenciasAlunoParaTratar = frequenciasAluno.Where(a => a.DisciplinaId == componenteCurricular.Id.ToString());
                        FrequenciaAluno frequenciaAluno;

                        if (frequenciasAlunoParaTratar == null || !frequenciasAlunoParaTratar.Any())
                        {
                            frequenciaAluno = new FrequenciaAluno() { DisciplinaId = componenteCurricular.Id.ToString(), TurmaId = componenteCurricular.TurmaCodigo };
                        }
                        else if (frequenciasAlunoParaTratar.Count() == 1)
                        {
                            frequenciaAluno = frequenciasAlunoParaTratar.FirstOrDefault();
                        }
                        else
                        {
                            frequenciaAluno = new FrequenciaAluno()
                            {
                                DisciplinaId = componenteCurricular.CodigoComponenteCurricular.ToString(),
                                CodigoAluno = filtro.AlunoCodigo
                            };


                            frequenciaAluno.TotalAulas = frequenciasAlunoParaTratar.Sum(a => a.TotalAulas);
                            frequenciaAluno.TotalAusencias = frequenciasAlunoParaTratar.Sum(a => a.TotalAusencias);
                            frequenciaAluno.TotalCompensacoes = frequenciasAlunoParaTratar.Sum(a => a.TotalCompensacoes);

                        }

                        if (componenteCurricular.Regencia)
                        {
                            retorno.AddRange(await ObterNotasFrequenciaRegencia(componenteCurricular.CodigoComponenteCurricular,
                                                                                frequenciaAluno,
                                                                                periodoEscolar,
                                                                                turma,
                                                                                notasConselhoClasseAluno,
                                                                                notasFechamentoAluno));
                        }
                        else
                            retorno.Add(await ObterNotasFrequenciaComponente(componenteCurricular,
                                                                        frequenciaAluno,
                                                                        periodoEscolar,
                                                                        turma,
                                                                        notasConselhoClasseAluno,
                                                                        notasFechamentoAluno, filtro.Bimestre));
                    }
                }
            }

            return retorno;
        }       

        private async Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaAlunoRefatorada(IEnumerable<DisciplinaDto> componenteCurricularesDaTurma, PeriodoEscolar periodoEscolar, string alunoCodigo,
            long tipoCalendarioId, int bimestre)
        {
            var frequenciasAlunoRetorno = new List<FrequenciaAluno>();


            var componentesId = componenteCurricularesDaTurma.Select(a => a.CodigoComponenteCurricular.ToString()).Distinct().ToArray();
            var turmasCodigo = componenteCurricularesDaTurma.Select(a => a.TurmaCodigo).Distinct().ToArray();

            int[] bimestres;
            if (periodoEscolar == null)
            {
                var periodosEscolaresTurma = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdQuery(tipoCalendarioId));
                if (periodosEscolaresTurma.Any())
                {
                    bimestres = periodosEscolaresTurma.Select(a => a.Bimestre).ToArray();
                }
                else throw new NegocioException("Não foi possível localizar os períodos escolares da turma");
            }
            else bimestres = new int[] { bimestre };

            var frequenciasAluno = await mediator.Send(new ObterFrequenciasAlunosPorCodigoAlunoCodigoComponentesTurmaQuery(alunoCodigo, turmasCodigo, componentesId));
            frequenciasAluno = frequenciasAluno.Where(f => f.Bimestre == bimestre);

            var aulasComponentesTurmas = await mediator.Send(new ObterAulasDadasTurmaEBimestreEComponenteCurricularQuery(turmasCodigo, tipoCalendarioId, componentesId, bimestres));

            if (frequenciasAluno != null && frequenciasAluno.Any())
                frequenciasAlunoRetorno.AddRange(frequenciasAluno);

            foreach (var aulaComponenteTurma in aulasComponentesTurmas)
            {
                if (!frequenciasAlunoRetorno.Any(a => a.TurmaId == aulaComponenteTurma.TurmaCodigo && a.DisciplinaId == aulaComponenteTurma.ComponenteCurricularCodigo && a.Bimestre == aulaComponenteTurma.Bimestre))
                {
                    frequenciasAlunoRetorno.Add(new FrequenciaAluno()
                    {
                        CodigoAluno = alunoCodigo,
                        DisciplinaId = aulaComponenteTurma.ComponenteCurricularCodigo,
                        TurmaId = aulaComponenteTurma.TurmaCodigo,
                        TotalAulas = aulaComponenteTurma.AulasQuantidade,
                        Bimestre = aulaComponenteTurma.Bimestre
                    });
                }
            }

            return frequenciasAlunoRetorno;
        }

        private async Task<IEnumerable<DetalhamentoComponentesCurricularesAlunoDto>> ObterNotasFrequenciaRegencia(long componenteCurricularCodigo, FrequenciaAluno frequenciaAluno, PeriodoEscolar periodoEscolar, Turma turma, IEnumerable<NotaConceitoBimestreComponenteDto> notasConselhoClasseAluno,
            IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno)
        {
            var componentesRegencia = await mediator.Send(new ObterComponentesCurricularesRegenciaPorTurmaQuery(turma, componenteCurricularCodigo));

            if (componentesRegencia == null || !componentesRegencia.Any())
                throw new NegocioException("Não foram encontrados componentes curriculares para a regência informada.");

            var percentualFrequencia = (frequenciaAluno.TotalAulas > 0 ? frequenciaAluno?.PercentualFrequencia ?? 100 : 100);

            // Cálculo de frequência particular do ano de 2020
            if (periodoEscolar == null && turma.AnoLetivo.Equals(2020))
                percentualFrequencia = frequenciaAluno.PercentualFrequenciaFinal;

            var lstDetalhesNotas = new List<DetalhamentoComponentesCurricularesAlunoDto>();

            foreach (var componenteRegencia in componentesRegencia)
                lstDetalhesNotas.Add(ObterNotasRegencia(componenteRegencia.Descricao, componenteRegencia.Codigo, periodoEscolar, notasConselhoClasseAluno, notasFechamentoAluno, frequenciaAluno?.TotalAusencias, frequenciaAluno?.TotalCompensacoes, percentualFrequencia));

            return lstDetalhesNotas;
        }

        private async Task<DetalhamentoComponentesCurricularesAlunoDto> ObterNotasFrequenciaComponente(DisciplinaDto componenteCurricular, 
            FrequenciaAluno frequenciaAluno, 
            PeriodoEscolar periodoEscolar, Turma turma, IEnumerable<NotaConceitoBimestreComponenteDto> notasConselhoClasseAluno, 
            IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno, int bimestre)
        {
            var percentualFrequencia = (frequenciaAluno.TotalAulas > 0 ? frequenciaAluno?.PercentualFrequencia ?? 100 : 100);

            // Cálculo de frequência particular do ano de 2020
            if (periodoEscolar == null && turma.AnoLetivo.Equals(2020))
                percentualFrequencia = frequenciaAluno.PercentualFrequenciaFinal;

            var notasFechamento = ObterNotasComponente(componenteCurricular.CodigoComponenteCurricular, periodoEscolar, notasFechamentoAluno);
            var notaPosConselho = ObterNotaPosConselho(componenteCurricular.CodigoComponenteCurricular, periodoEscolar?.Bimestre, notasConselhoClasseAluno, notasFechamentoAluno);

            var parecerFinal = bimestre == 0 ? await consultasFrequencia.ObterSinteseAluno(percentualFrequencia, componenteCurricular) : null;

            var notaFechamento = !componenteCurricular.LancaNota ? parecerFinal?.Valor : (notasFechamento != null && notasFechamento.Any() &&
                                 notasFechamento.FirstOrDefault().NotaConceito != null ? String.Format("{0:0.0}", notasFechamento.FirstOrDefault().NotaConceito) : null);

            var conselhoClasseComponente = new DetalhamentoComponentesCurricularesAlunoDto()
            {
                LancaNota = componenteCurricular.LancaNota,
                NomeComponenteCurricular = componenteCurricular.Nome,
                QuantidadeAusencia = frequenciaAluno?.TotalAusencias ?? 0,
                QuantidadeCompensacoes = frequenciaAluno?.TotalCompensacoes ?? 0,
                PercentualFrequencia = percentualFrequencia,
                NotaFechamento = notaFechamento,
                NotaPosConselho = notaPosConselho != null && notaPosConselho?.Nota != null ? String.Format("{0:0.0}", notaPosConselho.Nota) : null
            };

            return conselhoClasseComponente;
        }

        private DetalhamentoComponentesCurricularesAlunoDto ObterNotasRegencia(string componenteCurricularNome, long componenteCurricularCodigo, PeriodoEscolar periodoEscolar, IEnumerable<NotaConceitoBimestreComponenteDto> notasConselhoClasseAluno, IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno, int? totalAusencias, int? totalCompensacoes, double percentualFrequencia)
        {
            var notasFechamento = ObterNotasComponente(componenteCurricularCodigo, periodoEscolar, notasFechamentoAluno);
            var notaPosConselho = ObterNotaPosConselho(componenteCurricularCodigo, periodoEscolar?.Bimestre, notasConselhoClasseAluno, notasFechamentoAluno);

            return new DetalhamentoComponentesCurricularesAlunoDto()
            {
                QuantidadeAusencia = totalAusencias ?? 0,
                QuantidadeCompensacoes = totalCompensacoes ?? 0,
                PercentualFrequencia = percentualFrequencia,
                NomeComponenteCurricular = componenteCurricularNome,
                NotaFechamento = notasFechamento != null && notasFechamento.Any() &&
                                 notasFechamento.FirstOrDefault().NotaConceito != null ? String.Format("{0:0.0}", notasFechamento.FirstOrDefault().NotaConceito) : null,
                NotaPosConselho = notaPosConselho != null && notaPosConselho.Nota != null ? String.Format("{0:0.0}", notaPosConselho.Nota) : null
            };
        }

        private List<NotaBimestreDto> ObterNotasComponente(long componenteCurricularCodigo, PeriodoEscolar periodoEscolar, IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno)
        {
            var notasFinais = new List<NotaBimestreDto>();

            if (periodoEscolar != null)
                notasFinais.Add(ObterNotaFinalComponentePeriodo(componenteCurricularCodigo, periodoEscolar.Bimestre, notasFechamentoAluno));
            else
                notasFinais.AddRange(ObterNotasFinaisComponentePeriodos(componenteCurricularCodigo, notasFechamentoAluno));

            return notasFinais;
        }

        private NotaPosConselhoDto ObterNotaPosConselho(long componenteCurricularCodigo, int? bimestre, IEnumerable<NotaConceitoBimestreComponenteDto> notasConselhoClasseAluno, IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno)
        {
            // Busca nota do conselho de classe consultado
            var notaComponente = notasConselhoClasseAluno.FirstOrDefault(c => c.ComponenteCurricularCodigo == componenteCurricularCodigo);
            if (notaComponente == null)
            {
                // Sugere nota final do fechamento
                var notaComponenteComConselhoNota = notasFechamentoAluno.FirstOrDefault(c => c.ComponenteCurricularCodigo == componenteCurricularCodigo && c.Bimestre == bimestre && c.ConselhoClasseNotaId > 0);
                if (notaComponenteComConselhoNota != null) notaComponente = notaComponenteComConselhoNota;
                else
                    notaComponente = notasFechamentoAluno.FirstOrDefault(c => c.ComponenteCurricularCodigo == componenteCurricularCodigo && c.Bimestre == bimestre);
            }

            return new NotaPosConselhoDto()
            {
                Id = notaComponente?.Id,
                Nota = notaComponente?.NotaConceito
            };
        }

        private NotaBimestreDto ObterNotaFinalComponentePeriodo(long codigoComponenteCurricular, int bimestre, IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno)
        {
            double? notaConceito = null;
            // Busca nota do FechamentoNota
            var notaFechamento = notasFechamentoAluno.FirstOrDefault(c => c.ComponenteCurricularCodigo == codigoComponenteCurricular);
            if (notaFechamento != null)
                notaConceito = notaFechamento.NotaConceito;

            return new NotaBimestreDto()
            {
                Bimestre = bimestre,
                NotaConceito = notaConceito
            };
        }

        private IEnumerable<NotaBimestreDto> ObterNotasFinaisComponentePeriodos(long codigoComponenteCurricular, IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno)
        {
            var notasPeriodos = new List<NotaBimestreDto>();

            var notasFechamentoBimestres = notasFechamentoAluno.Where(c => c.ComponenteCurricularCodigo == codigoComponenteCurricular && c.Bimestre.HasValue);
            foreach (var notaFechamento in notasFechamentoBimestres)
            {
                notasPeriodos.Add(new NotaBimestreDto()
                {
                    Bimestre = notaFechamento.Bimestre.Value,
                    NotaConceito = notaFechamento.NotaConceito
                });
            }

            return notasPeriodos;
        }        
    }
}
