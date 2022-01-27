using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private readonly IServicoEol servicoEOL;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IRepositorioNotaTipoValorConsulta repositorioNotaTipoValor;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;

        public ServicoFechamentoFinal(IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina,
                                      IRepositorioFechamentoTurma repositorioFechamentoTurma,
                                      IRepositorioFechamentoAluno repositorioFechamentoAluno,
                                      IRepositorioFechamentoNota repositorioFechamentoNota,
                                      IRepositorioTipoCalendarioConsulta repositorioTipoCalendario,
                                      IRepositorioEvento repositorioEvento,
                                      IServicoEol servicoEOL,
                                      IServicoUsuario servicoUsuario,
                                      IRepositorioNotaTipoValorConsulta repositorioNotaTipoValor,
                                      IUnitOfWork unitOfWork,
                                      IMediator mediator)
        {
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
            this.repositorioFechamentoAluno = repositorioFechamentoAluno ?? throw new ArgumentNullException(nameof(repositorioFechamentoAluno));
            this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new ArgumentNullException(nameof(repositorioFechamentoNota));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.repositorioNotaTipoValor = repositorioNotaTipoValor ?? throw new ArgumentNullException(nameof(repositorioNotaTipoValor));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<AuditoriaPersistenciaDto> SalvarAsync(FechamentoTurmaDisciplina fechamentoFinal, Turma turma, Usuario usuarioLogado, IList<FechamentoFinalSalvarItemDto> notasDto, bool emAprovacao)
        {
            var notasEmAprovacao = new List<FechamentoNotaDto>();
            var mensagens = new List<string>();

            var componenteCurricular = await ObterComponenteCurricular(fechamentoFinal.DisciplinaId);
            var tipoNota = repositorioNotaTipoValor.ObterPorTurmaId(turma.Id, turma.TipoTurma);

            unitOfWork.IniciarTransacao();
            try
            {
                var fechamentoTurmaId = await repositorioFechamentoTurma.SalvarAsync(fechamentoFinal.FechamentoTurma);
                fechamentoFinal.FechamentoTurmaId = fechamentoTurmaId;
                var fechamentoTurmaDisciplinaId = await repositorioFechamentoTurmaDisciplina.SalvarAsync(fechamentoFinal);

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

                                var fechamentoNota = MapearNotaDto(notaDto, fechamentoAluno);

                                if (emAprovacao)
                                    AdicionaAprovacaoNota(notasEmAprovacao, fechamentoNota, notaDto, fechamentoAluno.AlunoCodigo);
                                else
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
                                        if (fechamentoNota.ConceitoId != notaDto.ConceitoId)
                                            await mediator.Send(new SalvarHistoricoConceitoFechamentoCommand(fechamentoNota.ConceitoId, notaDto.ConceitoId, fechamentoNota.Id));
                                    }

                                    await repositorioFechamentoNota.SalvarAsync(fechamentoNota);
                                }
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
                await EnviarNotasAprovacao(notasEmAprovacao, fechamentoTurmaDisciplinaId, usuarioLogado, turma, componenteCurricular);
                unitOfWork.PersistirTransacao();

                if (!emAprovacao)
                    await ExcluirPendenciaAusenciaFechamento(fechamentoFinal.DisciplinaId, fechamentoFinal.FechamentoTurma.TurmaId);

                var auditoria = (AuditoriaPersistenciaDto)fechamentoFinal.FechamentoTurma;
                auditoria.Mensagens = mensagens;
                auditoria.EmAprovacao = notasEmAprovacao.Any();
                return auditoria;
            }
            catch (Exception e)
            {
                await LogarErro("Erro ao persistir notas de fechamento final", e, LogNivel.Critico);

                unitOfWork.Rollback();
                throw e;
            }
        }

        private FechamentoNota MapearNotaDto(FechamentoFinalSalvarItemDto notaDto, FechamentoAluno fechamentoAluno)
            => fechamentoAluno.FechamentoNotas.FirstOrDefault(c => c.DisciplinaId == notaDto.ComponenteCurricularCodigo) ??
                new FechamentoNota() { FechamentoAlunoId = fechamentoAluno.Id, FechamentoAluno = fechamentoAluno };

        private void AdicionaAprovacaoNota(List<FechamentoNotaDto> notasEmAprovacao, FechamentoNota fechamentoNota, FechamentoFinalSalvarItemDto notaDto, string alunoCodigo)
        {
            notasEmAprovacao.Add(new FechamentoNotaDto()
            {
                Id = fechamentoNota.Id,
                NotaAnterior = fechamentoNota.Nota != null ? fechamentoNota.Nota.Value : (double?)null,
                Nota = notaDto.Nota,
                ConceitoIdAnterior = fechamentoNota.ConceitoId != null ? fechamentoNota.ConceitoId.Value : (long?)null,
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

        private async Task EnviarNotasAprovacao(List<FechamentoNotaDto> notasEmAprovacao, long fechamentoTurmaDisciplinaId, Usuario usuarioLogado, Turma turma, DisciplinaDto componenteCurricular)
        {
            if (notasEmAprovacao.Any())
                await mediator.Send(new EnviarNotasFechamentoParaAprovacaoCommand(notasEmAprovacao, fechamentoTurmaDisciplinaId, null, usuarioLogado, componenteCurricular, turma));
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
            var professorPodePersistirTurma = await servicoEOL.ProfessorPodePersistirTurma(professorRf, turma.CodigoTurma, diaAtual);
            if (!professorPodePersistirTurma)
                throw new NegocioException("Você não pode executar alterações nesta turma.");
        }
    }
}