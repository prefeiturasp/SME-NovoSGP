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

            var totalCompensacoesComponenteSemNota = await mediator.Send(new ObterTotalCompensacoesComponenteNaoLancaNotaQuery(conselhoClasseSinteseDto.CodigoTurma, conselhoClasseSinteseDto.Bimestre));
            totalCompensacoesComponenteSemNota = totalCompensacoesComponenteSemNota.Where(x => x.CodigoAluno == conselhoClasseSinteseDto.AlunoCodigo);

            var totalAulasComponenteSemNota = Enumerable.Empty<TotalAulasNaoLancamNotaDto>();

            if (conselhoClasseSinteseDto.Bimestre != (int)Bimestre.Final)
                totalAulasComponenteSemNota = await mediator.Send(new ObterTotalAulasNaoLancamNotaQuery(conselhoClasseSinteseDto.CodigoTurma, conselhoClasseSinteseDto.Bimestre, conselhoClasseSinteseDto.AlunoCodigo));

            var retorno = new List<ConselhoDeClasseGrupoMatrizDto>();
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(conselhoClasseSinteseDto.CodigoTurma));
            if (turma == null)
                throw new NegocioException(MensagemNegocioTurma.TURMA_NAO_ENCONTRADA);

            var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaPorIdAlunoCodigoQuery(conselhoClasseSinteseDto.FechamentoTurmaId, conselhoClasseSinteseDto.AlunoCodigo, turma.EhAnoAnterior()));

            if (fechamentoTurma != null)
                turma = fechamentoTurma?.Turma;
            else
            {
                if (!turma.EhAnoAnterior())
                    throw new NegocioException(MensagemNegocioFechamentoTurma.NAO_EXISTE_FECHAMENTO_TURMA);
            }

            if (turma.AnoLetivo != 2020 && turma.AnoLetivo == DateTime.Now.Year && conselhoClasseSinteseDto.Bimestre == 0 && !(await mediator.Send(new ExisteConselhoClasseUltimoBimestreQuery(turma, conselhoClasseSinteseDto.AlunoCodigo))))
                throw new NegocioException(MensagemNegocioConselhoClasse.ALUNO_NAO_POSSUI_CONSELHO_CLASSE_ULTIMO_BIMESTRE);

            var disciplinas = (await mediator.Send(new ObterDisciplinasPorCodigoTurmaQuery(turma.CodigoTurma)))?.ToList();
            if (disciplinas == null || !disciplinas.Any())
                return null;

            var componentesCurricularesSgp = await mediator.Send(new ObterInfoPedagogicasComponentesCurricularesPorIdsQuery(disciplinas.ObterCodigos()));
            disciplinas.PreencherInformacoesPegagogicasSgp(componentesCurricularesSgp);

            var gruposMatrizes = disciplinas.Where(x => !x.LancaNota && x.GrupoMatriz != null)
                                            .GroupBy(c => new { Id = c.GrupoMatriz?.Id, Nome = c.GrupoMatriz?.Nome });

            var tipoCalendarioId = await mediator.Send(new ObterTipoCalendarioIdPorTurmaQuery(turma));
            var frequenciaAluno = await mediator.Send(new ObterFrequenciasAlunoComponentePorTurmasQuery(conselhoClasseSinteseDto.AlunoCodigo, new string[] { turma.CodigoTurma }, tipoCalendarioId, informacoesAluno != null ? informacoesAluno.DataMatricula : null, conselhoClasseSinteseDto.Bimestre));

            var periodoEscolar = fechamentoTurma?.PeriodoEscolar;

            if (fechamentoTurma != null) turma = fechamentoTurma?.Turma;
            else
            {
                if (conselhoClasseSinteseDto.Bimestre > 0)
                    periodoEscolar = await mediator.Send(new ObterPeriodoEscolarPorTurmaBimestreQuery(turma, conselhoClasseSinteseDto.Bimestre));
            }

            var codigosComponentesConsiderados = new List<string>();

            codigosComponentesConsiderados
                .AddRange(disciplinas.Select(d => d.CodigoComponenteCurricular.ToString()));

            codigosComponentesConsiderados
                .AddRange(disciplinas.Where(d => d.TerritorioSaber && d.CodigoComponenteTerritorioSaber.HasValue && d.CodigoComponenteTerritorioSaber > 0).Select(d => d.CodigoComponenteTerritorioSaber.ToString()));

            var registrosFrequencia = await mediator
                .Send(new ObterFrequenciasRegistradasPorTurmasComponentesCurricularesQuery(conselhoClasseSinteseDto.AlunoCodigo, new string[] { turma.CodigoTurma }, codigosComponentesConsiderados.ToArray(), periodoEscolar?.Id));

            if (fechamentoTurma != null)
                turma = fechamentoTurma.Turma;

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
                    var codigoComponenteCurricular = ObterCodigoComponenteCurricular(componenteCurricular);
                    if (componenteCurricular.TerritorioSaber)
                        componenteCurricular.Nome = disciplinas.First(d => d.CodigoComponenteCurricular == componenteCurricular.CodigoComponenteCurricular).Nome;
                    var componentePermiteFrequencia = await mediator.Send(new ObterComponenteRegistraFrequenciaQuery(codigoComponenteCurricular));

                    if (conselhoClasseSinteseDto.Bimestre == (int)Bimestre.Final && componentePermiteFrequencia)
                    {
                        totalAulasComponenteSemNota = await mediator.Send(new ObterTotalAulasPorTurmaDisciplinaCodigoAlunoQuery(codigoComponenteCurricular.ToString(), conselhoClasseSinteseDto.CodigoTurma, conselhoClasseSinteseDto.AlunoCodigo));
                    }
                    else if (conselhoClasseSinteseDto.Bimestre == (int)Bimestre.Final && !componentePermiteFrequencia)
                    {
                        var totalAulasNaoPermitemFrequencia = await mediator.Send(
                            new ObterTotalAulasSemFrequenciaPorTurmaQuery(codigoComponenteCurricular.ToString(), conselhoClasseSinteseDto.CodigoTurma));

                        totalAulasComponenteSemNota = totalAulasNaoPermitemFrequencia.Select(x =>
                            new TotalAulasNaoLancamNotaDto
                            { DisciplinaId = Convert.ToInt32(x.DisciplinaId), TotalAulas = x.TotalAulas });
                    }

                    var componenteCurricularDto = await MapearDto(frequenciaAluno, componenteCurricular, conselhoClasseSinteseDto.Bimestre, registrosFrequencia,
                        turma.ModalidadeCodigo, turma.AnoLetivo, totalAulasComponenteSemNota,
                        totalCompensacoesComponenteSemNota);

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
        
        private async Task<ConselhoDeClasseComponenteSinteseDto> MapearDto(IEnumerable<FrequenciaAluno> frequenciaAluno, DisciplinaResposta componenteCurricular, int bimestre, IEnumerable<RegistroFrequenciaAlunoBimestreDto> registrosFrequencia, Modalidade modalidade, int anoLetivo, IEnumerable<TotalAulasNaoLancamNotaDto> totalAulas, IEnumerable<TotalCompensacoesComponenteNaoLancaNotaDto> totalCompensacoes)
        {
            var dto = MapearDisciplinasDto(componenteCurricular);
            
            var frequenciaComponente = frequenciaAluno
                .FirstOrDefault(a => (!componenteCurricular.TerritorioSaber && a.DisciplinaId == componenteCurricular.CodigoComponenteCurricular.ToString()) ||
                                     (componenteCurricular.TerritorioSaber && (a.DisciplinaId == componenteCurricular.CodigoComponenteCurricular.ToString() || a.DisciplinaId == componenteCurricular.CodigoComponenteTerritorioSaber.ToString()) && a.Professor == componenteCurricular.Professor));

            frequenciaComponente = VerificaTotalAulasParaCalcularPercentualFrequencia(frequenciaComponente, totalAulas);

            var percentualFrequencia = CalcularPercentualFrequenciaComponente(frequenciaComponente, componenteCurricular, anoLetivo);

            var parecerFinal = bimestre == 0 && EhEjaCompartilhada(componenteCurricular, modalidade) == false
                ? await mediator.Send(new ObterSinteseAlunoQuery(string.IsNullOrEmpty(percentualFrequencia) ? 0 : double.Parse(percentualFrequencia), dto, anoLetivo))
                : null;

            var componenteSinteseAdicionar = MapearConselhoDeClasseComponenteSinteseDto(componenteCurricular,
                frequenciaComponente, percentualFrequencia, parecerFinal, totalAulas, totalCompensacoes, bimestre);

            return componenteSinteseAdicionar;
        }
        
        private string CalcularPercentualFrequenciaComponente(FrequenciaAluno frequenciaComponente, DisciplinaResposta componenteCurricular, int anoLetivo)
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
            if (frequenciaAluno != null)
            {
                var dadosAulasComponente = totalAulas.FirstOrDefault(t => t.DisciplinaId == Convert.ToInt64(frequenciaAluno.DisciplinaId));

                if (dadosAulasComponente != null)
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
            var codigoComponenteCurricular = ObterCodigoComponenteCurricular(componenteCurricular);

            return new ConselhoDeClasseComponenteSinteseDto
            {
                Codigo = codigoComponenteCurricular,
                Nome = componenteCurricular.Nome,
                TotalFaltas = frequenciaDisciplina?.TotalAusencias,
                PercentualFrequencia = ExibirPercentualFrequencia(percentualFrequencia, totalAulas, frequenciaDisciplina?.TotalAulas, codigoComponenteCurricular),
                ParecerFinal = parecerFinal?.Valor == null || !totalAulas.Any() ? string.Empty : parecerFinal?.Valor,
                ParecerFinalId = (int)(parecerFinal?.Id ?? default),
                TotalAulas = ExibirTotalAulas(totalAulas, frequenciaDisciplina?.TotalAulas, codigoComponenteCurricular),
                TotalAusenciasCompensadas = ExibirTotalCompensadas(totalCompensacoes, codigoComponenteCurricular, bimestre)
            };
        }
        
        private long ObterCodigoComponenteCurricular(DisciplinaResposta componenteCurricular)
        {
            return componenteCurricular.TerritorioSaber
                ? componenteCurricular.CodigoComponenteTerritorioSaber.GetValueOrDefault()
                : componenteCurricular.CodigoComponenteCurricular;
        }
        
        private string ExibirPercentualFrequencia(string percentualFrequencia, IEnumerable<TotalAulasNaoLancamNotaDto> totalAulas, int? totalAulasAlunoDisciplina, long componenteCurricular)
        {
            var aulas = totalAulas.FirstOrDefault(x => x.DisciplinaId == componenteCurricular);

            totalAulasAlunoDisciplina = totalAulasAlunoDisciplina ?? 0;

            if ((aulas == null && totalAulasAlunoDisciplina == 0) || String.IsNullOrEmpty(percentualFrequencia) || (percentualFrequencia == "0" && aulas == null))
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

            return aulasComponente != null
                    ? Convert.ToInt32(aulasComponente.TotalAulas) >= totalAulasAlunoDisciplina
                                                                  ? aulasComponente.TotalAulas
                                                                  : totalAulasAlunoDisciplina.ToString()
                    : totalAulasAlunoDisciplina.ToString();
        }
    }
}