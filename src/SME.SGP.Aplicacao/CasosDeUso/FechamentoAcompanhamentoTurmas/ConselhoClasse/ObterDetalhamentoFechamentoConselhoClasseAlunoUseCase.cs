using MediatR;
using SME.SGP.Aplicacao.Interfaces;
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
        public ObterDetalhamentoFechamentoConselhoClasseAlunoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<DetalhamentoComponentesCurricularesAlunoDto>> Executar(FiltroConselhoClasseConsolidadoDto filtro)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(filtro.TurmaId));

            var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaCodigoBimestreQuery(turma.CodigoTurma, filtro.Bimestre));
            var conselhoClasse = fechamentoTurma != null ? await mediator.Send(new ObterConselhoClassePorFechamentoIdQuery(fechamentoTurma.Id)) : null;

            var periodoEscolar = fechamentoTurma?.PeriodoEscolar;
            var bimestre = filtro.Bimestre;

            if (fechamentoTurma != null) turma = fechamentoTurma?.Turma;
            else
            {
                if (bimestre > 0)
                    periodoEscolar = await mediator.Send(new ObterPeriodoEscolarPorTurmaBimestreQuery(turma, bimestre));
            }

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

            var disciplinasDaTurmaEol = await mediator.Send(new ObterComponentesCurricularesPorTurmasCodigoQuery(turmasCodigos, usuarioAtual.PerfilAtual, usuarioAtual.Login, turma.EnsinoEspecial, turma.TurnoParaComponentesCurriculares));

            var disciplinasDaTurma = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(disciplinasDaTurmaEol.Select(x => x.CodigoComponenteCurricular).Distinct().ToArray()));

            var retorno = new List<DetalhamentoComponentesCurricularesAlunoDto>();

            var gruposMatrizesNotas = new List<ConselhoClasseAlunoNotasConceitosDto>();

            var frequenciasAluno = await ObterFrequenciaAlunoRefatorada(disciplinasDaTurmaEol, periodoEscolar, filtro.AlunoCodigo, tipoCalendarioId, bimestre);

            var gruposMatrizes = disciplinasDaTurma.Where(c => c.LancaNota && c.GrupoMatrizNome != null).OrderBy(d => d.GrupoMatrizId).GroupBy(c => c.GrupoMatrizNome).ToList();

            foreach (var grupoDisiplinasMatriz in gruposMatrizes)
            {
                var conselhoClasseAlunoNotas = new DetalhamentoComponentesCurricularesAlunoDto();

                foreach (var disciplina in grupoDisiplinasMatriz.OrderBy(g => g.Nome))
                {
                    var frequenciasAlunoParaTratar = frequenciasAluno.Where(a => a.DisciplinaId == disciplina.Id.ToString());
                    FrequenciaAluno frequenciaAluno;

                    if (frequenciasAlunoParaTratar == null || !frequenciasAlunoParaTratar.Any())
                    {
                        frequenciaAluno = new FrequenciaAluno() { DisciplinaId = disciplina.Id.ToString(), TurmaId = disciplina.TurmaCodigo };
                    }
                    else if (frequenciasAlunoParaTratar.Count() == 1)
                    {
                        frequenciaAluno = frequenciasAlunoParaTratar.FirstOrDefault();
                    }
                    else
                    {
                        frequenciaAluno = new FrequenciaAluno()
                        {
                            DisciplinaId = disciplina.CodigoComponenteCurricular.ToString(),
                            CodigoAluno = filtro.AlunoCodigo
                        };


                        frequenciaAluno.TotalAulas = frequenciasAlunoParaTratar.Sum(a => a.TotalAulas);
                        frequenciaAluno.TotalAusencias = frequenciasAlunoParaTratar.Sum(a => a.TotalAusencias);
                        frequenciaAluno.TotalCompensacoes = frequenciasAlunoParaTratar.Sum(a => a.TotalCompensacoes);

                    }

                    if (disciplina.Regencia)
                    {
                        retorno.AddRange(await ObterNotasFrequenciaRegencia(disciplina.CodigoComponenteCurricular,
                                                                            frequenciaAluno,
                                                                            periodoEscolar,
                                                                            turma,
                                                                            notasConselhoClasseAluno,
                                                                            notasFechamentoAluno));
                    }
                    else
                        retorno.Add(ObterNotasFrequenciaComponente(disciplina.Nome,
                                                                    disciplina.CodigoComponenteCurricular,
                                                                    frequenciaAluno,
                                                                    periodoEscolar,
                                                                    turma,
                                                                    notasConselhoClasseAluno,
                                                                    notasFechamentoAluno));
                }
            }

            return retorno;
        }

        private async Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaAlunoRefatorada(IEnumerable<DisciplinaDto> disciplinasDaTurma, PeriodoEscolar periodoEscolar, string alunoCodigo,
            long tipoCalendarioId, int bimestre)
        {
            var frequenciasAlunoRetorno = new List<FrequenciaAluno>();


            var disciplinasId = disciplinasDaTurma.Select(a => a.CodigoComponenteCurricular.ToString()).Distinct().ToArray();
            var turmasCodigo = disciplinasDaTurma.Select(a => a.TurmaCodigo).Distinct().ToArray();

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

            var frequenciasAluno = await mediator.Send(new ObterFrequenciasAlunosPorCodigoAlunoCodigoComponentesTurmaQuery(alunoCodigo, turmasCodigo, disciplinasId));
            frequenciasAluno = frequenciasAluno.Where(f => f.Bimestre == bimestre);

            var aulasComponentesTurmas = await mediator.Send(new ObterAulasDadasTurmaEBimestreEComponenteCurricularQuery(turmasCodigo, tipoCalendarioId, disciplinasId, bimestres));

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

        private DetalhamentoComponentesCurricularesAlunoDto ObterNotasFrequenciaComponente(string componenteCurricularNome, long componenteCurricularCodigo, FrequenciaAluno frequenciaAluno, PeriodoEscolar periodoEscolar, Turma turma, IEnumerable<NotaConceitoBimestreComponenteDto> notasConselhoClasseAluno, IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno)
        {
            var percentualFrequencia = (frequenciaAluno.TotalAulas > 0 ? frequenciaAluno?.PercentualFrequencia ?? 100 : 100);

            // Cálculo de frequência particular do ano de 2020
            if (periodoEscolar == null && turma.AnoLetivo.Equals(2020))
                percentualFrequencia = frequenciaAluno.PercentualFrequenciaFinal;

            var notasFechamento = ObterNotasComponente(componenteCurricularCodigo, periodoEscolar, notasFechamentoAluno);
            var notaPosConselho = ObterNotaPosConselho(componenteCurricularCodigo, periodoEscolar?.Bimestre, notasConselhoClasseAluno, notasFechamentoAluno);

            var conselhoClasseComponente = new DetalhamentoComponentesCurricularesAlunoDto()
            {
                NomeComponenteCurricular = componenteCurricularNome,
                QuantidadeAusencia = frequenciaAluno?.TotalAusencias ?? 0,
                QuantidadeCompensacoes = frequenciaAluno?.TotalCompensacoes ?? 0,
                PercentualFrequencia = percentualFrequencia,
                NotaFechamento = notasFechamento != null && notasFechamento.Any() ? String.Format("{0:0.0}", notasFechamento.FirstOrDefault().NotaConceito) : "-",
                NotaPosConselho = notaPosConselho != null ? String.Format("{0:0.0}", notaPosConselho.Nota) : "-"
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
                NotaFechamento = notasFechamento != null && notasFechamento.Any() ? String.Format("{0:0.0}", notasFechamento.FirstOrDefault().NotaConceito) : "-",
                NotaPosConselho = notaPosConselho != null ? String.Format("{0:0.0}", notaPosConselho.Nota) : "-"
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
