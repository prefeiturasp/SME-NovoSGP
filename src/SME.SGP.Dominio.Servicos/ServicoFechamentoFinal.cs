﻿using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoFechamentoFinal : IServicoFechamentoFinal
    {
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina;
        private readonly IRepositorioFechamentoTurma repositorioFechamentoTurma;
        private readonly IRepositorioFechamentoAluno repositorioFechamentoAluno;
        private readonly IRepositorioFechamentoNota repositorioFechamentoNota;
        private readonly IRepositorioTipoCalendarioConsulta repositorioTipoCalendario;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IRepositorioNotaTipoValorConsulta repositorioNotaTipoValor;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;
        private readonly IRepositorioCache repositorioCache;
        private IEnumerable<ConselhoClasseAlunosNotaPorFechamentoIdDto> conselhosClasseAlunos;
        private const int BIMESTRE_4 = 4;
        private const int BIMESTRE_2 = 2;

        public ServicoFechamentoFinal(IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina,
                                      IRepositorioFechamentoTurma repositorioFechamentoTurma,
                                      IRepositorioFechamentoAluno repositorioFechamentoAluno,
                                      IRepositorioFechamentoNota repositorioFechamentoNota,
                                      IRepositorioTipoCalendarioConsulta repositorioTipoCalendario,
                                      IRepositorioEvento repositorioEvento,
                                      IServicoUsuario servicoUsuario,
                                      IRepositorioNotaTipoValorConsulta repositorioNotaTipoValor,
                                      IUnitOfWork unitOfWork,
                                      IMediator mediator,
                                      IRepositorioCache repositorioCache)
        {
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
            this.repositorioFechamentoAluno = repositorioFechamentoAluno ?? throw new ArgumentNullException(nameof(repositorioFechamentoAluno));
            this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new ArgumentNullException(nameof(repositorioFechamentoNota));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.repositorioNotaTipoValor = repositorioNotaTipoValor ?? throw new ArgumentNullException(nameof(repositorioNotaTipoValor));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<AuditoriaPersistenciaDto> SalvarAsync(FechamentoTurmaDisciplina fechamentoFinal, Turma turma, Usuario usuarioLogado, IList<FechamentoFinalSalvarItemDto> notasDto, bool emAprovacao)
        {
            var notasEmAprovacao = new List<FechamentoNotaDto>();
            var mensagens = new List<string>();

            if (!turma.EhTurmaEdFisicaOuItinerario() && !usuarioLogado.EhGestorEscolar() && !usuarioLogado.EhPerfilSME() && !usuarioLogado.EhPerfilDRE())
                await VerificaSeProfessorPodePersistirTurma(turma.CodigoTurma, fechamentoFinal.DisciplinaId, usuarioLogado);

            var mesmoAnoLetivo = turma.AnoLetivo == DateTimeExtension.HorarioBrasilia().Year;
            var bimestre = turma.EhEJA() ? BIMESTRE_2 : BIMESTRE_4;
            var temPeriodoAberto = await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTimeExtension.HorarioBrasilia().Date, bimestre, mesmoAnoLetivo));

            if (!temPeriodoAberto)
                throw new NegocioException(MensagemNegocioComuns.APENAS_EH_POSSIVEL_CONSULTAR_ESTE_REGISTRO_POIS_O_PERIODO_NAO_ESTA_EM_ABERTO);

            await ObterComponenteCurricular(fechamentoFinal.DisciplinaId, turma.CodigoTurma);
            var tipoNota = await repositorioNotaTipoValor.ObterPorTurmaIdAsync(turma.Id, turma.TipoTurma);

            var consolidacaoNotasAlunos = new List<ConsolidacaoNotaAlunoDto>();
            conselhosClasseAlunos = (await mediator.Send(new ObterConselhoClasseAlunosNotaPorFechamentoIdQuery(fechamentoFinal.FechamentoTurmaId))).ToList();

            unitOfWork.IniciarTransacao();
            try
            {
                var fechamentoTurmaId = await repositorioFechamentoTurma.SalvarAsync(fechamentoFinal.FechamentoTurma);
                fechamentoFinal.FechamentoTurmaId = fechamentoTurmaId;

                var fechamentoTurmaDisciplinaId = await repositorioFechamentoTurmaDisciplina.SalvarAsync(fechamentoFinal);
                var fechamentosNotasCache = new Dictionary<FechamentoAluno, List<FechamentoNota>>();

                foreach (var fechamentoAluno in fechamentoFinal.FechamentoAlunos)
                {
                    try
                    {
                        fechamentoAluno.FechamentoTurmaDisciplinaId = fechamentoTurmaDisciplinaId;
                        await repositorioFechamentoAluno.SalvarAsync(fechamentoAluno);
                        fechamentosNotasCache.Add(fechamentoAluno, new List<FechamentoNota>());

                        foreach (var notaDto in notasDto.Where(a => a.AlunoRf == fechamentoAluno.AlunoCodigo))
                        {
                            try
                            {
                                // Regra de não reprovação em 2020
                                if (turma.AnoLetivo == 2020)
                                    ValidarNotasFechamento2020(notaDto);

                                var fechamentoNota = CarregarNota(notaDto, fechamentoAluno);
                                
                                //-> Caso não estiver em aprovação ou estiver em aprovação e não houver qualquer lançamento de nota de fechamento,
                                //   deve gerar o registro do fechamento da nota inicial.
                                if (!emAprovacao || (emAprovacao && (fechamentoNota.Id == 0)))
                                {
                                    // Registra Histórico de alteração de nota
                                    if (fechamentoNota != null)
                                    {
                                        if (tipoNota.TipoNota == TipoNota.Nota)
                                        {
                                            if (fechamentoNota.Nota.HasValue && fechamentoNota.Nota.GetValueOrDefault().CompareTo(notaDto.Nota) != 0)
                                                await mediator.Send(new SalvarHistoricoNotaFechamentoCommand(fechamentoNota.Nota, notaDto.Nota, fechamentoNota.Id));
                                        }
                                        else
                                        if (fechamentoNota.ConceitoId != null && fechamentoNota.ConceitoId != notaDto.ConceitoId)
                                            await mediator.Send(new SalvarHistoricoConceitoFechamentoCommand(fechamentoNota.ConceitoId, notaDto.ConceitoId, fechamentoNota.Id));
                                    }

                                    if (!emAprovacao)
                                    {
                                        fechamentoNota.Nota = notaDto.Nota;
                                        fechamentoNota.ConceitoId = notaDto.ConceitoId;
                                    }

                                    fechamentoNota.SinteseId = notaDto.SinteseId;
                                    fechamentoNota.DisciplinaId = notaDto.ComponenteCurricularCodigo;

                                    await repositorioFechamentoNota.SalvarAsync(fechamentoNota);

                                    fechamentosNotasCache[fechamentoAluno].Add(fechamentoNota);

                                    ConsolidacaoNotasAlunos(consolidacaoNotasAlunos, turma, fechamentoAluno.AlunoCodigo, fechamentoNota);
                                }

                                if (emAprovacao)
                                    AdicionaAprovacaoNota(notasEmAprovacao, fechamentoNota, notaDto, fechamentoAluno.AlunoCodigo);
                            }
                            catch (NegocioException e)
                            {
                                var mensagem = $"Não foi possível salvar a nota do componente [{notaDto.ComponenteCurricularCodigo}] aluno [{fechamentoAluno.AlunoCodigo}]";
                                await LogarErro(mensagem, e, LogNivel.Negocio);
                                mensagens.Add(e.Message);
                            }
                            catch (Exception e)
                            {
                                var mensagem = $"Não foi possível salvar a nota do componente [{notaDto.ComponenteCurricularCodigo}] aluno [{fechamentoAluno.AlunoCodigo}]";
                                await LogarErro(mensagem, e, LogNivel.Critico);
                                mensagens.Add(mensagem);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        var mensagem = $"Não foi possível gravar o fechamento do aluno [{fechamentoAluno.AlunoCodigo}]";
                        await LogarErro(mensagem, e, LogNivel.Critico);
                        mensagens.Add(mensagem);
                    }
                }

                await EnviarNotasAprovacao(notasEmAprovacao, usuarioLogado);
                unitOfWork.PersistirTransacao();

                await AtualizarCache(fechamentoFinal, turma, emAprovacao, fechamentosNotasCache);

                foreach (var consolidacaoNotaAluno in consolidacaoNotasAlunos)
                    await mediator.Send(new ConsolidacaoNotaAlunoCommand(consolidacaoNotaAluno));

                if (!emAprovacao)
                    await ExcluirPendenciaAusenciaFechamento(fechamentoFinal.DisciplinaId, fechamentoFinal.FechamentoTurma.TurmaId);

                var auditoria = (AuditoriaPersistenciaDto)fechamentoFinal.FechamentoTurma;
                auditoria.Mensagens = mensagens;
                auditoria.EmAprovacao = notasEmAprovacao.Any();

                auditoria.MensagemConsistencia = emAprovacao
                    ? string.Format(
                        MensagemNegocioFechamentoNota.REGISTRADO_COM_SUCESSO_EM_24_HORAS_SERA_ENVIADO_PARA_APROVACAO,
                        tipoNota.TipoNota.Name())
                    : "Fechamento final salvo com sucesso.";

                return auditoria;
            }
            catch (Exception e)
            {
                await LogarErro("Erro ao persistir notas de fechamento final", e, LogNivel.Critico);

                unitOfWork.Rollback();
                throw e;
            }
        }

        private async Task AtualizarCache(FechamentoTurmaDisciplina fechamentoFinal, Turma turma, bool emAprovacao, Dictionary<FechamentoAluno,List<FechamentoNota>> fechamentosNotasCache)
        {
            foreach (var fechamentoAluno in fechamentosNotasCache.Keys)
            {
                foreach (var fechamentoNota in fechamentosNotasCache[fechamentoAluno])
                {
                    var nomeChaveCache = ObterChaveFechamentoNotaFinalComponenteTurma(fechamentoFinal.DisciplinaId.ToString(), turma.CodigoTurma);

                    var notasFechamentoFinaisNoCache = await repositorioCache.ObterObjetoAsync<List<FechamentoNotaAlunoAprovacaoDto>>(nomeChaveCache);

                    if (notasFechamentoFinaisNoCache != null)
                        await PersistirNotasFinaisNoCache(notasFechamentoFinaisNoCache, fechamentoNota,fechamentoAluno.AlunoCodigo, fechamentoFinal.DisciplinaId.ToString(), turma.CodigoTurma, emAprovacao);

                    nomeChaveCache = ObterChaveNotaConceitoFechamentoTurmaBimestreFinal(turma.CodigoTurma);

                    var notasConceitosFechamento = await repositorioCache.ObterObjetoAsync<List<NotaConceitoBimestreComponenteDto>>(nomeChaveCache);

                    if (notasConceitosFechamento != null)
                        await PersistirNotaConceitoBimestreNoCache(notasConceitosFechamento, fechamentoNota, fechamentoAluno.AlunoCodigo, turma.CodigoTurma);
                }
            }
        }

        private async Task VerificaSeProfessorPodePersistirTurma(string turmaCodigo, long disciplinaId, Usuario usuario)
        {
            var podePersistir = true;

            if (!usuario.EhProfessorCj())
                podePersistir = await mediator.Send(new ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery(disciplinaId, turmaCodigo, DateTimeExtension.HorarioBrasilia(), usuario));

            if (!podePersistir)
                throw new NegocioException(MensagemNegocioComuns.Voce_nao_pode_fazer_alteracoes_ou_inclusoes_nesta_turma_componente_e_data);
        }

        private static void ConsolidacaoNotasAlunos(List<ConsolidacaoNotaAlunoDto> consolidacaoNotasAlunos, Turma turma, string AlunoCodigo, FechamentoNota fechamentoNota)
        {
            consolidacaoNotasAlunos.Add(new ConsolidacaoNotaAlunoDto()
            {
                AlunoCodigo = AlunoCodigo,
                TurmaId = turma.Id,
                AnoLetivo = turma.AnoLetivo,
                Nota = fechamentoNota.Nota,
                ConceitoId = fechamentoNota.ConceitoId,
                ComponenteCurricularId = fechamentoNota.DisciplinaId
            });
        }

        private static FechamentoNota CarregarNota(FechamentoFinalSalvarItemDto notaDto, FechamentoAluno fechamentoAluno)
            => fechamentoAluno.FechamentoNotas.FirstOrDefault(c => c.DisciplinaId == notaDto.ComponenteCurricularCodigo) ??
                new FechamentoNota() { FechamentoAlunoId = fechamentoAluno.Id, FechamentoAluno = fechamentoAluno };

        private static void AdicionaAprovacaoNota(List<FechamentoNotaDto> notasEmAprovacao, FechamentoNota fechamentoNota, FechamentoFinalSalvarItemDto notaDto, string alunoCodigo)
        {
            notasEmAprovacao.Add(new FechamentoNotaDto()
            {
                Id = fechamentoNota.Id,
                NotaAnterior = fechamentoNota.Nota,
                Nota = notaDto.Nota,
                ConceitoIdAnterior = fechamentoNota.ConceitoId,
                ConceitoId = notaDto.ConceitoId,
                CodigoAluno = alunoCodigo
            });
        }

        private async Task<DisciplinaDto> ObterComponenteCurricular(long componenteCurricularId, string codigoTurma)
        {
            var componentes = await mediator.Send(new ObterComponentesCurricularesPorIdsUsuarioLogadoQuery(new long[] { componenteCurricularId }, codigoTurma: codigoTurma));

            if (!componentes.Any())
                throw new NegocioException($"Componente Curricular do Fechamento ({componenteCurricularId}) não localizado!");

            return componentes.FirstOrDefault();
        }

        private async Task EnviarNotasAprovacao(List<FechamentoNotaDto> notasEmAprovacao, Usuario usuarioLogado)
        {
            if (notasEmAprovacao.Any())
                await mediator.Send(new EnviarNotasFechamentoParaAprovacaoCommand(notasEmAprovacao, usuarioLogado));
        }

        private Task LogarErro(string mensagem, Exception e, LogNivel nivel)
            => mediator.Send(new SalvarLogViaRabbitCommand(mensagem, nivel, LogContexto.Fechamento, e.Message));

        private void ValidarNotasFechamento2020(FechamentoFinalSalvarItemDto notaDto)
        {
            if (notaDto.ConceitoId.HasValue && notaDto.ConceitoId.Value == 3)
                throw new NegocioException("Não é possível atribuir conceito NS (Não Satisfatório) pois em 2020 não há retenção dos estudantes conforme o Art 5º da LEI Nº 17.437 DE 12 DE AGOSTO DE 2020.");
            else
            if (!notaDto.SinteseId.HasValue && notaDto.Nota < 5)
                throw new NegocioException("Não é possível atribuir uma nota menor que 5 pois em 2020 não há retenção dos estudantes conforme o Art 5º da LEI Nº 17.437 DE 12 DE AGOSTO DE 2020.");
        }

        private async Task ExcluirPendenciaAusenciaFechamento(long disciplinaId, long turmaId)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            await mediator.Send(new PublicaFilaExcluirPendenciaAusenciaFechamentoCommand(disciplinaId, null, turmaId, usuarioLogado));
        }

        public async Task VerificaPersistenciaGeral(Turma turma)
        {
            var tipoCalendario = await repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(turma.AnoLetivo, turma.ObterModalidadeTipoCalendario(), turma.Semestre);
            if (tipoCalendario == null)
                throw new NegocioException("Não foi possível localizar o tipo de calendário.");

            var diaAtual = DateTime.Today;

            var eventoFechamento = await repositorioEvento.EventosNosDiasETipo(diaAtual, diaAtual, TipoEvento.FechamentoBimestre, tipoCalendario.Id, turma.Ue.CodigoUe, turma.Ue.Dre.CodigoDre);
            if (eventoFechamento == null || !eventoFechamento.Any())
                throw new NegocioException("Não foi possível localizar um fechamento de período ou reabertura para esta turma.");

            var professorRf = servicoUsuario.ObterRf();

            var professorPodePersistirTurma =
                await mediator.Send(new ProfessorPodePersistirTurmaQuery(professorRf, turma.CodigoTurma, diaAtual));

            if (!professorPodePersistirTurma)
                throw new NegocioException("Você não pode executar alterações nesta turma.");
        }

        private async Task PersistirNotasFinaisNoCache(List<FechamentoNotaAlunoAprovacaoDto> notasFinais, FechamentoNota fechamentoNota,
            string codigoAluno, string disciplinaId, string codigoTurma, bool emAprovacao)
        {
            var notaFinalAluno = notasFinais.FirstOrDefault(c => c.AlunoCodigo == codigoAluno && c.ComponenteCurricularId == fechamentoNota.DisciplinaId);

            if (notaFinalAluno == null) {
                notasFinais.Add(new FechamentoNotaAlunoAprovacaoDto
                {
                    Bimestre = null,
                    Nota = fechamentoNota.Nota,
                    AlunoCodigo = codigoAluno,
                    ConceitoId = fechamentoNota.ConceitoId,
                    EmAprovacao = emAprovacao,
                    ComponenteCurricularId = fechamentoNota.DisciplinaId
                });
            }
            else
            {
                notaFinalAluno.Nota = fechamentoNota.Nota;
                notaFinalAluno.ConceitoId = fechamentoNota.ConceitoId;
                notaFinalAluno.EmAprovacao = emAprovacao;
            }

            await mediator.Send(new SalvarCachePorValorObjetoCommand(ObterChaveFechamentoNotaFinalComponenteTurma(disciplinaId, codigoTurma), notasFinais));
        }

        private static string ObterChaveFechamentoNotaFinalComponenteTurma(string codigoDisciplina, string codigoTurma)
        {
            return string.Format(NomeChaveCache.CHAVE_FECHAMENTO_NOTA_FINAL_COMPONENTE_TURMA, codigoDisciplina,
                codigoTurma);
        }

        private static string ObterChaveNotaConceitoFechamentoTurmaBimestreFinal(string codigoTurma)
        {
            return string.Format(NomeChaveCache.CHAVE_NOTA_CONCEITO_FECHAMENTO_TURMA_TODOS_BIMESTRES_E_FINAL, codigoTurma);
        }

        private async Task PersistirNotaConceitoBimestreNoCache(List<NotaConceitoBimestreComponenteDto> notasConceitosFechamento,
            FechamentoNota fechamentoNota, string codigoAluno, string codigoTurma)
        {
            var notaConceitoFechamentoAluno = notasConceitosFechamento.FirstOrDefault(c => c.AlunoCodigo == codigoAluno &&
                c.ComponenteCurricularCodigo == fechamentoNota.DisciplinaId && c.Bimestre is 0 or null);

            if (notaConceitoFechamentoAluno == null)
                notasConceitosFechamento.Add(ObterNotaConceitoBimestreAluno(codigoAluno, fechamentoNota.DisciplinaId, codigoTurma, fechamentoNota));
            else
            {
                notaConceitoFechamentoAluno.Nota = fechamentoNota.Nota;
                notaConceitoFechamentoAluno.ConceitoId = fechamentoNota.ConceitoId;
            }

            await mediator.Send(new SalvarCachePorValorObjetoCommand(ObterChaveNotaConceitoFechamentoTurmaBimestreFinal(codigoTurma), notasConceitosFechamento));
        }

        private NotaConceitoBimestreComponenteDto ObterNotaConceitoBimestreAluno(string codigoAluno, 
                                                                                 long codigoDisciplina, 
                                                                                 string codigoTurma, 
                                                                                 FechamentoNota fechamentoNota)
        {
            var conselho = conselhosClasseAlunos.ToList().Find(ca => ca.AlunoCodigo == codigoAluno &&
                                                               ca.ComponenteCurricularCodigo == codigoDisciplina);
            return new NotaConceitoBimestreComponenteDto
            {
                AlunoCodigo = codigoAluno,
                Nota = fechamentoNota.Nota,
                ConceitoId = fechamentoNota.ConceitoId,
                ComponenteCurricularCodigo = codigoDisciplina,
                TurmaCodigo = codigoTurma,
                Bimestre = null,
                ConselhoClasseNotaId = conselho != null ? conselho.ConselhoClasseNotaId : 0,
                ConselhoClasseId = conselho != null ? conselho.ConselhoClasseId : 0
            };
        }
    }
}