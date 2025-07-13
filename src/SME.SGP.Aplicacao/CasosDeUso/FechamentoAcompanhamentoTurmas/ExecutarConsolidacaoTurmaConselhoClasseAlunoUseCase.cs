using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarConsolidacaoTurmaConselhoClasseAlunoUseCase : AbstractUseCase, IExecutarConsolidacaoTurmaConselhoClasseAlunoUseCase
    {
        private const int BIMESTRE_2 = 2;
        private const int BIMESTRE_4 = 4;
        private const double NOTA_CONCEITO_CINCO = 5.0;
        private const double NOTA_CONCEITO_SETE = 7.0;
        private const int EdFisica = 6;

        public ExecutarConsolidacaoTurmaConselhoClasseAlunoUseCase(IMediator mediator) : base(mediator)
        { }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<MensagemConsolidacaoConselhoClasseAlunoDto>();

            if (filtro.EhNulo())
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível iniciar a consolidação do conselho de clase da turma -> aluno. O id da turma bimestre aluno não foram informados", LogNivel.Critico, LogContexto.ConselhoClasse));
                return false;
            }

            bool componenteEdFisicaEJANecessitaConversaoNotaConceito = false;
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(filtro.TurmaId));
            var ue = turma.Ue;
            var turmasCodigos = new List<string> { turma.CodigoTurma };
            if (turma.EhEJA() || turma.EhTurmaEnsinoMedio)
            {
                var codigosComplementares = await ObterTurmasComplementaresEOL(turma, filtro.AlunoCodigo);
                turmasCodigos.AddRange(codigosComplementares);
                if (!turma.EhTurmaRegular())
                {
                    var turmaRegular = await ObterTurmaRegular(codigosComplementares, turma.Semestre, turma.EhEJA(), turma.EhTurmaEnsinoMedio);
                    if (turmaRegular.EhNulo())
                    {
                        turmaRegular = await ObterTurmaRegularPorConselhoClasseComplementar(turma, filtro.AlunoCodigo);
                        turmasCodigos.Add(turmaRegular?.CodigoTurma);
                    }

                    if (turmaRegular.EhNulo())
                        throw new NegocioException(MensagemNegocioTurma.TURMA_REGULAR_NAO_ENCONTRADA);

                    if (turma.EhTurmaEdFisica())
                        componenteEdFisicaEJANecessitaConversaoNotaConceito = await TipoNotaEhConceito(turmaRegular, (filtro.Bimestre ?? 0));

                    turma = turmaRegular;
                    filtro.TurmaId = turma?.Id ?? 0;

                }
                else if (turma.EhTurmaRegular() && turma.EhEJA())
                    componenteEdFisicaEJANecessitaConversaoNotaConceito = await TipoNotaEhConceito(turma, (filtro.Bimestre ?? 0));
            }

            var statusNovo = SituacaoConselhoClasse.NaoIniciado;
            var consolidadoTurmaAluno = await mediator.Send(new ObterConselhoClasseConsolidadoPorTurmaAlunoQuery(filtro.TurmaId, filtro.AlunoCodigo));

            consolidadoTurmaAluno ??= new ConselhoClasseConsolidadoTurmaAluno
            {
                AlunoCodigo = filtro.AlunoCodigo,
                TurmaId = filtro.TurmaId,
                Status = statusNovo
            };

            var componentesComNotaFechamentoOuConselho = (await mediator
                                .Send(new ObterComponentesComNotaDeFechamentoOuConselhoQuery(turma?.AnoLetivo ?? 0, turmasCodigos.ToArray(), filtro.Bimestre, filtro.AlunoCodigo))).ToList();

            if (PodeAdicionarNota(filtro, componentesComNotaFechamentoOuConselho))
            {
                var lancaNota = await mediator.Send(new ObterComponenteLancaNotaQuery((long)filtro.ComponenteCurricularId));
                componentesComNotaFechamentoOuConselho.Add(new ComponenteCurricularDto() { Codigo = filtro.ComponenteCurricularId.ToString(), LancaNota = lancaNota });
            }
            if (!filtro.Inativo && componentesComNotaFechamentoOuConselho.Any())
            {
                var codigosComplementares = await ObterTurmasComplementaresEOL(turma, filtro.AlunoCodigo);

                if (turma.ModalidadeCodigo != Modalidade.Fundamental && codigosComplementares.Any())
                    turmasCodigos.AddRange(codigosComplementares);

                var componentesDaTurmaES = await mediator.Send(new ObterInfoComponentesCurricularesESPorTurmasCodigoQuery(turmasCodigos.ToArray()));

                if (componentesDaTurmaES.NaoEhNulo() && componentesDaTurmaES.Any())
                {
                    if (!filtro.Bimestre.HasValue || filtro.Bimestre == 0)
                    {
                        var fechamento = await mediator.Send(new ObterFechamentoPorTurmaPeriodoQuery() { TurmaId = filtro.TurmaId });
                        var conselhoClasse = fechamento != null ? await mediator.Send(new ObterConselhoClassePorFechamentoIdQuery(fechamento.Id)) : null;
                        var conselhoClasseAluno = conselhoClasse != null ? await mediator.Send(new ObterConselhoClasseAlunoPorAlunoCodigoConselhoIdQuery(conselhoClasse.Id, filtro.AlunoCodigo)) : null;
                        consolidadoTurmaAluno.ParecerConclusivoId = conselhoClasseAluno?.ConselhoClasseParecerId;
                    }

                    //Exceção de disciplina ED. Fisica para modalidade EJA
                    if (turma.EhEJA())
                    {
                        var matriculasAluno = await mediator.Send(new ObterMatriculasAlunoNaTurmaQuery(turma.CodigoTurma, filtro.AlunoCodigo));
                        var dispensadoEdFisica = matriculasAluno?.Where(m => m.CodigoTurma.ToString() == turma.CodigoTurma && m.CodigoSituacaoMatricula == SituacaoMatriculaAluno.DispensadoEdFisica)?.FirstOrDefault();
                        if (dispensadoEdFisica != null)
                            componentesDaTurmaES = componentesDaTurmaES.Where(a => a.Codigo != EdFisica);
                    }

                    var possuiComponentesSemNotaConceito = componentesDaTurmaES
                        .Where(ct => ct.LancaNota && !ct.EhTerritorioSaber)
                        .Select(ct => ct.Codigo.ToString())
                        .Except(componentesComNotaFechamentoOuConselho.Select(cn => cn.Codigo))
                        .Any();

                    if (possuiComponentesSemNotaConceito)
                        statusNovo = SituacaoConselhoClasse.EmAndamento;
                    else
                        statusNovo = SituacaoConselhoClasse.Concluido;
                }

                if (consolidadoTurmaAluno.ParecerConclusivoId.NaoEhNulo())
                    statusNovo = SituacaoConselhoClasse.Concluido;
            }

            consolidadoTurmaAluno.Status = statusNovo;
            consolidadoTurmaAluno.DataAtualizacao = DateTime.Now;

            try
            {
                var consolidadoTurmaAlunoId = await mediator.Send(new SalvarConselhoClasseConsolidadoTurmaAlunoCommand(consolidadoTurmaAluno));

                //Quando parecer conclusivo, não altera a nota, atualiza somente o parecerId
                if (!filtro.EhParecer &&
                    componentesComNotaFechamentoOuConselho.NaoEhNulo() &&
                    componentesComNotaFechamentoOuConselho.Any())
                {
                    var turmasIds = await mediator.Send(new ObterTurmaIdsPorCodigosQuery(turmasCodigos.ToArray()));
                    var conselhoClasseNotas = await mediator.Send(new ObterNotasConceitosConselhoClassePorTurmasCodigosEBimestreQuery(turmasCodigos.ToArray(), filtro.Bimestre ?? 0, alunoCodigo: filtro.AlunoCodigo));
                    var fechamentoNotas = await mediator.Send(new ObterNotasConceitosFechamentoPorTurmasCodigosEBimestreEAlunoCodigoQuery(turmasCodigos.ToArray(), filtro.Bimestre ?? 0, filtro.AlunoCodigo));
                    var converterNotaEmConceitoTurmaEdFisicaEJA = (turma.EhEJA() && componenteEdFisicaEJANecessitaConversaoNotaConceito);
                    if (converterNotaEmConceitoTurmaEdFisicaEJA)
                        TratarConversaoNotaEdFisicaEJA(fechamentoNotas);

                    foreach (var componenteCurricular in componentesComNotaFechamentoOuConselho)
                    {
                        if (!componenteCurricular.LancaNota)
                            continue;

                        var notaConceitoCache = await ObterNotaConceitoCache(turmasIds.ToArray(), long.Parse(componenteCurricular.Codigo), filtro.Bimestre ?? 0, filtro.AlunoCodigo, converterNotaEmConceitoTurmaEdFisicaEJA);
                        await SalvarConsolidacaoConselhoClasseNota(turma, long.Parse(componenteCurricular.Codigo),
                                                                    consolidadoTurmaAlunoId, conselhoClasseNotas, fechamentoNotas,
                                                                    filtro, notaConceitoCache);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Ocorreu um erro na persistência da consolidação do conselho de classe da turma aluno/nota", LogNivel.Critico, LogContexto.ConselhoClasse, ex.Message, "SGP", ex.StackTrace, ex.InnerException?.ToString()));
                throw;
            }
        }

        private bool PodeAdicionarNota(
                                       MensagemConsolidacaoConselhoClasseAlunoDto filtro,
                                       List<ComponenteCurricularDto> componentes)
        {
            var possuiIdComponente = filtro.ComponenteCurricularId.HasValue && filtro.ComponenteCurricularId != 0;

            return possuiIdComponente && !componentes.Any(cc => cc.Codigo.Equals(filtro.ComponenteCurricularId.ToString()));
        }

        private async Task<(double? Nota, long? ConceitoId, bool EhNotaConceitoConselhoCache)> ObterNotaConceitoCache(long[] turmasId, long componenteCurricularId, int bimestre, string alunoCodigo, bool converterNotaEmConceitoTurmaEdFisicaEJA = false)
        {
            var retorno = new List<ConsolidadoConselhoClasseAlunoNotaCacheDto>();
            foreach (var turma in turmasId)
            {
                var retornoCacheMapeado = (await mediator.Send(new ObterCacheNotaConceitoConsolidacaoConselhoClasseAlunoQuery(turma, componenteCurricularId, bimestre, alunoCodigo)));
                if (retornoCacheMapeado.NaoEhNulo())
                    retorno.Add(retornoCacheMapeado);
            }

            var notaConselhoClasse = retorno.FirstOrDefault(x => x.NotaConselhoClasse.HasValue || x.ConceitoIdConselhoClasse.HasValue);
            if (notaConselhoClasse.NaoEhNulo())
                return (notaConselhoClasse.NotaConselhoClasse, notaConselhoClasse.ConceitoIdConselhoClasse, true);

            var notaFechamento = retorno.FirstOrDefault(x => x.NotaFechamento.HasValue || x.ConceitoIdFechamento.HasValue);
            if (notaFechamento.NaoEhNulo())
            {
                var nota = notaFechamento.NotaFechamento;
                var conceitoId = notaFechamento.ConceitoIdFechamento;
                if (converterNotaEmConceitoTurmaEdFisicaEJA && componenteCurricularId.Equals(MensagemNegocioComponentesCurriculares.COMPONENTE_CURRICULAR_CODIGO_ED_FISICA))
                {
                    conceitoId = ConverterNotaConceito(nota, conceitoId);
                    nota = null;
                }
                return (nota, conceitoId, false);
            }

            return (null, null, false);
        }

        private void TratarConversaoNotaEdFisicaEJA(IEnumerable<FechamentoNotaAlunoAprovacaoDto> fechamentoNotas)
        {
            if (fechamentoNotas.NaoEhNulo())
            {
                var fechamentosNotaDisciplinaEdFisica = fechamentoNotas.Where(fn => fn.ComponenteCurricularId.Equals(MensagemNegocioComponentesCurriculares.COMPONENTE_CURRICULAR_CODIGO_ED_FISICA));
                foreach (var fechamentoDisciplinaEdFisica in fechamentosNotaDisciplinaEdFisica)
                {
                    fechamentoDisciplinaEdFisica.ConceitoId = ConverterNotaConceito(fechamentoDisciplinaEdFisica.Nota, (long?)fechamentoDisciplinaEdFisica.ConceitoId);
                    fechamentoDisciplinaEdFisica.Nota = null;
                }
            }
        }

        private async Task<Turma> ObterTurmaRegular(string[] codigosTurmasComplementares, int semestre, bool ehTurmaEJA, bool ehTurmaEM)
        {
            return (await mediator.Send(new ObterTurmasPorCodigosQuery(codigosTurmasComplementares))).FirstOrDefault(t => t.EhTurmaRegular()
                                                                                                            && t.Semestre == semestre
                                                                                                            && t.EhEJA() == ehTurmaEJA
                                                                                                            && t.EhTurmaEnsinoMedio == ehTurmaEM
                                                                                                            );
        }

        private async Task<Turma> ObterTurmaRegularPorConselhoClasseComplementar(Turma turmaComplementar, string aluno)
        {
            var turmasAluno = await mediator.Send(new ObterTurmasFechamentoConselhoPorAlunosQuery(new long[] { long.Parse(aluno) }, turmaComplementar.AnoLetivo));
            if (!turmasAluno.Any())
                throw new NegocioException(MensagemNegocioTurma.TURMA_NAO_ENCONTRADA);
            var turmaRegularCodigo = turmasAluno.FirstOrDefault(x => x.TipoTurma == TipoTurma.Regular)?.TurmaCodigo ?? turmasAluno.FirstOrDefault(t => t.TurmaCodigo == turmaComplementar.CodigoTurma && !string.IsNullOrEmpty(t.RegularCodigo))?.RegularCodigo;
            if (!string.IsNullOrEmpty(turmaRegularCodigo))
                return (await mediator.Send(new ObterTurmasPorCodigosQuery(new string[] { turmaRegularCodigo }))).FirstOrDefault();
            return null;
        }

        private async Task<bool> TipoNotaEhConceito(Turma turma, int bimestre)
        {
            if (bimestre == 0)
                bimestre = turma.ModalidadeTipoCalendario.EhEjaOuCelp() ? BIMESTRE_2 : BIMESTRE_4;

            var periodoEscolar = await mediator.Send(new ObterPeriodoEscolarPorTurmaBimestreQuery(turma, bimestre));
            var tipoNota = await mediator.Send(new ObterNotaTipoPorAnoModalidadeDataReferenciaQuery(turma.Ano, turma.ModalidadeCodigo, periodoEscolar.NaoEhNulo() ? periodoEscolar.PeriodoFim : DateTimeExtension.HorarioBrasilia().Date));
            if (tipoNota.EhNulo())
                throw new NegocioException(MensagemNegocioTurma.NAO_FOI_POSSIVEL_IDENTIFICAR_TIPO_NOTA_TURMA);
            return tipoNota.TipoNota == TipoNota.Conceito;
        }
        private static long? ConverterNotaConceito(double? nota, long? conceitoId)
        {
            if (!nota.HasValue)
                return conceitoId;
            else if (nota < NOTA_CONCEITO_CINCO)
                return (long)ConceitoValores.NS;
            else if (nota is >= NOTA_CONCEITO_CINCO and < NOTA_CONCEITO_SETE)
                return (long)ConceitoValores.S;
            else if (nota is >= NOTA_CONCEITO_SETE)
                return (long)ConceitoValores.P;
            else return conceitoId;
        }

        private async Task<string[]> ObterTurmasComplementaresEOL(Turma turma, string codigoAluno)
        {
            var turmasItinerarioEnsinoMedio = await mediator.Send(ObterTurmaItinerarioEnsinoMedioQuery.Instance);

            if (turma.DeveVerificarRegraRegulares() || (turmasItinerarioEnsinoMedio?.Any(a => a.Id == (int)turma.TipoTurma) ?? false))
            {
                var tiposParaConsulta = new List<int> { (int)turma.TipoTurma };
                var tiposRegularesDiferentes = turma.ObterTiposRegularesDiferentes();

                tiposParaConsulta.AddRange(tiposRegularesDiferentes.Where(c => tiposParaConsulta.All(x => x != c)));

                if (turmasItinerarioEnsinoMedio.NaoEhNulo())
                    tiposParaConsulta.AddRange(turmasItinerarioEnsinoMedio.Select(s => s.Id).Where(c => tiposParaConsulta.All(x => x != c)));

                var turmasCodigosComplementares = await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(turma.AnoLetivo, codigoAluno, tiposParaConsulta, null, null, (turma.Semestre != 0 ? turma.Semestre : null)));

                if (turmasCodigosComplementares.Any())
                {
                    var turmasComplementares = turmasCodigosComplementares.Select(s => s).Except(new string[] { turma.CodigoTurma }).ToArray();
                    if (turmasComplementares.Any())
                        return turmasComplementares;
                }
            }

            return Array.Empty<string>();
        }

        private async Task<bool> SalvarConsolidacaoConselhoClasseNota(Turma turma, long componenteCurricularId, long consolidadoTurmaAlunoId,
                                                                      IEnumerable<NotaConceitoBimestreComponenteDto> notaConceitoBimestreComponenteDto,
                                                                      IEnumerable<FechamentoNotaAlunoAprovacaoDto> fechamentoNotas,
                                                                      MensagemConsolidacaoConselhoClasseAlunoDto filtro,
                                                                      (double? Nota, long? ConceitoId, bool EhNotaConceitoConselhoCache) notaConceitoCache)
        {
            double? nota = null;
            double? conceito = null;

            IEnumerable<FechamentoNotaAlunoAprovacaoDto> fechamentoNotasDiciplina = null;
            IEnumerable<NotaConceitoBimestreComponenteDto> conselhoClasseNotasAluno = null;

            var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaPorIdTurmaQuery(turma.Id, filtro.Bimestre));
            if (fechamentoTurma.NaoEhNulo())
            {
                var conselhoClasse = await mediator.Send(new ObterConselhoClassePorFechamentoIdQuery(fechamentoTurma.Id));
                if (conselhoClasse.NaoEhNulo())
                    conselhoClasseNotasAluno = notaConceitoBimestreComponenteDto
                        .Where(c => c.AlunoCodigo.Equals(filtro.AlunoCodigo)
                                && c.ConselhoClasseId == conselhoClasse.Id
                                && c.ComponenteCurricularCodigo == componenteCurricularId
                                && (((filtro.Bimestre ?? 0) != 0 && c.Bimestre == filtro.Bimestre.Value) || ((filtro.Bimestre ?? 0) == 0 && !c.Bimestre.HasValue)));
            }
            var contemNotaConselhoClasse = conselhoClasseNotasAluno.NaoEhNulo() && conselhoClasseNotasAluno.Any();

            fechamentoNotasDiciplina = fechamentoNotas.Where(t => t.ComponenteCurricularId == componenteCurricularId
                                                                && (((filtro.Bimestre ?? 0) != 0 && t.Bimestre == filtro.Bimestre.Value) || ((filtro.Bimestre ?? 0) == 0 && !t.Bimestre.HasValue)));

            if (contemNotaConselhoClasse)
            {
                nota = conselhoClasseNotasAluno.FirstOrDefault().Nota;
                conceito = conselhoClasseNotasAluno.FirstOrDefault().ConceitoId;
            }

            if (nota.EhNulo() && conceito.EhNulo())
            {
                contemNotaConselhoClasse = false;
                var notaFechamento = fechamentoNotasDiciplina.FirstOrDefault();
                nota = notaFechamento?.Nota;
                conceito = notaFechamento?.ConceitoId;
            }

            if ((notaConceitoCache.Nota.NaoEhNulo() || notaConceitoCache.ConceitoId.NaoEhNulo()) &&
                (notaConceitoCache.EhNotaConceitoConselhoCache || !contemNotaConselhoClasse))
            {
                nota = notaConceitoCache.Nota;
                conceito = notaConceitoCache.ConceitoId;
            }

            var consolidadoNota = await mediator.Send(new ObterConselhoClasseConsolidadoNotaPorConsolidadoBimestreComponenteQuery(consolidadoTurmaAlunoId, filtro.Bimestre, componenteCurricularId));
            consolidadoNota ??= new ConselhoClasseConsolidadoTurmaAlunoNota()
            {
                ConselhoClasseConsolidadoTurmaAlunoId = consolidadoTurmaAlunoId,
                Bimestre = filtro.Bimestre.HasValue && filtro.Bimestre == 0 ? null : filtro.Bimestre,
            };
            consolidadoNota.ComponenteCurricularId = componenteCurricularId;
            consolidadoNota.Nota = nota;
            consolidadoNota.ConceitoId = (long?)conceito;
            await mediator.Send(new SalvarConselhoClasseConsolidadoTurmaAlunoNotaCommand(consolidadoNota));

            return true;
        }
    }
}
