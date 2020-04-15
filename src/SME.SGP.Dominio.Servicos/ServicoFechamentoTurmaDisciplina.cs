using Microsoft.Extensions.Configuration;
using SME.Background.Core;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoFechamentoTurmaDisciplina : IServicoFechamentoTurmaDisciplina
    {
        private readonly IComandosWorkflowAprovacao comandosWorkflowAprovacao;
        private readonly IConfiguration configuration;
        private readonly IConsultasDisciplina consultasDisciplina;
        private readonly IConsultasFrequencia consultasFrequencia;
        private readonly IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina;
        private readonly IRepositorioAtividadeAvaliativaRegencia repositorioAtividadeAvaliativaRegencia;
        private readonly IRepositorioConceito repositorioConceito;
        private readonly IRepositorioDre repositorioDre;
        private readonly IRepositorioFechamentoAluno repositorioFechamentoAluno;
        private readonly IRepositorioFechamentoNota repositorioFechamentoNota;
        private readonly IRepositorioFechamentoTurma repositorioFechamentoTurma;
        private readonly IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina;
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioTipoAvaliacao repositorioTipoAvaliacao;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IRepositorioUe repositorioUe;
        private readonly IRepositorioWfAprovacaoNotaFechamento repositorioWfAprovacaoNotaFechamento;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IServicoPendenciaFechamento servicoPendenciaFechamento;
        private readonly IServicoPeriodoFechamento servicoPeriodoFechamento;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IUnitOfWork unitOfWork;
        private List<FechamentoNotaDto> notasEnvioWfAprovacao;
        private Turma turmaFechamento;

        public ServicoFechamentoTurmaDisciplina(IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina,
                                                IRepositorioFechamentoTurma repositorioFechamentoTurma,
                                                IRepositorioFechamentoAluno repositorioFechamentoAluno,
                                                IRepositorioFechamentoNota repositorioFechamentoNota,
                                                IRepositorioDre repositorioDre,
                                                IRepositorioTurma repositorioTurma,
                                                IRepositorioUe repositorioUe,
                                                IServicoPeriodoFechamento servicoPeriodoFechamento,
                                                IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                                                IRepositorioTipoCalendario repositorioTipoCalendario,
                                                IRepositorioTipoAvaliacao repositorioTipoAvaliacao,
                                                IRepositorioAtividadeAvaliativaRegencia repositorioAtividadeAvaliativaRegencia,
                                                IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina,
                                                IRepositorioParametrosSistema repositorioParametrosSistema,
                                                IRepositorioConceito repositorioConceito,
                                                IRepositorioWfAprovacaoNotaFechamento repositorioWfAprovacaoNotaFechamento,
                                                IConsultasDisciplina consultasDisciplina,
                                                IConsultasFrequencia consultasFrequencia,
                                                IServicoNotificacao servicoNotificacao,
                                                IServicoPendenciaFechamento servicoPendenciaFechamento,
                                                IServicoEOL servicoEOL,
                                                IServicoUsuario servicoUsuario,
                                                IComandosWorkflowAprovacao comandosWorkflowAprovacao,
                                                IUnitOfWork unitOfWork,
                                                IConfiguration configuration)
        {
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
            this.repositorioFechamentoAluno = repositorioFechamentoAluno ?? throw new ArgumentNullException(nameof(repositorioFechamentoAluno));
            this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new ArgumentNullException(nameof(repositorioFechamentoNota));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.servicoPeriodoFechamento = servicoPeriodoFechamento ?? throw new ArgumentNullException(nameof(servicoPeriodoFechamento));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioTipoAvaliacao = repositorioTipoAvaliacao ?? throw new ArgumentNullException(nameof(repositorioTipoAvaliacao));
            this.repositorioAtividadeAvaliativaRegencia = repositorioAtividadeAvaliativaRegencia ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativaRegencia));
            this.repositorioAtividadeAvaliativaDisciplina = repositorioAtividadeAvaliativaDisciplina ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativaDisciplina));
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
            this.consultasDisciplina = consultasDisciplina ?? throw new ArgumentNullException(nameof(consultasDisciplina));
            this.consultasFrequencia = consultasFrequencia ?? throw new ArgumentNullException(nameof(consultasFrequencia));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioConceito = repositorioConceito ?? throw new ArgumentNullException(nameof(repositorioConceito));
            this.repositorioWfAprovacaoNotaFechamento = repositorioWfAprovacaoNotaFechamento ?? throw new ArgumentNullException(nameof(repositorioWfAprovacaoNotaFechamento));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
            this.servicoPendenciaFechamento = servicoPendenciaFechamento ?? throw new ArgumentNullException(nameof(servicoPendenciaFechamento));
            this.comandosWorkflowAprovacao = comandosWorkflowAprovacao ?? throw new ArgumentNullException(nameof(comandosWorkflowAprovacao));
        }

        public void GerarNotificacaoAlteracaoLimiteDias(Turma turma, Usuario usuarioLogado, Ue ue, int bimestre, string alunosComNotaAlterada)
        {
            var dataAtual = DateTime.Now;
            var mensagem = $"<p>A(s) nota(s)/conceito(s) final(is) da turma {turma.Nome} da {ue.Nome} (DRE {ue.Dre.Nome}) no bimestre {bimestre} de {turma.AnoLetivo} foram alterados pelo Professor " +
                $"{usuarioLogado.Nome} ({usuarioLogado.CodigoRf}) em  {dataAtual.ToString("dd/MM/yyyy")} às {dataAtual.ToString("HH:mm")} para o(s) seguinte(s) aluno(s):</p><br/>{alunosComNotaAlterada} ";
            var listaCPs = servicoEOL.ObterFuncionariosPorCargoUe(turma.Ue.CodigoUe, (long)Cargo.CP);
            var listaDiretores = servicoEOL.ObterFuncionariosPorCargoUe(turma.Ue.CodigoUe, (long)Cargo.Diretor);
            var listaSupervisores = servicoEOL.ObterFuncionariosPorCargoUe(turma.Ue.CodigoUe, (long)Cargo.Supervisor);
            var usuariosNotificacao = new List<UsuarioEolRetornoDto>();

            if (listaCPs != null)
                usuariosNotificacao.AddRange(listaCPs);
            if (listaDiretores != null)
                usuariosNotificacao.AddRange(listaDiretores);
            if (listaSupervisores != null)
                usuariosNotificacao.AddRange(listaSupervisores);

            foreach (var usuarioNotificacaoo in usuariosNotificacao)
            {
                var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(usuarioNotificacaoo.CodigoRf);
                var notificacao = new Notificacao()
                {
                    Ano = turma.AnoLetivo,
                    Categoria = NotificacaoCategoria.Alerta,
                    DreId = ue.Dre.Id.ToString(),
                    Mensagem = mensagem,
                    UsuarioId = usuario.Id,
                    Tipo = NotificacaoTipo.Notas,
                    Titulo = $"Alteração em nota/conceito final - Turma {turma.Nome}",
                    TurmaId = turma.Id.ToString(),
                    UeId = turma.UeId.ToString(),
                };
                servicoNotificacao.Salvar(notificacao);
            }
        }

        public async Task GerarPendenciasFechamento(long disciplinaId, Turma turma, PeriodoEscolar periodoEscolar, FechamentoTurmaDisciplina fechamento, Usuario usuarioLogado, bool componenteSemNota = false)
        {
            var situacaoFechamento = SituacaoFechamento.ProcessadoComSucesso;

            if (!componenteSemNota)
            {
                servicoPendenciaFechamento.ValidarAvaliacoesSemNotasParaNenhumAluno(fechamento.Id, turma.CodigoTurma, disciplinaId, periodoEscolar.PeriodoInicio, periodoEscolar.PeriodoFim);
                servicoPendenciaFechamento.ValidarPercentualAlunosAbaixoDaMedia(fechamento);
                servicoPendenciaFechamento.ValidarAlteracaoExtemporanea(fechamento.Id, turma.CodigoTurma, disciplinaId);
            }
            servicoPendenciaFechamento.ValidarAulasReposicaoPendente(fechamento.Id, turma, disciplinaId, periodoEscolar.PeriodoInicio, periodoEscolar.PeriodoFim);
            servicoPendenciaFechamento.ValidarAulasSemPlanoAulaNaDataDoFechamento(fechamento.Id, turma, disciplinaId, periodoEscolar.PeriodoInicio, periodoEscolar.PeriodoFim);
            servicoPendenciaFechamento.ValidarAulasSemFrequenciaRegistrada(fechamento.Id, turma, disciplinaId, periodoEscolar.PeriodoInicio, periodoEscolar.PeriodoFim);

            var quantidadePendencias = servicoPendenciaFechamento.ObterQuantidadePendenciasGeradas();
            if (quantidadePendencias > 0)
            {
                situacaoFechamento = SituacaoFechamento.ProcessadoComPendencias;
                GerarNotificacaoFechamento(fechamento, turma, quantidadePendencias, usuarioLogado, periodoEscolar);
            }

            fechamento.AtualizarSituacao(situacaoFechamento);
            await repositorioFechamentoTurmaDisciplina.SalvarAsync(fechamento);
        }

        public async Task Reprocessar(long fechamentoTurmaDisciplinaId)
        {
            var fechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina.ObterPorId(fechamentoTurmaDisciplinaId);
            if (fechamentoTurmaDisciplina == null)
                throw new NegocioException("Fechamento ainda não realizado para essa turma.");

            var turma = repositorioTurma.ObterTurmaComUeEDrePorId(fechamentoTurmaDisciplina.FechamentoTurma.TurmaId);
            if (turma == null)
                throw new NegocioException("Turma não encontrada.");

            var disciplinaEOL = servicoEOL.ObterDisciplinasPorIds(new long[] { fechamentoTurmaDisciplina.DisciplinaId }).FirstOrDefault();
            if (disciplinaEOL == null)
                throw new NegocioException("Componente Curricular não localizado.");

            var periodoEscolar = repositorioPeriodoEscolar.ObterPorId(fechamentoTurmaDisciplina.FechamentoTurma.PeriodoEscolarId.Value);
            if (periodoEscolar == null)
                throw new NegocioException("Período escolar não encontrado.");

            fechamentoTurmaDisciplina.AdicionarPeriodoEscolar(periodoEscolar);
            fechamentoTurmaDisciplina.AtualizarSituacao(SituacaoFechamento.EmProcessamento);
            repositorioFechamentoTurmaDisciplina.Salvar(fechamentoTurmaDisciplina);

            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();
            Cliente.Executar<IServicoFechamentoTurmaDisciplina>(c => c.GerarPendenciasFechamento(fechamentoTurmaDisciplina.DisciplinaId, turma, periodoEscolar, fechamentoTurmaDisciplina, usuarioLogado, !disciplinaEOL.LancaNota));
        }

        public async Task<AuditoriaPersistenciaDto> Salvar(long id, FechamentoTurmaDisciplinaDto entidadeDto, bool componenteSemNota = false)
        {
            notasEnvioWfAprovacao = new List<FechamentoNotaDto>();

            var fechamentoTurmaDisciplina = MapearParaEntidade(id, entidadeDto);
            CarregarTurma(entidadeDto.TurmaId);

            // Valida periodo de fechamento
            var tipoCalendario = repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(turmaFechamento.AnoLetivo
                                                                , turmaFechamento.ModalidadeCodigo == Modalidade.EJA ? ModalidadeTipoCalendario.EJA : ModalidadeTipoCalendario.FundamentalMedio
                                                                , DateTime.Now.Semestre());

            var ue = turmaFechamento.Ue;
            var periodoFechamento = await servicoPeriodoFechamento.ObterPorTipoCalendarioDreEUe(tipoCalendario.Id, ue.Dre, ue);
            var periodoFechamentoBimestre = periodoFechamento?.FechamentosBimestres.FirstOrDefault(x => x.Bimestre == entidadeDto.Bimestre);

            if (periodoFechamento == null || periodoFechamentoBimestre == null)
                throw new NegocioException($"Não localizado período de fechamento em aberto para turma informada no {entidadeDto.Bimestre}º Bimestre");

            await CarregaFechamentoTurma(fechamentoTurmaDisciplina, turmaFechamento, periodoFechamentoBimestre.PeriodoEscolar);

            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();
            // Valida Permissão do Professor na Turma/Disciplina
            VerificaSeProfessorPodePersistirTurma(usuarioLogado.CodigoRf, entidadeDto.TurmaId, periodoFechamentoBimestre.PeriodoEscolar.PeriodoFim);

            var fechamentoAlunos = Enumerable.Empty<FechamentoAluno>();
            // reprocessar do fechamento de componente sem nota deve atualizar a sintise de frequencia
            if (componenteSemNota && id > 0)
            {
                var disciplinaEOL = await consultasDisciplina.ObterDisciplina(fechamentoTurmaDisciplina.DisciplinaId);
                fechamentoAlunos = await AtualizaSinteseAlunos(id, periodoFechamentoBimestre.PeriodoEscolar.PeriodoFim, disciplinaEOL);
            }
            else
                // Carrega notas alunos
                fechamentoAlunos = await CarregarFechamentoAlunoENota(id, entidadeDto.NotaConceitoAlunos);

            var alunos = await servicoEOL.ObterAlunosPorTurma(turmaFechamento.CodigoTurma);
            var parametroDiasAlteracao = repositorioParametrosSistema.ObterValorPorTipoEAno(TipoParametroSistema.QuantidadeDiasAlteracaoNotaFinal, turmaFechamento.AnoLetivo);
            var diasAlteracao = DateTime.Today.DayOfYear - fechamentoTurmaDisciplina.CriadoEm.Date.DayOfYear;
            var acimaDiasPermitidosAlteracao = parametroDiasAlteracao != null && diasAlteracao > int.Parse(parametroDiasAlteracao);
            var alunosComNotaAlterada = "";

            unitOfWork.IniciarTransacao();
            try
            {
                var fechamentoTurmaId = await repositorioFechamentoTurma.SalvarAsync(fechamentoTurmaDisciplina.FechamentoTurma);
                fechamentoTurmaDisciplina.FechamentoTurmaId = fechamentoTurmaId;

                await repositorioFechamentoTurmaDisciplina.SalvarAsync(fechamentoTurmaDisciplina);
                foreach (var fechamentoAluno in fechamentoAlunos)
                {
                    fechamentoAluno.FechamentoTurmaDisciplinaId = fechamentoTurmaDisciplina.Id;
                    await repositorioFechamentoAluno.SalvarAsync(fechamentoAluno);
                    foreach (var fechamentoNota in fechamentoAluno.FechamentoNotas)
                    {
                        fechamentoNota.FechamentoAlunoId = fechamentoAluno.Id;
                        await repositorioFechamentoNota.SalvarAsync(fechamentoNota);
                    }

                    if (!componenteSemNota)
                    {
                        var notaAlunoAlterada = entidadeDto.NotaConceitoAlunos.FirstOrDefault(n => n.CodigoAluno.Equals(fechamentoAluno.AlunoCodigo));
                        if (id > 0 && acimaDiasPermitidosAlteracao && notaAlunoAlterada != null && !alunosComNotaAlterada.Contains(fechamentoAluno.AlunoCodigo))
                        {
                            var aluno = alunos.FirstOrDefault(a => a.CodigoAluno == fechamentoAluno.AlunoCodigo);
                            alunosComNotaAlterada += $"<li>{aluno.CodigoAluno} - {aluno.NomeAluno}</li>";
                        }
                    }
                }

                await EnviarNotasWfAprovacao(fechamentoTurmaDisciplina.Id, fechamentoTurmaDisciplina.FechamentoTurma.PeriodoEscolar, usuarioLogado);

                unitOfWork.PersistirTransacao();

                if (alunosComNotaAlterada.Length > 0)
                    Cliente.Executar<IServicoFechamentoTurmaDisciplina>(s => s.GerarNotificacaoAlteracaoLimiteDias(turmaFechamento, usuarioLogado, ue, entidadeDto.Bimestre, alunosComNotaAlterada));

                Cliente.Executar<IServicoFechamentoTurmaDisciplina>(c => c.GerarPendenciasFechamento(fechamentoTurmaDisciplina.DisciplinaId, turmaFechamento, periodoFechamentoBimestre.PeriodoEscolar, fechamentoTurmaDisciplina, usuarioLogado, componenteSemNota));

                return (AuditoriaPersistenciaDto)fechamentoTurmaDisciplina;
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw e;
            }
        }

        public void VerificaPendenciasFechamento(long fechamentoId)
        {
            // Verifica existencia de pendencia em aberto
            if (!servicoPendenciaFechamento.VerificaPendenciasFechamento(fechamentoId))
            {
                var fechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina.ObterPorId(fechamentoId);
                // Atualiza situação do fechamento
                fechamentoTurmaDisciplina.Situacao = SituacaoFechamento.ProcessadoComSucesso;
                repositorioFechamentoTurmaDisciplina.Salvar(fechamentoTurmaDisciplina);
            }
        }

        private async Task<IEnumerable<FechamentoAluno>> AtualizaSinteseAlunos(long fechamentoTurmaDisciplinaId, DateTime dataReferencia, DisciplinaDto disciplina)
        {
            var fechamentoAlunos = await repositorioFechamentoAluno.ObterPorFechamentoTurmaDisciplina(fechamentoTurmaDisciplinaId);
            foreach (var fechamentoAluno in fechamentoAlunos)
            {
                foreach (var fechamentoNota in fechamentoAluno.FechamentoNotas)
                {
                    var frequencia = consultasFrequencia.ObterPorAlunoDisciplinaData(fechamentoAluno.AlunoCodigo, fechamentoNota.DisciplinaId.ToString(), dataReferencia);
                    var sinteseDto = consultasFrequencia.ObterSinteseAluno(frequencia.PercentualFrequencia, disciplina);

                    fechamentoNota.SinteseId = (long)sinteseDto.SinteseId;
                }
            }

            return fechamentoAlunos;
        }

        private async Task CarregaFechamentoTurma(FechamentoTurmaDisciplina fechamentoTurmaDisciplina, Turma turma, PeriodoEscolar periodoEscolar)
        {
            if (fechamentoTurmaDisciplina.Id > 0)
            {
                // Alterando registro de fechamento
                fechamentoTurmaDisciplina.FechamentoTurma.Turma = turma;
                fechamentoTurmaDisciplina.FechamentoTurma.TurmaId = turma.Id;
                fechamentoTurmaDisciplina.FechamentoTurma.PeriodoEscolar = periodoEscolar;
                fechamentoTurmaDisciplina.FechamentoTurma.PeriodoEscolarId = periodoEscolar.Id;
            }
            else
            {
                // Incluindo registro de fechamento turma disciplina

                // Busca registro existente de fechamento da turma
                var fechamentoTurma = await repositorioFechamentoTurma.ObterPorTurmaPeriodo(turma.Id, periodoEscolar.Id);
                if (fechamentoTurma == null)
                    fechamentoTurma = new FechamentoTurma(turma, periodoEscolar);

                fechamentoTurmaDisciplina.FechamentoTurma = fechamentoTurma;
            }
        }

        private async Task<IEnumerable<FechamentoAluno>> CarregarFechamentoAlunoENota(long fechamentoTurmaDisciplinaId, IEnumerable<FechamentoNotaDto> fechamentoNotasDto)
        {
            var fechamentoAlunos = new List<FechamentoAluno>();

            if (fechamentoTurmaDisciplinaId > 0)
                fechamentoAlunos = (await repositorioFechamentoAluno.ObterPorFechamentoTurmaDisciplina(fechamentoTurmaDisciplinaId)).ToList();

            // Edita as notas existentes
            foreach (var agrupamentoNotasAluno in fechamentoNotasDto.GroupBy(g => g.CodigoAluno))
            {
                // Busca fechamento do aluno
                var fechamentoAluno = fechamentoAlunos.FirstOrDefault(c => c.AlunoCodigo == agrupamentoNotasAluno.Key);
                if (fechamentoAluno == null)
                    fechamentoAluno = new FechamentoAluno() { AlunoCodigo = agrupamentoNotasAluno.Key, FechamentoTurmaDisciplinaId = fechamentoTurmaDisciplinaId };

                foreach (var fechamentoNotaDto in agrupamentoNotasAluno)
                {
                    // busca nota do aluno
                    var notaFechamento = fechamentoAluno.FechamentoNotas.FirstOrDefault(x => x.DisciplinaId == fechamentoNotaDto.DisciplinaId);
                    if (notaFechamento != null)
                    {
                        if (!EnviarWfAprovacao())
                        {
                            notaFechamento.Nota = fechamentoNotaDto.Nota;
                            notaFechamento.ConceitoId = fechamentoNotaDto.ConceitoId;
                            notaFechamento.SinteseId = fechamentoNotaDto.SinteseId;
                        }
                        else
                        {
                            fechamentoNotaDto.Id = notaFechamento.Id;
                            notasEnvioWfAprovacao.Add(fechamentoNotaDto);
                        }
                    }
                    else
                        fechamentoAluno.AdicionarNota(MapearParaEntidade(fechamentoNotaDto));
                }
                fechamentoAlunos.Add(fechamentoAluno);
            }

            return fechamentoAlunos;
        }

        private void CarregarTurma(string turmaCodigo)
        {
            turmaFechamento = repositorioTurma.ObterTurmaComUeEDrePorCodigo(turmaCodigo);
            if (turmaFechamento == null)
                throw new NegocioException($"Turma com código [{turmaCodigo}] não localizada!");
        }

        private async Task EnviarNotasWfAprovacao(long fechamentoTurmaDisciplinaId, PeriodoEscolar periodoEscolar, Usuario usuarioLogado)
        {
            if (notasEnvioWfAprovacao.Any())
            {
                var lancaNota = !notasEnvioWfAprovacao.First().ConceitoId.HasValue;
                var notaConceitoMensagem = lancaNota ? "nota" : "conceito";

                var mensagem = await MontaMensagemWfAprovacao(notaConceitoMensagem, periodoEscolar, usuarioLogado);

                var wfAprovacaoNota = new WorkflowAprovacaoDto()
                {
                    Ano = DateTime.Today.Year,
                    NotificacaoCategoria = NotificacaoCategoria.Workflow_Aprovacao,
                    EntidadeParaAprovarId = fechamentoTurmaDisciplinaId,
                    Tipo = WorkflowAprovacaoTipo.AlteracaoNotaFechamento,
                    TurmaId = turmaFechamento.CodigoTurma,
                    UeId = turmaFechamento.Ue.CodigoUe,
                    DreId = turmaFechamento.Ue.Dre.CodigoDre,
                    NotificacaoTitulo = $"Alteração em {notaConceitoMensagem} final - Turma {turmaFechamento.Nome} ({turmaFechamento.AnoLetivo})",
                    NotificacaoTipo = NotificacaoTipo.Notas,
                    NotificacaoMensagem = mensagem
                };

                wfAprovacaoNota.AdicionarNivel(Cargo.CP);
                wfAprovacaoNota.AdicionarNivel(Cargo.Diretor);
                wfAprovacaoNota.AdicionarNivel(Cargo.Supervisor);

                var idWorkflow = comandosWorkflowAprovacao.Salvar(wfAprovacaoNota);
                foreach (var notaFechamento in notasEnvioWfAprovacao)
                {
                    await repositorioWfAprovacaoNotaFechamento.SalvarAsync(new WfAprovacaoNotaFechamento()
                    {
                        WfAprovacaoId = idWorkflow,
                        FechamentoNotaId = notaFechamento.Id,
                        Nota = notaFechamento.Nota,
                        ConceitoId = notaFechamento.ConceitoId
                    });
                }
            }
        }

        private bool EnviarWfAprovacao()
            => turmaFechamento.AnoLetivo != DateTime.Today.Year;

        private void GerarNotificacaoFechamento(FechamentoTurmaDisciplina fechamentoTurmaDisciplina, Turma turma, int quantidadePendencias, Usuario usuarioLogado, PeriodoEscolar periodoEscolar)
        {
            var componentes = servicoEOL.ObterDisciplinasPorIds(new long[] { fechamentoTurmaDisciplina.DisciplinaId });
            if (componentes == null || !componentes.Any())
            {
                throw new NegocioException("Componente curricular não encontrado.");
            }
            var ue = turma.Ue;
            if (ue == null)
                throw new NegocioException("UE não encontrada.");

            var dre = ue.Dre;
            if (dre == null)
                throw new NegocioException("DRE não encontrada.");

            var urlFrontEnd = configuration["UrlFrontEnd"];
            if (string.IsNullOrWhiteSpace(urlFrontEnd))
                throw new NegocioException("Url do frontend não encontrada.");

            var notificacao = new Notificacao()
            {
                UsuarioId = usuarioLogado.Id,
                Ano = DateTime.Now.Year,
                Categoria = NotificacaoCategoria.Aviso,
                Titulo = $"Pendência no fechamento da turma {turma.Nome}",
                Tipo = NotificacaoTipo.Fechamento,
                Mensagem = $"O fechamento do {periodoEscolar.Bimestre}º bimestre de {componentes.FirstOrDefault().Nome} da turma {turma.Nome} da {ue.Nome} ({dre.Nome}) gerou {quantidadePendencias} pendência(s). " +
                $"Clique <a href='{urlFrontEnd}fechamento/pendencias-fechamento/{periodoEscolar.Bimestre}/{fechamentoTurmaDisciplina.DisciplinaId}'>aqui</a> para mais detalhes."
            };
            servicoNotificacao.Salvar(notificacao);

            var diretores = servicoEOL.ObterFuncionariosPorCargoUe(ue.CodigoUe, (long)Cargo.Diretor);

            if (diretores != null)
            {
                foreach (var diretor in diretores)
                {
                    var notificacaoDiretor = notificacao;
                    notificacaoDiretor.Id = 0;
                    var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(diretor.CodigoRf);
                    notificacaoDiretor.UsuarioId = usuario.Id;
                    servicoNotificacao.Salvar(notificacaoDiretor);
                }
            }

            var cps = servicoEOL.ObterFuncionariosPorCargoUe(ue.CodigoUe, (long)Cargo.CP);

            if (cps != null)
            {
                foreach (var cp in cps)
                {
                    var notificacaoCp = notificacao;
                    notificacaoCp.Id = 0;
                    var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(cp.CodigoRf);
                    notificacaoCp.UsuarioId = usuario.Id;
                    servicoNotificacao.Salvar(notificacaoCp);
                }
            }
        }

        private FechamentoNota MapearParaEntidade(FechamentoNotaDto fechamentoNotaDto)
            => fechamentoNotaDto == null ? null :
              new FechamentoNota()
              {
                  DisciplinaId = fechamentoNotaDto.DisciplinaId,
                  Nota = fechamentoNotaDto.Nota,
                  ConceitoId = fechamentoNotaDto.ConceitoId,
                  SinteseId = fechamentoNotaDto.SinteseId
              };

        private FechamentoTurmaDisciplina MapearParaEntidade(long id, FechamentoTurmaDisciplinaDto fechamentoDto)
        {
            var fechamento = new FechamentoTurmaDisciplina();
            if (id > 0)
                fechamento = repositorioFechamentoTurmaDisciplina.ObterPorId(id);

            fechamento.AtualizarSituacao(SituacaoFechamento.EmProcessamento);
            fechamento.DisciplinaId = fechamentoDto.DisciplinaId;
            fechamento.Justificativa = fechamentoDto.Justificativa;

            return fechamento;
        }

        private async Task<string> MontaMensagemWfAprovacao(string notaConceitoMensagem, PeriodoEscolar periodoEscolar, Usuario usuarioLogado)
        {
            var mensagem = new StringBuilder();
            mensagem.Append($"<p>A(s) {notaConceitoMensagem}(s) final(is) da turma {turmaFechamento.Nome} da ");
            mensagem.Append($"{turmaFechamento.Ue.TipoEscola.ShortName()} {turmaFechamento.Ue.Nome} (DRE {turmaFechamento.Ue.Dre.Nome}) ");
            mensagem.Append($"no bimestre {periodoEscolar.Bimestre} de {turmaFechamento.AnoLetivo} foram alterados pelo Professor {usuarioLogado.Nome}");
            mensagem.Append($"({usuarioLogado.CriadoEm}) em {DateTime.Now.ToString("dd/MM/yyyy")} às {DateTime.Now.ToString("HH:mm")} para o(s) seguinte(s) aluno(s):</p>");

            mensagem.AppendLine("<table style='margin-left: auto; margin-right: auto;' border='2' cellpadding='5'>");
            mensagem.AppendLine("<tr>");
            mensagem.AppendLine("<td style='padding: 5px;'>Código Aluno</td>");
            mensagem.AppendLine("<td style='padding: 5px;'>Nome do aluno</td>");
            mensagem.AppendLine("</tr>");

            var alunosTurma = await servicoEOL.ObterAlunosPorTurma(turmaFechamento.CodigoTurma);
            foreach (var notaAprovacao in notasEnvioWfAprovacao)
            {
                var aluno = alunosTurma.FirstOrDefault(c => c.CodigoAluno == notaAprovacao.CodigoAluno);

                mensagem.AppendLine("<tr>");
                mensagem.Append($"<td style='padding: 5px;'>{notaAprovacao.CodigoAluno}</td>");
                mensagem.Append($"<td style='padding: 5px;'>{aluno?.NomeAluno}</td>");
                mensagem.AppendLine("</tr>");
            }
            mensagem.AppendLine("</table>");
            mensagem.AppendLine("<p>Você precisa aceitar esta notificação para que a alteração seja considerada válida.</p>");

            return mensagem.ToString();
        }

        private async Task VerificaSeProfessorPodePersistirTurma(string codigoRf, string turmaId, DateTime data)
        {
            var usuario = await servicoUsuario.ObterUsuarioLogado();

            if (!usuario.EhProfessorCj() && !await servicoUsuario.PodePersistirTurma(codigoRf, turmaId, data))
                throw new NegocioException("Você não pode fazer alterações ou inclusões nesta turma e data.");
        }
    }
}