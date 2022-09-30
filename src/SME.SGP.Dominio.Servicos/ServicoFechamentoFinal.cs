using MediatR;
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

            var componenteCurricular = await ObterComponenteCurricular(fechamentoFinal.DisciplinaId);
            var tipoNota = repositorioNotaTipoValor.ObterPorTurmaId(turma.Id, turma.TipoTurma);

            var consolidacaoNotasAlunos = new List<ConsolidacaoNotaAlunoDto>();

            unitOfWork.IniciarTransacao();
            try
            {
                var fechamentoTurmaId = await repositorioFechamentoTurma.SalvarAsync(fechamentoFinal.FechamentoTurma);
                fechamentoFinal.FechamentoTurmaId = fechamentoTurmaId;

                var fechamentoTurmaDisciplinaId = await repositorioFechamentoTurmaDisciplina.SalvarAsync(fechamentoFinal);
                var listaNotasFechamento = new List<FechamentoNota>();

                foreach (var fechamentoAluno in fechamentoFinal.FechamentoAlunos)
                {
                    try
                    {
                        fechamentoAluno.FechamentoTurmaDisciplinaId = fechamentoTurmaDisciplinaId;
                        var fechamentoAlunoId = await repositorioFechamentoAluno.SalvarAsync(fechamentoAluno);

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
                                            if (fechamentoNota.Nota.HasValue && fechamentoNota.Nota != notaDto.Nota)
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

                                    var notasFechamentoFinaisNoCache = await repositorioCache.ObterObjetoAsync<List<FechamentoNotaAlunoAprovacaoDto>>($"fechamentoNotaFinais-{fechamentoFinal.DisciplinaId}-{turma.CodigoTurma}");

                                    if (notasFechamentoFinaisNoCache != null)
                                        await PersistirNotasFinaisNoCache(notasFechamentoFinaisNoCache, fechamentoNota, fechamentoAluno.AlunoCodigo, fechamentoFinal.DisciplinaId.ToString(), turma.CodigoTurma);

                                    await repositorioFechamentoNota.SalvarAsync(fechamentoNota);

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

                foreach (var consolidacaoNotaAluno in consolidacaoNotasAlunos)
                    await mediator.Send(new ConsolidacaoNotaAlunoCommand(consolidacaoNotaAluno));

                if (!emAprovacao)
                    await ExcluirPendenciaAusenciaFechamento(fechamentoFinal.DisciplinaId, fechamentoFinal.FechamentoTurma.TurmaId);

                var auditoria = (AuditoriaPersistenciaDto)fechamentoFinal.FechamentoTurma;
                auditoria.Mensagens = mensagens;
                auditoria.EmAprovacao = notasEmAprovacao.Any();

                if (emAprovacao)
                    auditoria.MensagemConsistencia = string.Format(MensagemNegocioFechamentoNota.REGISTRADO_COM_SUCESSO_EM_24_HORAS_SERA_ENVIADO_PARA_APROVACAO, tipoNota.TipoNota.Name());
                else
                    auditoria.MensagemConsistencia = $"Fechamento final salvo com sucesso.";

                return auditoria;
            }
            catch (Exception e)
            {
                await LogarErro("Erro ao persistir notas de fechamento final", e, LogNivel.Critico);

                unitOfWork.Rollback();
                throw e;
            }
        }

        private async Task VerificaSeProfessorPodePersistirTurma(string turmaCodigo, long disciplinaId, Usuario usuario)
        {
            var podePersistir = true;

            if (!usuario.EhProfessorCj())
                podePersistir = await mediator.Send(new ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery(disciplinaId, turmaCodigo, DateTimeExtension.HorarioBrasilia(), usuario));

            if (!podePersistir)
                throw new NegocioException(MensagemNegocioFechamentoNota.VOCE_NAO_PODE_FAZER_ALTERACOES_OU_INCLUSOES_NESTA_TURMA_COMPONENTE_E_DATA);
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

        private async Task<DisciplinaDto> ObterComponenteCurricular(long componenteCurricularId)
        {
            var componentes = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(new long[] { componenteCurricularId }));

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

        private async Task PersistirNotasFinaisNoCache(IEnumerable<FechamentoNotaAlunoAprovacaoDto> notasFinais, FechamentoNota fechamentoNota, string codigoAluno, string codigoDisciplina, string codigoTurma)
        {
            foreach (var notaFinal in notasFinais)
            {
                if (notaFinal.AlunoCodigo != codigoAluno) 
                    continue;
                
                notaFinal.Nota = fechamentoNota.Nota;
                notaFinal.ConceitoId = fechamentoNota.ConceitoId;
            }
            
            await mediator.Send(new SalvarCachePorValorObjetoCommand(string.Format(NomeChaveCache.CHAVE_FECHAMENTO_NOTA_FINAL_COMPONENTE_TURMA, codigoDisciplina, codigoTurma), notasFinais));
        }
    }
}