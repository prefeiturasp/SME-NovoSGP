﻿using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarConsolidacaoTurmaConselhoClasseAlunoUseCase : AbstractUseCase, IExecutarConsolidacaoTurmaConselhoClasseAlunoUseCase
    {
        private readonly long COMPONENTE_CURRICULAR_CODIGO_ED_FISICA = 6;
        private const int BIMESTRE_2 = 2;
        private const int BIMESTRE_4 = 4;
        private const double NOTA_CONCEITO_CINCO = 5.0;
        private const double NOTA_CONCEITO_SETE = 7.0;
        private readonly IRepositorioCache repositorioCache;

        public ExecutarConsolidacaoTurmaConselhoClasseAlunoUseCase(IMediator mediator, IRepositorioCache repositorioCache) : base(mediator)
        {
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<MensagemConsolidacaoConselhoClasseAlunoDto>();

            if (filtro == null)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível iniciar a consolidação do conselho de clase da turma -> aluno. O id da turma bimestre aluno não foram informados", LogNivel.Critico, LogContexto.ConselhoClasse));
                return false;
            }

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(filtro.TurmaId));
            var ue = turma.Ue;
            var turmasCodigos = new List<string> { turma.CodigoTurma };
            if (turma.EhEJA() || turma.EhTurmaEnsinoMedio)
            {
                var codigosComplementares = await ObterTurmasComplementares(turma, ue, filtro.AlunoCodigo);
                turmasCodigos.AddRange(codigosComplementares);
                if (!turma.EhTurmaRegular())
                {
                    var turmaRegular = await ObterTurmaRegular(codigosComplementares, turma.Semestre, turma.EhEJA(), turma.EhTurmaEnsinoMedio);
                    if (turmaRegular != null)
                    {
                        turma = turmaRegular;
                        filtro.TurmaId = turma.Id;
                    }
                }
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
                                .Send(new ObterComponentesComNotaDeFechamentoOuConselhoQuery(turma.AnoLetivo, turmasCodigos.ToArray(), filtro.Bimestre, filtro.AlunoCodigo))).ToList();

            if (filtro.ComponenteCurricularId.HasValue && filtro.ComponenteCurricularId != 0)
                if (!componentesComNotaFechamentoOuConselho.Any(cc => cc.Codigo.Equals(filtro.ComponenteCurricularId.ToString())))
                    componentesComNotaFechamentoOuConselho.Add(new ComponenteCurricularDto() { Codigo = filtro.ComponenteCurricularId.ToString(), LancaNota = true } );
                  
            if (!filtro.Inativo)
            {
                var componentesDoAluno = await mediator
                    .Send(new ObterComponentesParaFechamentoAcompanhamentoCCAlunoQuery(filtro.AlunoCodigo, filtro.Bimestre, filtro.TurmaId));

                if (componentesDoAluno != null && componentesDoAluno.Any())
                {
                    if (!filtro.Bimestre.HasValue || filtro.Bimestre == 0)
                    {
                        var fechamento = await mediator.Send(new ObterFechamentoPorTurmaPeriodoQuery() { TurmaId = filtro.TurmaId });
                        var conselhoClasse = await mediator.Send(new ObterConselhoClassePorFechamentoIdQuery(fechamento.Id));
                        var conselhoClasseAluno = await mediator.Send(new ObterConselhoClasseAlunoPorAlunoCodigoConselhoIdQuery(conselhoClasse.Id, filtro.AlunoCodigo));
                        consolidadoTurmaAluno.ParecerConclusivoId = conselhoClasseAluno?.ConselhoClasseParecerId;
                    }

                    var codigosComplementares = await ObterTurmasComplementares(turma, ue, filtro.AlunoCodigo);
                    turmasCodigos.AddRange(codigosComplementares);
                    var componentesDaTurmaEol = await mediator
                        .Send(new ObterComponentesCurricularesEOLPorTurmasCodigoQuery(turmasCodigos.ToArray()));

                    //Excessão de disciplina ED. Fisica para modalidade EJA
                    if (turma.EhEJA())
                        componentesDaTurmaEol = componentesDaTurmaEol.Where(a => a.Codigo != "6");

                    var possuiComponentesSemNotaConceito = componentesDaTurmaEol
                        .Where(ct => ct.LancaNota && !ct.TerritorioSaber)
                        .Select(ct => ct.Codigo)
                        .Except(componentesComNotaFechamentoOuConselho.Select(cn => cn.Codigo))
                        .Any();

                    if (possuiComponentesSemNotaConceito)
                        statusNovo = SituacaoConselhoClasse.EmAndamento;
                    else
                        statusNovo = SituacaoConselhoClasse.Concluido;
                }

                if (consolidadoTurmaAluno.ParecerConclusivoId != null)
                    statusNovo = SituacaoConselhoClasse.Concluido;
            }

            consolidadoTurmaAluno.Status = statusNovo;
            consolidadoTurmaAluno.DataAtualizacao = DateTime.Now;

            try
            {
                var consolidadoTurmaAlunoId = await mediator.Send(new SalvarConselhoClasseConsolidadoTurmaAlunoCommand(consolidadoTurmaAluno));

                //Quando parecer conclusivo, não altera a nota, atualiza somente o parecerId
                if (!filtro.EhParecer)
                {
                    if (componentesComNotaFechamentoOuConselho != null && componentesComNotaFechamentoOuConselho.Any())
                    {
                        var turmasIds = await mediator.Send(new ObterTurmaIdsPorCodigosQuery(turmasCodigos.ToArray()));
                        var conselhoClasseNotas = await mediator.Send(new ObterNotasConceitosConselhoClassePorTurmasCodigosEBimestreQuery(turmasCodigos.ToArray(), filtro.Bimestre ?? 0));
                        var fechamentoNotas = await mediator.Send(new ObterNotasConceitosFechamentoPorTurmasCodigosEBimestreEAlunoCodigoQuery(turmasCodigos.ToArray(), filtro.Bimestre ?? 0, filtro.AlunoCodigo));

                        foreach (var componenteCurricular in componentesComNotaFechamentoOuConselho)
                        {
                            if (!componenteCurricular.LancaNota)
                                continue;

                            var notaConceitoCache = await ObterNotaConceitoCache(turmasIds.ToArray(), long.Parse(componenteCurricular.Codigo), filtro.Bimestre??0, filtro.AlunoCodigo);
                            await SalvarConsolidacaoConselhoClasseNota(turma, filtro.Bimestre, long.Parse(componenteCurricular.Codigo),
                                                                       filtro.AlunoCodigo, consolidadoTurmaAlunoId, conselhoClasseNotas, fechamentoNotas,
                                                                       notaConceitoCache.Item1, notaConceitoCache.Item2);
                        }
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

        private async Task<(double?, long?)> ObterNotaConceitoCache(long[] turmasId, long componenteCurricularId, int bimestre, string alunoCodigo)
        {
            var retorno = new List<ConsolidadoConselhoClasseAlunoNotaCacheDto>();
            foreach (var turma in turmasId)
            {
                var nomeChaveCache = string.Format(NomeChaveCache.CHAVE_NOTA_CONSOLIDACAO_CONSELHO_CLASSE_TURMA_COMPONENTE_BIMESTRE_ALUNO,
               turma, componenteCurricularId, bimestre, alunoCodigo);

                var retornoCacheMapeado =
                    await repositorioCache.ObterObjetoAsync<ConsolidadoConselhoClasseAlunoNotaCacheDto>(nomeChaveCache,
                        "Obter nota/conceito cache consolidação conselho classe turma/componente/bimestre/aluno");
                if (retornoCacheMapeado != null)
                    retorno.Add(retornoCacheMapeado);
            }

            var notaConselhoClasse = retorno.Where(x => x.NotaConselhoClasse.HasValue || x.ConceitoIdConselhoClasse.HasValue).FirstOrDefault();
            if (notaConselhoClasse != null)
                return (notaConselhoClasse.NotaConselhoClasse, notaConselhoClasse.ConceitoIdConselhoClasse);

            var notaFechamento = retorno.Where(x => x.NotaFechamento.HasValue || x.ConceitoIdFechamento.HasValue).FirstOrDefault();
            if (notaFechamento != null)
                return (notaFechamento.NotaFechamento, notaFechamento.ConceitoIdFechamento);

            return (null, null);
        }

        private void TratarConversaoNotaEdFisicaEJA(IEnumerable<FechamentoNotaAlunoAprovacaoDto> fechamentoNotas)
        {
            if (fechamentoNotas != null)
            {
                var fechamentosNotaDisciplinaEdFisica = fechamentoNotas.Where(fn => fn.ComponenteCurricularId.Equals(COMPONENTE_CURRICULAR_CODIGO_ED_FISICA));
                foreach (var fechamentoDisciplinaEdFisica in fechamentosNotaDisciplinaEdFisica)
                {
                    fechamentoDisciplinaEdFisica.ConceitoId = ConverterNotaConceito(fechamentoDisciplinaEdFisica.Nota, (long?)fechamentoDisciplinaEdFisica.ConceitoId);
                    fechamentoDisciplinaEdFisica.Nota = null;
                }
            }

            /*if (COMPONENTE_CURRICULAR_CODIGO_ED_FISICA.Equals(filtro.ComponenteCurricularId ?? 0))
            {
                filtro.ConceitoId = ConverterNotaConceito(filtro.Nota, filtro.ConceitoId);
                filtro.Nota = null;
            }*/
        }

        private async Task<Turma> ObterTurmaRegular(string[] codigosTurmasComplementares, int semestre, bool ehTurmaEJA, bool ehTurmaEM)
        {
            return (await mediator.Send(new ObterTurmasPorCodigosQuery(codigosTurmasComplementares))).Where(t => t.EhTurmaRegular()
                                                                                                            && t.Semestre == semestre
                                                                                                            && t.EhEJA() == ehTurmaEJA
                                                                                                            && t.EhTurmaEnsinoMedio == ehTurmaEM
                                                                                                            ).FirstOrDefault();
        }

        private async Task<bool> TipoNotaEhConceito(Turma turma, int bimestre)
        {
            var periodoEscolar = await mediator.Send(new ObterPeriodoEscolarPorTurmaBimestreQuery(turma, bimestre == 0 ? turma.ModalidadeTipoCalendario == ModalidadeTipoCalendario.EJA ? BIMESTRE_2 : BIMESTRE_4 : bimestre));
            var tipoNota = await mediator.Send(new ObterNotaTipoPorAnoModalidadeDataReferenciaQuery(turma.Ano, turma.ModalidadeCodigo, periodoEscolar.PeriodoFim));
            if (tipoNota == null)
                throw new NegocioException(MensagemNegocioTurma.NAO_FOI_POSSIVEL_IDENTIFICAR_TIPO_NOTA_TURMA);
            return tipoNota.TipoNota == TipoNota.Conceito;
        }
        private long? ConverterNotaConceito(double? nota, long? conceitoId)
        {
            if (!nota.HasValue)
                return conceitoId;
            else
            if (nota < NOTA_CONCEITO_CINCO)
                return (long)ConceitoValores.NS;
            else if (nota is >= NOTA_CONCEITO_CINCO and < NOTA_CONCEITO_SETE)
                return (long)ConceitoValores.S;
            else if (nota is >= NOTA_CONCEITO_SETE)
                return (long)ConceitoValores.P;
            else return conceitoId;
        }

        private async Task<string[]> ObterTurmasComplementares(Turma turma, Ue ue, string codigoAluno)
        {
            var turmasItinerarioEnsinoMedio = await mediator.Send(new ObterTurmaItinerarioEnsinoMedioQuery());

            if (turma.DeveVerificarRegraRegulares() || (turmasItinerarioEnsinoMedio?.Any(a => a.Id == (int)turma.TipoTurma) ?? false))
            {
                var tiposParaConsulta = new List<int> { (int)turma.TipoTurma };
                var tiposRegularesDiferentes = turma.ObterTiposRegularesDiferentes();

                tiposParaConsulta.AddRange(tiposRegularesDiferentes.Where(c => tiposParaConsulta.All(x => x != c)));

                if (turmasItinerarioEnsinoMedio != null)
                    tiposParaConsulta.AddRange(turmasItinerarioEnsinoMedio.Select(s => s.Id).Where(c => tiposParaConsulta.All(x => x != c)));

                var turmasCodigosComplementares = await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(turma.AnoLetivo, codigoAluno, tiposParaConsulta, null, null, null, (turma.Semestre != 0 ? turma.Semestre : null)));

                if (turmasCodigosComplementares.Any())
                {
                    var turmasComplementares = turmasCodigosComplementares.Select(s => s).Except(new string[] { turma.CodigoTurma }).ToArray();
                    if (turmasComplementares.Any())
                        return turmasComplementares;
                }
            }

            return new string[] { };
        }

        private async Task<bool> SalvarConsolidacaoConselhoClasseNota(Turma turma, int? bimestre, long componenteCurricularId, string alunoCodigo, long consolidadoTurmaAlunoId, 
                                                                      IEnumerable<NotaConceitoBimestreComponenteDto> notaConceitoBimestreComponenteDto, IEnumerable<FechamentoNotaAlunoAprovacaoDto> fechamentoNotas,
                                                                      double? notaCache, long? conceitoIdCache)
        {
            double? nota = notaCache;
            double? conceito = conceitoIdCache;

            if (nota == null && conceito == null)
            {
                IEnumerable<FechamentoNotaAlunoAprovacaoDto> fechamentoNotasDiciplina = null;
                IEnumerable<NotaConceitoBimestreComponenteDto> conselhoClasseNotasAluno = null;

                var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaPorIdTurmaQuery(turma.Id, bimestre));
                if (fechamentoTurma != null)
                {
                    var conselhoClasse = await mediator.Send(new ObterConselhoClassePorFechamentoIdQuery(fechamentoTurma.Id));
                    if (conselhoClasse != null)
                        conselhoClasseNotasAluno = notaConceitoBimestreComponenteDto
                            .Where(c => c.AlunoCodigo.Equals(alunoCodigo)
                                    && c.ConselhoClasseId == conselhoClasse.Id
                                    && c.ComponenteCurricularCodigo == componenteCurricularId
                                    && (((bimestre ?? 0) != 0 && c.Bimestre == bimestre.Value) || ((bimestre ?? 0) == 0 && !c.Bimestre.HasValue)));
                }
                var contemNotaConselhoClasse = conselhoClasseNotasAluno != null && conselhoClasseNotasAluno.Any();

                fechamentoNotasDiciplina = fechamentoNotas.Where(t => t.ComponenteCurricularId == componenteCurricularId
                                                                    && ((bimestre ?? 0) != 0 && t.Bimestre == bimestre.Value) || ((bimestre ?? 0) == 0 && !t.Bimestre.HasValue));

                if (contemNotaConselhoClasse)
                {
                    nota = conselhoClasseNotasAluno.FirstOrDefault().Nota;
                    conceito = conselhoClasseNotasAluno.FirstOrDefault().ConceitoId;
                }

                if (nota == null && conceito == null)
                {
                    contemNotaConselhoClasse = false;
                    var notaFechamento = fechamentoNotasDiciplina.FirstOrDefault();
                    nota = notaFechamento?.Nota;
                    conceito = notaFechamento?.ConceitoId;
                }
            }
            
            var consolidadoNota = await mediator.Send(new ObterConselhoClasseConsolidadoNotaPorConsolidadoBimestreComponenteQuery(consolidadoTurmaAlunoId, bimestre, componenteCurricularId));
            consolidadoNota ??= new ConselhoClasseConsolidadoTurmaAlunoNota()
            {
                ConselhoClasseConsolidadoTurmaAlunoId = consolidadoTurmaAlunoId,
                Bimestre = bimestre.HasValue && bimestre == 0 ? null : bimestre,
            };
            consolidadoNota.ComponenteCurricularId = componenteCurricularId;
            consolidadoNota.Nota = nota;
            consolidadoNota.ConceitoId = (long?)conceito;
            await mediator.Send(new SalvarConselhoClasseConsolidadoTurmaAlunoNotaCommand(consolidadoNota));

            return true;
        }
    }
}
