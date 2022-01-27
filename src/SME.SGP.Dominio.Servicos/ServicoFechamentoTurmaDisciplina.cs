using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SME.SGP.Aplicacao.GerarNotificacaoAlteracaoLimiteDiasUseCase;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoFechamentoTurmaDisciplina : IServicoFechamentoTurmaDisciplina
    {
        private readonly IConsultasDisciplina consultasDisciplina;
        private readonly IConsultasFrequencia consultasFrequencia;
        private readonly IConsultasSupervisor consultasSupervisor;
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IRepositorioEventoTipo repositorioEventoTipo;
        private readonly IRepositorioFechamentoAlunoConsulta repositorioFechamentoAlunoConsulta;
        private readonly IRepositorioFechamentoAluno repositorioFechamentoAluno;
        private readonly IRepositorioFechamentoNota repositorioFechamentoNota;
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;
        private readonly IRepositorioFechamentoTurma repositorioFechamentoTurma;
        private readonly IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurmaConsulta;
        private readonly IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina;
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;
        private readonly IRepositorioTurmaConsulta repositorioTurma;
        private readonly IRepositorioParametrosSistemaConsulta repositorioParametrosSistema;
        private readonly IRepositorioTipoCalendarioConsulta repositorioTipoCalendario;
        private readonly IServicoEol servicoEOL;
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IServicoPeriodoFechamento servicoPeriodoFechamento;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular;
        private readonly IUnitOfWork unitOfWork;
        private List<FechamentoNotaDto> notasEnvioWfAprovacao;
        private Turma turmaFechamento;
        private readonly IMediator mediator;

        public ServicoFechamentoTurmaDisciplina(IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina,
                                                IRepositorioFechamentoTurma repositorioFechamentoTurma,
                                                IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurmaConsulta,
                                                IRepositorioFechamentoAlunoConsulta repositorioFechamentoAlunoConsulta,
                                                IRepositorioFechamentoAluno repositorioFechamentoAluno,
                                                IRepositorioFechamentoNota repositorioFechamentoNota,
                                                IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular,
                                                IRepositorioTurmaConsulta repositorioTurma,
                                                IServicoPeriodoFechamento servicoPeriodoFechamento,
                                                IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar,
                                                IRepositorioTipoCalendarioConsulta repositorioTipoCalendario,
                                                IRepositorioParametrosSistemaConsulta repositorioParametrosSistema,
                                                IConsultasDisciplina consultasDisciplina,
                                                IConsultasFrequencia consultasFrequencia,
                                                IServicoNotificacao servicoNotificacao,
                                                IServicoEol servicoEOL,
                                                IServicoUsuario servicoUsuario,
                                                IUnitOfWork unitOfWork,
                                                IConsultasSupervisor consultasSupervisor,
                                                IRepositorioEvento repositorioEvento,
                                                IRepositorioEventoTipo repositorioEventoTipo,
                                                IRepositorioFechamentoReabertura repositorioFechamentoReabertura,
                                                IMediator mediator)
        {
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
            this.repositorioFechamentoTurmaConsulta = repositorioFechamentoTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaConsulta));
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
            this.repositorioFechamentoAluno = repositorioFechamentoAluno ?? throw new ArgumentNullException(nameof(repositorioFechamentoAluno));
            this.repositorioFechamentoAlunoConsulta = repositorioFechamentoAlunoConsulta ?? throw new ArgumentNullException(nameof(repositorioFechamentoAlunoConsulta));
            this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new ArgumentNullException(nameof(repositorioFechamentoNota));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.servicoPeriodoFechamento = servicoPeriodoFechamento ?? throw new ArgumentNullException(nameof(servicoPeriodoFechamento));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
            this.consultasDisciplina = consultasDisciplina ?? throw new ArgumentNullException(nameof(consultasDisciplina));
            this.consultasFrequencia = consultasFrequencia ?? throw new ArgumentNullException(nameof(consultasFrequencia));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.consultasSupervisor = consultasSupervisor ?? throw new ArgumentNullException(nameof(consultasSupervisor));
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
            this.repositorioEventoTipo = repositorioEventoTipo ?? throw new ArgumentNullException(nameof(repositorioEventoTipo));
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new ArgumentNullException(nameof(repositorioFechamentoReabertura));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        }

        public void GerarNotificacaoAlteracaoLimiteDias(Turma turma, Usuario usuarioLogado, Ue ue, int bimestre, string alunosComNotaAlterada)
        {
            var dataAtual = DateTime.Now;
            var mensagem = $"<p>A(s) nota(s)/conceito(s) final(is) da turma {turma.Nome} da {ue.Nome} (DRE {ue.Dre.Nome}) no bimestre {bimestre} de {turma.AnoLetivo} foram alterados pelo Professor " +
                $"{usuarioLogado.Nome} ({usuarioLogado.CodigoRf}) em  {dataAtual.ToString("dd/MM/yyyy")} às {dataAtual.ToString("HH:mm")} para o(s) seguinte(s) aluno(s):</p><br/>{alunosComNotaAlterada} ";
            var listaCPs = servicoEOL.ObterFuncionariosPorCargoUe(turma.Ue.CodigoUe, (long)Cargo.CP);
            var listaDiretores = servicoEOL.ObterFuncionariosPorCargoUe(turma.Ue.CodigoUe, (long)Cargo.Diretor);

            var listaSupervisores = consultasSupervisor.ObterPorUe(turma.Ue.CodigoUe);

            var usuariosNotificacao = new List<UsuarioEolRetornoDto>();

            if (listaCPs != null)
                usuariosNotificacao.AddRange(listaCPs);
            if (listaDiretores != null)
                usuariosNotificacao.AddRange(listaDiretores);
            if (listaSupervisores != null)
                usuariosNotificacao.Add(new UsuarioEolRetornoDto() { CodigoRf = listaSupervisores.SupervisorId, NomeServidor = listaSupervisores.SupervisorNome });

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

        private async Task GerarPendenciasFechamento(long componenteCurricularId, string turmaCodigo, string turmaNome, DateTime periodoEscolarInicio, DateTime periodoEscolarFim, int bimestre, Usuario usuario, long fechamentoTurmaDisciplinaId, string justificativa, string criadoRF, long turmaId, bool componenteSemNota = false, bool registraFrequencia = true)
        {
            await mediator.Send(new IncluirFilaGeracaoPendenciasFechamentoCommand(
                componenteCurricularId,
                turmaCodigo,
                turmaNome,
                periodoEscolarInicio,
                periodoEscolarFim,
                bimestre,
                usuario,
                fechamentoTurmaDisciplinaId,
                justificativa,
                criadoRF,
                turmaId,
                componenteSemNota,
                registraFrequencia));
        }

        public async Task Reprocessar(long fechamentoTurmaDisciplinaId, Usuario usuario = null)
        {
            var fechamentoTurmaDisciplina = await mediator.Send(new ObterFechamentoTurmaDisciplinaPorIdQuery(fechamentoTurmaDisciplinaId));
                
            if (fechamentoTurmaDisciplina == null)
                throw new NegocioException("Fechamento ainda não realizado para essa turma.");

            var turma = await repositorioTurma.ObterTurmaComUeEDrePorId(fechamentoTurmaDisciplina.FechamentoTurma.TurmaId);
            if (turma == null)
                throw new NegocioException("Turma não encontrada.");

            var disciplinaEOL = (await repositorioComponenteCurricular.ObterDisciplinasPorIds(new long[] { fechamentoTurmaDisciplina.DisciplinaId })).ToList().FirstOrDefault();
            if (disciplinaEOL == null)
                throw new NegocioException("Componente Curricular não localizado.");

            var periodoEscolar = repositorioPeriodoEscolar.ObterPorId(fechamentoTurmaDisciplina.FechamentoTurma.PeriodoEscolarId.Value);
            if (periodoEscolar == null)
                throw new NegocioException("Período escolar não encontrado.");

            fechamentoTurmaDisciplina.AdicionarPeriodoEscolar(periodoEscolar);
            fechamentoTurmaDisciplina.AtualizarSituacao(SituacaoFechamento.EmProcessamento);
            repositorioFechamentoTurmaDisciplina.Salvar(fechamentoTurmaDisciplina);

            var usuarioLogado = usuario ?? await servicoUsuario.ObterUsuarioLogado();
            await GerarPendenciasFechamento(fechamentoTurmaDisciplina.DisciplinaId,
                                            turma.CodigoTurma,
                                            turma.Nome,
                                            periodoEscolar.PeriodoInicio,
                                            periodoEscolar.PeriodoFim,
                                            periodoEscolar.Bimestre,
                                            usuarioLogado,
                                            fechamentoTurmaDisciplina.Id,
                                            fechamentoTurmaDisciplina.Justificativa,
                                            fechamentoTurmaDisciplina.CriadoRF,
                                            turma.Id,
                                            !disciplinaEOL.LancaNota,
                                            disciplinaEOL.RegistraFrequencia);
        }

        public async Task<AuditoriaPersistenciaDto> Salvar(long id, FechamentoTurmaDisciplinaDto entidadeDto, bool componenteSemNota = false)
        {
            notasEnvioWfAprovacao = new List<FechamentoNotaDto>();

            var fechamentoTurmaDisciplina = MapearParaEntidade(id, entidadeDto);

            await CarregarTurma(entidadeDto.TurmaId);

            // Valida periodo de fechamento
            var tipoCalendario = await repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(turmaFechamento.AnoLetivo
                                                                , turmaFechamento.ModalidadeCodigo == Modalidade.EJA ? ModalidadeTipoCalendario.EJA : ModalidadeTipoCalendario.FundamentalMedio
                                                                , turmaFechamento.Semestre);

            var ue = turmaFechamento.Ue;

            var periodos = await ObterPeriodoEscolarFechamentoReabertura(tipoCalendario.Id, ue, entidadeDto.Bimestre);
            PeriodoEscolar periodoEscolar = periodos.periodoEscolar;
            if (periodoEscolar == null)
                throw new NegocioException($"Não localizado período de fechamento em aberto para turma informada no {entidadeDto.Bimestre}º Bimestre");

            await CarregaFechamentoTurma(fechamentoTurmaDisciplina, turmaFechamento, periodoEscolar);

            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();

            var parametroAlteracaoNotaFechamento = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.AprovacaoAlteracaoNotaFechamento, turmaFechamento.AnoLetivo));

            // Valida Permissão do Professor na Turma/Disciplina            
            if (!turmaFechamento.EhTurmaEdFisicaOuItinerario() && !usuarioLogado.EhGestorEscolar() && !usuarioLogado.EhPerfilSME() && !usuarioLogado.EhPerfilDRE())
                await VerificaSeProfessorPodePersistirTurma(usuarioLogado.CodigoRf, entidadeDto.TurmaId, periodoEscolar.PeriodoFim, periodos.periodoFechamento, entidadeDto.DisciplinaId.ToString(), usuarioLogado);
            
            var fechamentoAlunos = Enumerable.Empty<FechamentoAluno>();

            DisciplinaDto disciplinaEOL = await consultasDisciplina.ObterDisciplina(fechamentoTurmaDisciplina.DisciplinaId);
            if (disciplinaEOL == null)
                throw new NegocioException("Não foi possível localizar o componente curricular no EOL.");

            // reprocessar do fechamento de componente sem nota deve atualizar a sintise de frequencia
            if (componenteSemNota && id > 0)
                fechamentoAlunos = await AtualizaSinteseAlunos(id, periodoEscolar.PeriodoFim, disciplinaEOL, turmaFechamento.AnoLetivo);
            else
                fechamentoAlunos = await CarregarFechamentoAlunoENota(id, entidadeDto.NotaConceitoAlunos, usuarioLogado, parametroAlteracaoNotaFechamento);

            var alunos = await servicoEOL.ObterAlunosPorTurma(turmaFechamento.CodigoTurma);
            var parametroDiasAlteracao = await repositorioParametrosSistema.ObterValorPorTipoEAno(TipoParametroSistema.QuantidadeDiasAlteracaoNotaFinal, turmaFechamento.AnoLetivo);
            var diasAlteracao = DateTime.Today.DayOfYear - fechamentoTurmaDisciplina.CriadoEm.Date.DayOfYear;
            var acimaDiasPermitidosAlteracao = parametroDiasAlteracao != null && diasAlteracao > int.Parse(parametroDiasAlteracao);
            var alunosComNotaAlterada = "";

            unitOfWork.IniciarTransacao();
            try
            {
                var fechamentoTurmaId = fechamentoTurmaDisciplina.FechamentoTurma.Id > 0 ?
                    fechamentoTurmaDisciplina.FechamentoTurma.Id :
                    await repositorioFechamentoTurma.SalvarAsync(fechamentoTurmaDisciplina.FechamentoTurma);

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

                await EnviarNotasWfAprovacao(fechamentoTurmaDisciplina.Id, fechamentoTurmaDisciplina.FechamentoTurma.PeriodoEscolar, usuarioLogado, disciplinaEOL, turmaFechamento);

                unitOfWork.PersistirTransacao();

                if (alunosComNotaAlterada.Length > 0)
                {
                    var dados = new GerarNotificacaoAlteracaoLimiteDiasParametros
                    {
                        TurmaFechamento = turmaFechamento,
                        UsuarioLogado = usuarioLogado,
                        Ue = ue,
                        Bimestre = entidadeDto.Bimestre,
                        AlunosComNotaAlterada = alunosComNotaAlterada
                    };
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.GerarNotificacaoAlteracaoLimiteDias, dados, Guid.NewGuid(), null));
                }

                await GerarPendenciasFechamento(fechamentoTurmaDisciplina.DisciplinaId,
                                                turmaFechamento.CodigoTurma,
                                                turmaFechamento.Nome,
                                                periodoEscolar.PeriodoInicio,
                                                periodoEscolar.PeriodoFim,
                                                periodoEscolar.Bimestre,
                                                usuarioLogado,
                                                fechamentoTurmaDisciplina.Id,
                                                fechamentoTurmaDisciplina.Justificativa,
                                                fechamentoTurmaDisciplina.CriadoRF,
                                                fechamentoTurmaDisciplina.FechamentoTurma.TurmaId,
                                                componenteSemNota,
                                                disciplinaEOL.RegistraFrequencia);

                await mediator.Send(new PublicaFilaExcluirPendenciaAusenciaFechamentoCommand(fechamentoTurmaDisciplina.DisciplinaId, periodoEscolar.Id, turmaFechamento.Id, usuarioLogado));

                var auditoria = (AuditoriaPersistenciaDto)fechamentoTurmaDisciplina;
                auditoria.EmAprovacao = notasEnvioWfAprovacao.Any();
                return auditoria;
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw e;
            }
        }

        private async Task<IEnumerable<FechamentoAluno>> AtualizaSinteseAlunos(long fechamentoTurmaDisciplinaId, DateTime dataReferencia, DisciplinaDto disciplina, int anoLetivo)
        {
            var fechamentoAlunos = await repositorioFechamentoAlunoConsulta.ObterPorFechamentoTurmaDisciplina(fechamentoTurmaDisciplinaId);
            foreach (var fechamentoAluno in fechamentoAlunos)
            {
                foreach (var fechamentoNota in fechamentoAluno.FechamentoNotas)
                {
                    var frequencia = consultasFrequencia.ObterPorAlunoDisciplinaData(fechamentoAluno.AlunoCodigo, fechamentoNota.DisciplinaId.ToString(), dataReferencia);
                    var percentualFrequencia = frequencia == null ? 100 : frequencia.PercentualFrequencia;
                    var sinteseDto = await consultasFrequencia.ObterSinteseAluno(percentualFrequencia, disciplina, anoLetivo);

                    fechamentoNota.SinteseId = (long)sinteseDto.Id;
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
                var fechamentoTurma = await repositorioFechamentoTurmaConsulta.ObterPorTurmaPeriodo(turma.Id, periodoEscolar.Id);
                if (fechamentoTurma == null)
                    fechamentoTurma = new FechamentoTurma(turma, periodoEscolar);

                fechamentoTurmaDisciplina.FechamentoTurma = fechamentoTurma;
            }
        }

        private async Task<IEnumerable<FechamentoAluno>> CarregarFechamentoAlunoENota(long fechamentoTurmaDisciplinaId, IEnumerable<FechamentoNotaDto> fechamentoNotasDto, Usuario usuarioLogado, ParametrosSistema parametroAlteracaoNotaFechamento)
        {
            var fechamentoAlunos = new List<FechamentoAluno>();

            if (fechamentoTurmaDisciplinaId > 0)
                fechamentoAlunos = (await repositorioFechamentoAlunoConsulta.ObterPorFechamentoTurmaDisciplina(fechamentoTurmaDisciplinaId)).ToList();

            foreach (var agrupamentoNotasAluno in fechamentoNotasDto.GroupBy(g => g.CodigoAluno))
            {
                var fechamentoAluno = fechamentoAlunos.FirstOrDefault(c => c.AlunoCodigo == agrupamentoNotasAluno.Key);
                if (fechamentoAluno == null)
                    fechamentoAluno = new FechamentoAluno() { AlunoCodigo = agrupamentoNotasAluno.Key, FechamentoTurmaDisciplinaId = fechamentoTurmaDisciplinaId };

                foreach (var fechamentoNotaDto in agrupamentoNotasAluno)
                {
                    var notaFechamento = fechamentoAluno.FechamentoNotas.FirstOrDefault(x => x.DisciplinaId == fechamentoNotaDto.DisciplinaId);
                    if (notaFechamento != null)
                    {
                        if (!notaFechamento.ConceitoId.HasValue)
                        {
                            if (fechamentoNotaDto.Nota != notaFechamento.Nota)
                                await mediator.Send(new SalvarHistoricoNotaFechamentoCommand(notaFechamento.Nota != null ? notaFechamento.Nota.Value : (double?)null, fechamentoNotaDto.Nota != null ? fechamentoNotaDto.Nota.Value : (double?)null, notaFechamento.Id));
                        }
                        else
                        {
                            if (fechamentoNotaDto.ConceitoId != notaFechamento.ConceitoId)
                                await mediator.Send(new SalvarHistoricoConceitoFechamentoCommand(notaFechamento.ConceitoId != null ? notaFechamento.ConceitoId.Value : (long?)null, fechamentoNotaDto.ConceitoId != null ? fechamentoNotaDto.ConceitoId.Value : (long?)null, notaFechamento.Id));
                        }

                        if (EnviarWfAprovacao(usuarioLogado) && parametroAlteracaoNotaFechamento.Ativo)
                        {
                            fechamentoNotaDto.Id = notaFechamento.Id;
                            if (!notaFechamento.ConceitoId.HasValue)
                                fechamentoNotaDto.NotaAnterior = notaFechamento.Nota != null ? notaFechamento.Nota.Value : (double?)null;
                            else
                                fechamentoNotaDto.ConceitoIdAnterior = notaFechamento.ConceitoId != null ? notaFechamento.ConceitoId.Value : (long?)null;

                            notasEnvioWfAprovacao.Add(fechamentoNotaDto);
                        }
                        else
                        {
                            notaFechamento.Nota = fechamentoNotaDto.Nota;
                            notaFechamento.ConceitoId = fechamentoNotaDto.ConceitoId;
                            notaFechamento.SinteseId = fechamentoNotaDto.SinteseId;
                        }
                    }
                    else
                        fechamentoAluno.AdicionarNota(MapearParaEntidade(fechamentoNotaDto));
                }
                fechamentoAlunos.Add(fechamentoAluno);
            }

            return fechamentoAlunos;
        }

        private async Task CarregarTurma(string turmaCodigo)
        {
            turmaFechamento = await repositorioTurma.ObterTurmaComUeEDrePorCodigo(turmaCodigo);
            if (turmaFechamento == null)
                throw new NegocioException($"Turma com código [{turmaCodigo}] não localizada!");
        }

        private async Task EnviarNotasWfAprovacao(long fechamentoTurmaDisciplinaId, PeriodoEscolar periodoEscolar, Usuario usuarioLogado, DisciplinaDto componenteCurricular, Turma turma)
        {
            if (notasEnvioWfAprovacao.Any())
                await mediator.Send(new EnviarNotasFechamentoParaAprovacaoCommand(notasEnvioWfAprovacao, fechamentoTurmaDisciplinaId, periodoEscolar, usuarioLogado, componenteCurricular, turma));
        }

        private bool EnviarWfAprovacao(Usuario usuarioLogado)
        {
            if (turmaFechamento.AnoLetivo != DateTime.Today.Year && !usuarioLogado.EhGestorEscolar())
                return true;

            return false;
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
            {
                fechamento = repositorioFechamentoTurmaDisciplina.ObterPorId(id);
                fechamento.FechamentoTurma = repositorioFechamentoTurmaConsulta.ObterPorId(fechamento.FechamentoTurmaId);
            }                

            fechamento.AtualizarSituacao(SituacaoFechamento.EmProcessamento);
            fechamento.DisciplinaId = fechamentoDto.DisciplinaId;
            fechamento.Justificativa = fechamentoDto.Justificativa;

            return fechamento;
        }

        private async Task<(PeriodoEscolar periodoEscolar, PeriodoDto periodoFechamento)> ObterPeriodoEscolarFechamentoReabertura(long tipoCalendarioId, Ue ue, int bimestre)
        {
            var periodoFechamento = await servicoPeriodoFechamento.ObterPorTipoCalendarioSme(tipoCalendarioId);
            var periodoFechamentoBimestre = periodoFechamento?.FechamentosBimestres.FirstOrDefault(x => x.Bimestre == bimestre);

            if (periodoFechamento == null || periodoFechamentoBimestre == null)
            {
                var hoje = DateTime.Today;
                var tipodeEventoReabertura = ObterTipoEventoFechamentoBimestre();

                if (await repositorioEvento.TemEventoNosDiasETipo(hoje, hoje, (TipoEvento)tipodeEventoReabertura.Codigo, tipoCalendarioId, ue.CodigoUe, ue.Dre.CodigoDre))
                {
                    var fechamentoReabertura = await repositorioFechamentoReabertura.ObterReaberturaFechamentoBimestrePorDataReferencia(bimestre, hoje, tipoCalendarioId, ue.Dre.CodigoDre, ue.CodigoUe);
                    if (fechamentoReabertura == null)
                        throw new NegocioException($"Não localizado período de fechamento em aberto para turma informada no {bimestre}º Bimestre");

                    return ((await repositorioPeriodoEscolar.ObterPorTipoCalendario(tipoCalendarioId)).FirstOrDefault(a => a.Bimestre == bimestre)
                        , new PeriodoDto(fechamentoReabertura.Inicio, fechamentoReabertura.Fim));
                }
            }

            return (periodoFechamentoBimestre?.PeriodoEscolar
                , periodoFechamentoBimestre is null ?
                    null :
                    new PeriodoDto(periodoFechamentoBimestre.InicioDoFechamento.Value, periodoFechamentoBimestre.FinalDoFechamento.Value));
        }

        private EventoTipo ObterTipoEventoFechamentoBimestre()
        {
            EventoTipo tipoEvento = repositorioEventoTipo.ObterPorCodigo((int)TipoEvento.FechamentoBimestre);
            if (tipoEvento == null)
                throw new NegocioException($"Não foi possível localizar o tipo de evento {TipoEvento.FechamentoBimestre.ObterAtributo<DisplayAttribute>().Name}.");
            return tipoEvento;
        }

        private async Task VerificaSeProfessorPodePersistirTurma(string codigoRf, string turmaId, DateTime dataAula, PeriodoDto periodoFechamento,string disciplinaId, Usuario usuario = null)
        {
            if (usuario == null)
                usuario = await servicoUsuario.ObterUsuarioLogado();

            var podePersistir = await servicoUsuario.PodePersistirTurmaDisciplina(codigoRf, turmaId, disciplinaId, dataAula);

            if (!usuario.EhProfessorCj() && !podePersistir)
                throw new NegocioException("Você não pode fazer alterações ou inclusões nesta turma, componente curricular e data.");
        }

        private async Task<bool> VerificarAtribuicao(string codigoRf, string turmaId, DateTime data, PeriodoDto periodoFechamento)
        {
            var atribuicao = await servicoEOL.VerificaAtribuicaoProfessorTurma(codigoRf, turmaId);
            if (atribuicao is null)
                return false;

            if (atribuicao.SemAtribuicaoNaData(data))
            {
                // Se o motivo for 34 então tem acesso até o final do fechamento
                if (atribuicao.CodigoMotivoDisponibilizacao.Equals(34))
                    return atribuicao.SemAtribuicaoNaData(periodoFechamento.Fim);
            }

            return true;
        }

    }
}
