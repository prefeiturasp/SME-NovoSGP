using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using RespostasDto = SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Runtime.CompilerServices;

namespace SME.SGP.Aplicacao
{
    public class ObterSinteseConselhoDeClasseUseCase : IObterSinteseConselhoDeClasseUseCase
    {
        private readonly IMediator mediator;

        public ObterSinteseConselhoDeClasseUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<ConselhoDeClasseGrupoMatrizDto>> Executar(ConselhoClasseSinteseDto conselhoClasseSinteseDto)
        {
            var alunosEol = await mediator.Send(new ObterAlunosEolPorTurmaQuery(conselhoClasseSinteseDto.CodigoTurma, true));
            var informacoesAluno = alunosEol.FirstOrDefault(a => a.CodigoAluno == conselhoClasseSinteseDto.AlunoCodigo);

            var totalCompensacoesComponentesNaoLancamNotas = await mediator.Send(new ObterTotalCompensacoesComponenteNaoLancaNotaQuery(conselhoClasseSinteseDto.CodigoTurma, conselhoClasseSinteseDto.Bimestre));
            totalCompensacoesComponentesNaoLancamNotas = totalCompensacoesComponentesNaoLancamNotas.Where(x => x.CodigoAluno == conselhoClasseSinteseDto.AlunoCodigo);

            var totalAulasComponentesNaoLancamNota = Enumerable.Empty<TotalAulasNaoLancamNotaDto>();

            if (conselhoClasseSinteseDto.Bimestre != (int)Bimestre.Final)
                totalAulasComponentesNaoLancamNota = await mediator.Send(new ObterTotalAulasNaoLancamNotaQuery(conselhoClasseSinteseDto.CodigoTurma, conselhoClasseSinteseDto.Bimestre, conselhoClasseSinteseDto.AlunoCodigo));

            var retorno = new List<ConselhoDeClasseGrupoMatrizDto>();
            
            var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaPorIdAlunoCodigoQuery(conselhoClasseSinteseDto.FechamentoTurmaId, conselhoClasseSinteseDto.AlunoCodigo));
            var turma = await ObterTurmaFechamento(fechamentoTurma, conselhoClasseSinteseDto.CodigoTurma);

            await ValidarAlunoPossuiConselhoUltimoBimestreAnoAtual(turma, conselhoClasseSinteseDto.AlunoCodigo, conselhoClasseSinteseDto.Bimestre);
            
            var disciplinas = await mediator.Send(new ObterDisciplinasPorCodigoTurmaQuery(turma.CodigoTurma));
            if (disciplinas.NaoPossuiRegistros())
                return null;

            var tipoCalendarioId = await mediator.Send(new ObterTipoCalendarioIdPorTurmaQuery(turma));
            var frequenciasAluno = await mediator.Send(new ObterFrequenciasAlunoComponentePorTurmasQuery(conselhoClasseSinteseDto.AlunoCodigo, new string[] { turma.CodigoTurma }, tipoCalendarioId, 
                                                                                                        informacoesAluno.NaoEhNulo() ? informacoesAluno.DataMatricula : null, 
                                                                                                        conselhoClasseSinteseDto.Bimestre));
            var periodoEscolar = ObterPeriodoEscolarFechamento(fechamentoTurma, turma, conselhoClasseSinteseDto.Bimestre);

            var codigosComponentesCurriculares = disciplinas.Select(d => d.CodigoComponenteCurricular.ToString());

            return await ObterConselhosClassesGrupoMatriz(disciplinas, turma,
                                                          frequenciasAluno, 
                                                          totalAulasComponentesNaoLancamNota,
                                                          totalCompensacoesComponentesNaoLancamNotas,
                                                          conselhoClasseSinteseDto.AlunoCodigo, 
                                                          conselhoClasseSinteseDto.Bimestre);
        }

