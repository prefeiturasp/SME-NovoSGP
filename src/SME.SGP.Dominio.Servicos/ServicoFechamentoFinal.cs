using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IServicoEol servicoEOL;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IUnitOfWork unitOfWork;
        private readonly IServicoLog servicoLog;
        private readonly IMediator mediator;

        public ServicoFechamentoFinal(IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina,
                                      IRepositorioFechamentoTurma repositorioFechamentoTurma,
                                      IRepositorioFechamentoAluno repositorioFechamentoAluno,
                                      IRepositorioFechamentoNota repositorioFechamentoNota,
                                      IRepositorioTipoCalendario repositorioTipoCalendario,
                                      IRepositorioEvento repositorioEvento,
                                      IServicoEol servicoEOL,
                                      IServicoUsuario servicoUsuario,
                                      IUnitOfWork unitOfWork,
                                      IServicoLog servicoLog,
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
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<List<string>> SalvarAsync(FechamentoTurmaDisciplina fechamentoFinal, Turma turma)
        {
            var mensagens = new List<string>();
            unitOfWork.IniciarTransacao();
            try
            {
                var fechamentoTurmaId = await repositorioFechamentoTurma.SalvarAsync(fechamentoFinal.FechamentoTurma);
                fechamentoFinal.FechamentoTurmaId = fechamentoTurmaId;
                var fechamentoTurmaDisciplinaId = await repositorioFechamentoTurmaDisciplina.SalvarAsync(fechamentoFinal);

                foreach(var fechamentoAluno in fechamentoFinal.FechamentoAlunos)
                {
                    try
                    {
                        fechamentoAluno.FechamentoTurmaDisciplinaId = fechamentoTurmaDisciplinaId;
                        var fechamentoAlunoId = await repositorioFechamentoAluno.SalvarAsync(fechamentoAluno);

                        foreach (var fechamentoNota in fechamentoAluno.FechamentoNotas)
                        {
                            try
                            {
                                if (turma.AnoLetivo == 2020)
                                    ValidarNotasFechamento2020(fechamentoNota);

                                fechamentoNota.FechamentoAlunoId = fechamentoAlunoId;
                                await repositorioFechamentoNota.SalvarAsync(fechamentoNota);
                            }
                            catch(NegocioException e)
                            {
                                servicoLog.Registrar(e);
                                mensagens.Add(e.Message);
                            }
                            catch (Exception e)
                            {
                                servicoLog.Registrar(e);
                                mensagens.Add($"Não foi possível salvar a nota do componente [{fechamentoNota.DisciplinaId}] aluno [{fechamentoAluno.AlunoCodigo}]");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        servicoLog.Registrar(e);
                        mensagens.Add($"Não foi possível gravar o fechamento do aluno [{fechamentoAluno.AlunoCodigo}]");
                    }                
                }
                unitOfWork.PersistirTransacao();

                await ExcluirPendenciaAusenciaFechamento(fechamentoFinal.DisciplinaId, fechamentoFinal.FechamentoTurma.TurmaId);

                return mensagens;
            }
            catch (Exception e)
            {
                servicoLog.Registrar(e);

                unitOfWork.Rollback();
                throw e;
            }
        }

        private void ValidarNotasFechamento2020(FechamentoNota fechamentoNota)
        {
            if (fechamentoNota.ConceitoId.HasValue && fechamentoNota.ConceitoId.Value == 3)
                throw new NegocioException("Não é possível atribuir conceito NS (Não Satisfatório) pois em 2020 não há retenção dos estudantes conforme o Art 5º da LEI Nº 17.437 DE 12 DE AGOSTO DE 2020.");
            else
            if (!fechamentoNota.SinteseId.HasValue && fechamentoNota.Nota < 5)
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