        private async Task<IEnumerable<ConselhoDeClasseGrupoMatrizDto>> ObterConselhosClassesGrupoMatriz(IEnumerable<DisciplinaResposta> disciplinas, 
                                                                                                         Turma turma, 
                                                                                                         IEnumerable<FrequenciaAluno> frequenciasAluno,
                                                                                                         IEnumerable<TotalAulasNaoLancamNotaDto> totalAulasComponentesNaoLancamNota,
                                                                                                         IEnumerable<TotalCompensacoesComponenteNaoLancaNotaDto> totalCompensacoesComponentesNaoLancamNotas,
                                                                                                         string codigoAluno, int bimestre)
        {
            var retorno = new List<ConselhoDeClasseGrupoMatrizDto>();
            var gruposMatrizes = disciplinas.Where(x => !x.LancaNota && x.GrupoMatriz.NaoEhNulo())
                                            .GroupBy(c => new { Id = c.GrupoMatriz?.Id, Nome = c.GrupoMatriz?.Nome });

            foreach (var grupoDisiplinasMatriz in gruposMatrizes.OrderBy(k => k.Key.Nome))
            {
                var grupoMatriz = new ConselhoDeClasseGrupoMatrizDto()
                {
                    Id = grupoDisiplinasMatriz.Key.Id ?? 0,
                    Titulo = grupoDisiplinasMatriz.Key.Nome ?? "",
                    ComponenteSinteses = new List<ConselhoDeClasseComponenteSinteseDto>()
                };

                foreach (var componenteCurricular in grupoDisiplinasMatriz.Where(x => !x.LancaNota))
                {
                    var codigoComponenteCurricular = componenteCurricular.CodigoComponenteCurricular;
                    var componentePermiteFrequencia = await mediator.Send(new ObterComponenteRegistraFrequenciaQuery(codigoComponenteCurricular));

                    if (bimestre == (int)Bimestre.Final)
                        if (componentePermiteFrequencia)
                            totalAulasComponentesNaoLancamNota = await mediator.Send(new ObterTotalAulasPorTurmaDisciplinaCodigoAlunoQuery(codigoComponenteCurricular.ToString(), turma.CodigoTurma, codigoAluno));
                        else 
                        {
                            var totalAulasNaoPermitemFrequencia = await mediator.Send(
                                new ObterTotalAulasSemFrequenciaPorTurmaQuery(codigoComponenteCurricular.ToString(), turma.CodigoTurma));

                            totalAulasComponentesNaoLancamNota = totalAulasNaoPermitemFrequencia.Select(x =>
                                new TotalAulasNaoLancamNotaDto
                                { DisciplinaId = Convert.ToInt32(x.DisciplinaId), TotalAulas = x.TotalAulas });
                        }

                    var componenteCurricularDto = await MapearDto(frequenciasAluno, componenteCurricular, bimestre,
                        turma.ModalidadeCodigo, turma.AnoLetivo, totalAulasComponentesNaoLancamNota,
                        totalCompensacoesComponentesNaoLancamNotas);

                    grupoMatriz.ComponenteSinteses.Add(componenteCurricularDto);
                }
                if (grupoMatriz.ComponenteSinteses.Any())
                    grupoMatriz.ComponenteSinteses = grupoMatriz.ComponenteSinteses
                                                        .OrderBy(c => c.Nome)
                                                        .GroupBy(c => c.Codigo)
                                                        .Select(c => c.First())
                                                        .ToList();

                retorno.Add(grupoMatriz);
            }
            return retorno;
        }

        private async Task<Turma> ObterTurmaFechamento(FechamentoTurma fechamento, string codigoTurma)
        {
            if (fechamento.NaoEhNulo())
                return fechamento?.Turma;
            
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(codigoTurma)) ?? throw new NegocioException(MensagemNegocioTurma.TURMA_NAO_ENCONTRADA);
            if (!turma.EhAnoAnterior())
                throw new NegocioException(MensagemNegocioFechamentoTurma.NAO_EXISTE_FECHAMENTO_TURMA);
            return turma;            
        }

        private async Task<PeriodoEscolar> ObterPeriodoEscolarFechamento(FechamentoTurma fechamento, Turma turma, int bimestre)
        {         
            if (fechamento.EhNulo() && bimestre > 0)
                return await mediator.Send(new ObterPeriodoEscolarPorTurmaBimestreQuery(turma, bimestre));
            return fechamento?.PeriodoEscolar;
        }

        private async Task ValidarAlunoPossuiConselhoUltimoBimestreAnoAtual(Turma turma, string codigoAluno, int bimestre)
        {
            var ehAnoAtual = turma.AnoLetivo == DateTime.Now.Year;
            var ehAno2020 = turma.AnoLetivo == 2020;
            var ehBimestreFinalAnoAtual = !ehAno2020 && ehAnoAtual && bimestre == 0;
            if (ehBimestreFinalAnoAtual && 
                !(await mediator.Send(new ExisteConselhoClasseUltimoBimestreQuery(turma, codigoAluno))))
                throw new NegocioException(MensagemNegocioConselhoClasse.ALUNO_NAO_POSSUI_CONSELHO_CLASSE_ULTIMO_BIMESTRE);
        }


        private async Task<ConselhoDeClasseComponenteSinteseDto> MapearDto(IEnumerable<FrequenciaAluno> frequenciaAluno, DisciplinaResposta componenteCurricular, int bimestre, Modalidade modalidade, int anoLetivo, IEnumerable<TotalAulasNaoLancamNotaDto> totalAulas, IEnumerable<TotalCompensacoesComponenteNaoLancaNotaDto> totalCompensacoes)
        {
            var dto = MapearDisciplinasDto(componenteCurricular);
            
            var frequenciaComponente = frequenciaAluno
                .FirstOrDefault(a => (a.DisciplinaId == componenteCurricular.CodigoComponenteCurricular.ToString() || a.DisciplinaId == componenteCurricular.CodigoComponenteTerritorioSaber.ToString()));

            frequenciaComponente = VerificaTotalAulasParaCalcularPercentualFrequencia(frequenciaComponente, totalAulas);

            var percentualFrequencia = CalcularPercentualFrequenciaComponente(frequenciaComponente, anoLetivo);

            var parecerFinal = bimestre == 0 && EhEjaCompartilhada(componenteCurricular, modalidade) == false
                ? await mediator.Send(new ObterSinteseAlunoQuery(string.IsNullOrEmpty(percentualFrequencia) ? 0 : double.Parse(percentualFrequencia), dto, anoLetivo))
                : null;

            var componenteSinteseAdicionar = MapearConselhoDeClasseComponenteSinteseDto(componenteCurricular,
                frequenciaComponente, percentualFrequencia, parecerFinal, totalAulas, totalCompensacoes, bimestre);

            return componenteSinteseAdicionar;
        }
        
        private string CalcularPercentualFrequenciaComponente(FrequenciaAluno frequenciaComponente, int anoLetivo)
        {
            return (anoLetivo == 2020 ? frequenciaComponente?.PercentualFrequenciaFinalFormatado : frequenciaComponente?.PercentualFrequenciaFormatado) ?? FrequenciaAluno.FormatarPercentual(0);
        }

        private bool EhEjaCompartilhada(DisciplinaResposta componenteCurricular, Modalidade modalidade)
        {
            const long componenteInformatica = 1061;
            const long componenteLeitura = 1060;

            return modalidade == Modalidade.EJA
                   && (componenteCurricular.CodigoComponenteCurricular == componenteInformatica || componenteCurricular.CodigoComponenteCurricular == componenteLeitura);
        }
        
        private FrequenciaAluno VerificaTotalAulasParaCalcularPercentualFrequencia(FrequenciaAluno frequenciaAluno, IEnumerable<TotalAulasNaoLancamNotaDto> totalAulas)
        {
            if (frequenciaAluno.NaoEhNulo())
            {
                var dadosAulasComponente = totalAulas.FirstOrDefault(t => t.DisciplinaId == Convert.ToInt64(frequenciaAluno.DisciplinaId));

                if (dadosAulasComponente.NaoEhNulo())
                {
                    int totalAulasComponente = Convert.ToInt32(dadosAulasComponente.TotalAulas);
                    frequenciaAluno.TotalAulas = frequenciaAluno.TotalAulas == totalAulasComponente
                        ? frequenciaAluno.TotalAulas
                        : frequenciaAluno.TotalAulas > totalAulasComponente
                            ? frequenciaAluno.TotalAulas
                            : totalAulasComponente;
                }
            }

            return frequenciaAluno;
        }
        
        private DisciplinaDto MapearDisciplinasDto(DisciplinaResposta componenteCurricular)
        {
            return new DisciplinaDto
            {
                CodigoComponenteCurricular = componenteCurricular.CodigoComponenteCurricular,
                Compartilhada = componenteCurricular.Compartilhada,
                LancaNota = componenteCurricular.LancaNota,
                Nome = componenteCurricular.Nome,
                Regencia = componenteCurricular.Regencia,
                RegistraFrequencia = componenteCurricular.RegistroFrequencia,
                TerritorioSaber = componenteCurricular.TerritorioSaber
            };
        }
		        
        private ConselhoDeClasseComponenteSinteseDto MapearConselhoDeClasseComponenteSinteseDto(DisciplinaResposta componenteCurricular, FrequenciaAluno frequenciaDisciplina, string percentualFrequencia, SinteseDto parecerFinal, IEnumerable<TotalAulasNaoLancamNotaDto> totalAulas, IEnumerable<TotalCompensacoesComponenteNaoLancaNotaDto> totalCompensacoes, int bimestre)
        {
            var codigoComponenteCurricular = componenteCurricular.CodigoComponenteCurricular;

            return new ConselhoDeClasseComponenteSinteseDto
            {
                Codigo = codigoComponenteCurricular,
                Nome = componenteCurricular.Nome,
                TotalFaltas = frequenciaDisciplina?.TotalAusencias,
                PercentualFrequencia = ExibirPercentualFrequencia(percentualFrequencia, totalAulas, frequenciaDisciplina?.TotalAulas, codigoComponenteCurricular),
                ParecerFinal = (parecerFinal?.Valor).EhNulo() || !totalAulas.Any() ? string.Empty : parecerFinal?.Valor,
                ParecerFinalId = (int)(parecerFinal?.Id ?? default),
                TotalAulas = ExibirTotalAulas(totalAulas, frequenciaDisciplina?.TotalAulas, codigoComponenteCurricular),
                TotalAusenciasCompensadas = ExibirTotalCompensadas(totalCompensacoes, codigoComponenteCurricular, bimestre)
            };
        }
        
        private string ExibirPercentualFrequencia(string percentualFrequencia, IEnumerable<TotalAulasNaoLancamNotaDto> totalAulas, int? totalAulasAlunoDisciplina, long componenteCurricular)
        {
            var aulas = totalAulas.FirstOrDefault(x => x.DisciplinaId == componenteCurricular);

            totalAulasAlunoDisciplina = totalAulasAlunoDisciplina ?? 0;

            if ((aulas.EhNulo() && totalAulasAlunoDisciplina == 0) || String.IsNullOrEmpty(percentualFrequencia) || (percentualFrequencia == "0" && aulas.EhNulo()))
                return "";

            return $"{percentualFrequencia}%";
        }

        private string ExibirTotalCompensadas(IEnumerable<TotalCompensacoesComponenteNaoLancaNotaDto> totalCompensacao, long codigoComponenteCurricular, int bimestre)
        {
            if (bimestre != (int)Bimestre.Final)
            {
                var compensacoes = totalCompensacao.FirstOrDefault(x => x.DisciplinaId == codigoComponenteCurricular);
                return compensacoes?.TotalCompensacoes ?? "0";
            }
            else
            {
                var compensacoes = totalCompensacao.Where(x => x.DisciplinaId == codigoComponenteCurricular);
                return compensacoes.Sum(x => Convert.ToInt64(x.TotalCompensacoes)).ToString();
            }
        }

        private string ExibirTotalAulas(IEnumerable<TotalAulasNaoLancamNotaDto> aulas, int? totalAulasAlunoDisciplina, long codigoComponenteCurricular)
        {
            var aulasComponente = aulas.FirstOrDefault(x => x.DisciplinaId == codigoComponenteCurricular);

            totalAulasAlunoDisciplina = totalAulasAlunoDisciplina ?? 0;

            return aulasComponente.NaoEhNulo()
                    ? Convert.ToInt32(aulasComponente.TotalAulas) >= totalAulasAlunoDisciplina
                                                                  ? aulasComponente.TotalAulas
                                                                  : totalAulasAlunoDisciplina.ToString()
                    : totalAulasAlunoDisciplina.ToString();
        }
    }
}