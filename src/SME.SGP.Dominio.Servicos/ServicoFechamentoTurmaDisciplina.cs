using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static SME.SGP.Aplicacao.GerarNotificacaoAlteracaoLimiteDiasUseCase;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoFechamentoTurmaDisciplina : IServicoFechamentoTurmaDisciplina
    {
        private readonly IConsultasDisciplina consultasDisciplina;
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
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IServicoPeriodoFechamento servicoPeriodoFechamento;
        private readonly IServicoUsuario servicoUsuario;        
        private readonly IRepositorioCache repositorioCache;
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
                                                IRepositorioTurmaConsulta repositorioTurma,
                                                IServicoPeriodoFechamento servicoPeriodoFechamento,
                                                IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar,
                                                IRepositorioTipoCalendarioConsulta repositorioTipoCalendario,
                                                IRepositorioParametrosSistemaConsulta repositorioParametrosSistema,
                                                IConsultasDisciplina consultasDisciplina,
                                                IServicoNotificacao servicoNotificacao,
                                                IServicoUsuario servicoUsuario,
                                                IUnitOfWork unitOfWork,
                                                IConsultasSupervisor consultasSupervisor,
                                                IRepositorioEvento repositorioEvento,
                                                IRepositorioEventoTipo repositorioEventoTipo,
                                                IRepositorioFechamentoReabertura repositorioFechamentoReabertura,
                                                IRepositorioCache repositorioCache,
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
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.consultasSupervisor = consultasSupervisor ?? throw new ArgumentNullException(nameof(consultasSupervisor));
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
            this.repositorioEventoTipo = repositorioEventoTipo ?? throw new ArgumentNullException(nameof(repositorioEventoTipo));
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new ArgumentNullException(nameof(repositorioFechamentoReabertura));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task GerarNotificacaoAlteracaoLimiteDias(Turma turma, Usuario usuarioLogado, Ue ue, int bimestre, string alunosComNotaAlterada)
        {
            var dataAtual = DateTime.Now;

            var mensagem = $"<p>A(s) nota(s)/conceito(s) final(is) da turma {turma.Nome} da {ue.Nome} (DRE {ue.Dre.Nome}) no bimestre {bimestre} de {turma.AnoLetivo} foram alterados pelo Professor " +
                $"{usuarioLogado.Nome} ({usuarioLogado.CodigoRf}) em  {dataAtual.ToString("dd/MM/yyyy")} às {dataAtual.ToString("HH:mm")} para o(s) seguinte(s) aluno(s):</p><br/>{alunosComNotaAlterada} ";
            var listaCPs = await mediator.Send(
                new ObterFuncionariosPorCargoUeQuery(turma.Ue.CodigoUe, (long)Cargo.CP));
            var listaDiretores = await mediator.Send(
                new ObterFuncionariosPorCargoUeQuery(turma.Ue.CodigoUe, (long)Cargo.Diretor));

            var filtro = new FiltroObterSupervisorEscolasDto
            {
                UeCodigo = turma.Ue.CodigoUe,
                DreCodigo = turma.Ue.Dre.CodigoDre
            };

            var listaSupervisores = await consultasSupervisor.ObterAtribuicaoResponsavel(filtro);

            var usuariosNotificacao = new List<UsuarioEolRetornoDto>();

            if (listaCPs.NaoEhNulo())
                usuariosNotificacao.AddRange(listaCPs);

            if (listaDiretores.NaoEhNulo())
                usuariosNotificacao.AddRange(listaDiretores);

            if (listaSupervisores.NaoEhNulo())
                usuariosNotificacao.Add(new UsuarioEolRetornoDto() { CodigoRf = listaSupervisores.FirstOrDefault().ResponsavelId, NomeServidor = listaSupervisores.FirstOrDefault().Responsavel });

            foreach (var usuarioNotificacaoo in usuariosNotificacao)
            {
                var usuario = await servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(usuarioNotificacaoo.CodigoRf);

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

                await servicoNotificacao.Salvar(notificacao);
            }
        }

        public async Task Reprocessar(long fechamentoTurmaDisciplinaId, Usuario usuario = null)
        {
            var fechamentoTurmaDisciplina = await mediator.Send(new ObterFechamentoTurmaDisciplinaPorIdQuery(fechamentoTurmaDisciplinaId));

            if (fechamentoTurmaDisciplina.EhNulo())
                throw new NegocioException("Fechamento ainda não realizado para essa turma.");

            var turma = await repositorioTurma.ObterTurmaComUeEDrePorId(fechamentoTurmaDisciplina.FechamentoTurma.TurmaId);
            if (turma.EhNulo())
                throw new NegocioException("Turma não encontrada.");

            var disciplina = await mediator.Send(new ObterComponenteCurricularPorIdQuery(fechamentoTurmaDisciplina.DisciplinaId));
            if (disciplina.EhNulo())
                throw new NegocioException("Componente Curricular não localizado.");

            var periodoEscolar = repositorioPeriodoEscolar.ObterPorId(fechamentoTurmaDisciplina.FechamentoTurma.PeriodoEscolarId.Value);
            if (periodoEscolar.EhNulo())
                throw new NegocioException("Período escolar não encontrado.");

            fechamentoTurmaDisciplina.AdicionarPeriodoEscolar(periodoEscolar);
            fechamentoTurmaDisciplina.AtualizarSituacao(SituacaoFechamento.EmProcessamento);
            repositorioFechamentoTurmaDisciplina.Salvar(fechamentoTurmaDisciplina);

            var usuarioLogado = usuario ?? await servicoUsuario.ObterUsuarioLogado();
            if (turma.TipoTurma != TipoTurma.Programa)
            {
                var fechamentoDto = new FechamentoTurmaDisciplinaPendenciaDto()
                {
                    DisciplinaId = fechamentoTurmaDisciplina.DisciplinaId,
                    CodigoTurma = turma.CodigoTurma,
                    NomeTurma = turma.Nome,
                    PeriodoInicio = periodoEscolar.PeriodoInicio,
                    PeriodoFim = periodoEscolar.PeriodoFim,
                    Bimestre = periodoEscolar.Bimestre,
                    UsuarioId = usuarioLogado.Id,
                    Id = fechamentoTurmaDisciplina.Id,
                    Justificativa = fechamentoTurmaDisciplina.Justificativa,
                    CriadoRF = fechamentoTurmaDisciplina.CriadoRF,
                    TurmaId = turma.Id,
                };

                await mediator.Send(new IncluirFilaGeracaoPendenciasFechamentoCommand(fechamentoDto, !disciplina.LancaNota, disciplina.RegistraFrequencia));
            }
        }

        public async Task<AuditoriaPersistenciaDto> Salvar(long id, FechamentoTurmaDisciplinaDto entidadeDto, bool componenteSemNota = false, bool processamento = false)
        {
            notasEnvioWfAprovacao = new List<FechamentoNotaDto>();

            var consolidacaoNotasAlunos = new List<ConsolidacaoNotaAlunoDto>();

            if (id == 0)
                id = (await mediator.Send(new ObterFechamentoTurmaDisciplinaDTOQuery(entidadeDto.TurmaId, entidadeDto.DisciplinaId, entidadeDto.Bimestre, null)))?.Id ?? 0;

            var fechamentoTurmaDisciplina = MapearParaEntidade(id, entidadeDto);

            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();

            await CarregarTurma(entidadeDto.TurmaId);

            // Valida periodo de fechamento
            var tipoCalendario = await repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(turmaFechamento.AnoLetivo, 
                turmaFechamento.ModalidadeCodigo.ObterModalidadeTipoCalendario(),
                turmaFechamento.Semestre);

            var ue = turmaFechamento.Ue;

            var periodos = await ObterPeriodoEscolarFechamentoReabertura(tipoCalendario.Id, ue, entidadeDto.Bimestre);

            PeriodoEscolar periodoEscolar = periodos.periodoEscolar;

            if (periodos.periodoFechamento.EhNulo())
                throw new NegocioException($"Não localizado período de fechamento em aberto para turma informada no {entidadeDto.Bimestre}º Bimestre");

            await CarregaFechamentoTurma(fechamentoTurmaDisciplina, turmaFechamento, periodoEscolar);
            
            var parametroAlteracaoNotaFechamento = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.AprovacaoAlteracaoNotaFechamento, turmaFechamento.AnoLetivo));

            // Valida Permissão do Professor na Turma/Disciplina            
            if (!turmaFechamento.EhTurmaEdFisicaOuItinerario() && !usuarioLogado.EhGestorEscolar() && !usuarioLogado.EhPerfilSME() && !usuarioLogado.EhPerfilDRE())
            {
                await VerificaSeProfessorPodePersistirTurma(usuarioLogado.CodigoRf, entidadeDto.TurmaId, periodoEscolar.PeriodoFim, entidadeDto.DisciplinaId.ToString(), usuarioLogado);
            }
            
            var mesmoAnoLetivo = turmaFechamento.AnoLetivo == DateTimeExtension.HorarioBrasilia().Year;

            var temPeriodoAberto = await mediator.Send(new TurmaEmPeriodoAbertoQuery(turmaFechamento, DateTimeExtension.HorarioBrasilia().Date, periodoEscolar.Bimestre, mesmoAnoLetivo)); 

            if(!temPeriodoAberto)
                throw new NegocioException(MensagemNegocioComuns.APENAS_EH_POSSIVEL_CONSULTAR_ESTE_REGISTRO_POIS_O_PERIODO_NAO_ESTA_EM_ABERTO);

            IEnumerable<FechamentoAluno> fechamentoAlunos;

            var disciplinaEOL = await consultasDisciplina.ObterDisciplina(fechamentoTurmaDisciplina.DisciplinaId);

            if (disciplinaEOL.EhNulo())
                throw new NegocioException("Não foi possível localizar o componente curricular no EOL.");

            var tipoNotaOuConceito = await mediator.Send(new ObterNotaTipoValorPorTurmaIdQuery(turmaFechamento)); 
            
            // reprocessar do fechamento de componente sem nota deve atualizar a sintise de frequencia
            if (componenteSemNota && id > 0)
                fechamentoAlunos = await AtualizaSinteseAlunos(id, periodoEscolar.PeriodoFim, disciplinaEOL, turmaFechamento.AnoLetivo);
            else
                fechamentoAlunos = await CarregarFechamentoAlunoENota(id, entidadeDto.NotaConceitoAlunos, usuarioLogado, parametroAlteracaoNotaFechamento,tipoNotaOuConceito?.TipoNota);

            var alunos = (await mediator.Send(new ObterAlunosDentroPeriodoQuery(turmaFechamento.CodigoTurma, (periodos.periodoFechamento.Inicio, periodos.periodoFechamento.Fim))))
                        .DistinctBy(a => a.CodigoAluno)
                        .OrderBy(a => a.NomeValido());

            if (!processamento)
            {
                var codigosAlunosAtivos = alunos?.Select(c => c.CodigoAluno)?.Distinct()?.ToArray();
                var codigosAlunosFechamento = fechamentoAlunos.Select(c => c.AlunoCodigo).Distinct().ToArray();

                if (codigosAlunosFechamento.Any(c => !codigosAlunosAtivos.Contains(c)))
                    throw new NegocioException(MensagemNegocioFechamentoNota.EXISTEM_ALUNOS_INATIVOS_FECHAMENTO_NOTA_BIMESTRE);
            }           

            var parametroDiasAlteracao = await repositorioParametrosSistema.ObterValorPorTipoEAno(TipoParametroSistema.QuantidadeDiasAlteracaoNotaFinal, turmaFechamento.AnoLetivo);
            var diasAlteracao = DateTime.Today.DayOfYear - fechamentoTurmaDisciplina.CriadoEm.Date.DayOfYear;
            var acimaDiasPermitidosAlteracao = parametroDiasAlteracao.NaoEhNulo() && diasAlteracao > int.Parse(parametroDiasAlteracao);
            var alunosComNotaAlterada = "";

            unitOfWork.IniciarTransacao();
            try
            {
                var fechamentoTurmaId = fechamentoTurmaDisciplina.FechamentoTurma.Id > 0 ?
                    fechamentoTurmaDisciplina.FechamentoTurma.Id :
                    await repositorioFechamentoTurma.SalvarAsync(fechamentoTurmaDisciplina.FechamentoTurma);

                var turma = await repositorioTurma.ObterTurmaComUeEDrePorId(fechamentoTurmaDisciplina.FechamentoTurma.TurmaId);

                fechamentoTurmaDisciplina.FechamentoTurmaId = fechamentoTurmaId;

                if (turma.TipoTurma == TipoTurma.Programa)
                    fechamentoTurmaDisciplina.AtualizarSituacao(SituacaoFechamento.ProcessadoComSucesso);

                await repositorioFechamentoTurmaDisciplina.SalvarAsync(fechamentoTurmaDisciplina);

                var emAprovacao = await ExigeAprovacao(turma, usuarioLogado);

                foreach (var fechamentoAluno in fechamentoAlunos)
                {
                    fechamentoAluno.FechamentoTurmaDisciplinaId = fechamentoTurmaDisciplina.Id;
                    await repositorioFechamentoAluno.SalvarAsync(fechamentoAluno);

                    foreach (var fechamentoNota in fechamentoAluno.FechamentoNotas)
                    {
                        var semFechamentoNota = fechamentoNota.Id == 0;
                        var ehAprovacaoSemFechamentoNota = emAprovacao && semFechamentoNota;

                        if(fechamentoNota.Nota.NaoEhNulo() && fechamentoNota.Nota > 10)
                            throw new NegocioException(MensagensNegocioLancamentoNota.NOTA_NUMERICA_DEVE_SER_MENOR_OU_IGUAL_A_10);

                        if (!emAprovacao || ehAprovacaoSemFechamentoNota)
                        {
                            FechamentoNota fechamentoNotaClone = null;

                            if (ehAprovacaoSemFechamentoNota)
                            {
                                fechamentoNotaClone = fechamentoNota.Clone();
                                fechamentoNota.Nota = null;
                                fechamentoNota.ConceitoId = null;
                            }
                            
                            fechamentoNota.FechamentoAlunoId = fechamentoAluno.Id;
                            await repositorioFechamentoNota.SalvarAsync(fechamentoNota);

                            if (fechamentoNotaClone.NaoEhNulo())
                            {
                                fechamentoNotaClone.Id = fechamentoNota.Id;
                                notasEnvioWfAprovacao.Add(MapearParaEntidade(fechamentoNotaClone));
                            }

                            if (!emAprovacao && semFechamentoNota && tipoNotaOuConceito.NaoEhNulo())
                            {
                                await SalvarHistoricoNotaFechamento(fechamentoNota.Id, tipoNotaOuConceito.TipoNota,null,fechamentoNota.Nota, null,fechamentoNota.ConceitoId,usuarioLogado.CodigoRf, usuarioLogado.Nome);
                            }
                        }


                        var alunoInativo = alunos.FirstOrDefault(t => t.CodigoAluno == fechamentoAluno.AlunoCodigo)?.Inativo ?? false;

                        ConsolidacaoNotasAlunos(periodoEscolar.Bimestre, consolidacaoNotasAlunos, turmaFechamento, fechamentoAluno.AlunoCodigo, fechamentoNota, alunoInativo);
                    }

                    if (!componenteSemNota)
                    {
                        var notaAlunoAlterada = entidadeDto.NotaConceitoAlunos.FirstOrDefault(n => n.CodigoAluno.Equals(fechamentoAluno.AlunoCodigo));

                        if (id > 0 && acimaDiasPermitidosAlteracao && notaAlunoAlterada.NaoEhNulo() && !alunosComNotaAlterada.Contains(fechamentoAluno.AlunoCodigo))
                        {
                            var aluno = alunos.FirstOrDefault(a => a.CodigoAluno == fechamentoAluno.AlunoCodigo);

                            if (aluno.NaoEhNulo())
                                alunosComNotaAlterada += $"<li>{aluno.CodigoAluno} - {aluno.NomeAluno}</li>";
                        }
                    }
                }

                await EnviarNotasWfAprovacao(usuarioLogado);
                unitOfWork.PersistirTransacao();


                await PersistirCache(turma, entidadeDto.Bimestre, fechamentoAlunos.ToList());

                foreach (var consolidacaoNotaAlunoDto in consolidacaoNotasAlunos)
                    await mediator.Send(new ConsolidacaoNotaAlunoCommand(consolidacaoNotaAlunoDto));

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

                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.GerarNotificacaoAlteracaoLimiteDias, dados, Guid.NewGuid(), null));
                }

                if (turma.TipoTurma != TipoTurma.Programa)
                {
                    var fechamentoDto = new FechamentoTurmaDisciplinaPendenciaDto()
                    {
                        DisciplinaId = fechamentoTurmaDisciplina.DisciplinaId,
                        CodigoTurma = turma.CodigoTurma,
                        NomeTurma = turma.Nome,
                        PeriodoInicio = periodoEscolar.PeriodoInicio,
                        PeriodoFim = periodoEscolar.PeriodoFim,
                        Bimestre = periodoEscolar.Bimestre,
                        UsuarioId = usuarioLogado.Id,
                        Id = fechamentoTurmaDisciplina.Id,
                        Justificativa = fechamentoTurmaDisciplina.Justificativa,
                        CriadoRF = fechamentoTurmaDisciplina.CriadoRF,
                        TurmaId = fechamentoTurmaDisciplina.FechamentoTurma.TurmaId,
                    };

                    await mediator.Send(new IncluirFilaGeracaoPendenciasFechamentoCommand(fechamentoDto, componenteSemNota, disciplinaEOL.RegistraFrequencia));
                }

                await mediator.Send(new PublicaFilaExcluirPendenciaAusenciaFechamentoCommand(fechamentoTurmaDisciplina.DisciplinaId, periodoEscolar.Id, turmaFechamento.Id, usuarioLogado));

                var auditoria = (AuditoriaPersistenciaDto)fechamentoTurmaDisciplina;
                auditoria.EmAprovacao = notasEnvioWfAprovacao.Any();

                if (parametroAlteracaoNotaFechamento.Ativo && turmaFechamento.AnoLetivo < DateTimeExtension.HorarioBrasilia().Year && !usuarioLogado.EhGestorEscolar())
                    auditoria.MensagemConsistencia = MensagensNegocioLancamentoNota.REGISTRADO_COM_SUCESSO_EM_24_HORAS_SERA_ENVIADO_PARA_APROVACAO; 
                else
                    auditoria.MensagemConsistencia = "Suas informações foram salvas com sucesso.";

                return auditoria;
            }
            catch (Exception)
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        private async Task PersistirCache(Turma turma,
                                         int bimestre,
                                         List<FechamentoAluno> fechamentosAlunos)
        {
            var notasFechamentoFinaisNoCache = new List<NotaConceitoBimestreComponenteDto>();

            foreach (var fechamentoAluno in fechamentosAlunos)
            {
                var nomeChaveCache = ObterChaveNotaConceitoFechamentoTurmaTodosBimestresEFinal(turma.CodigoTurma,fechamentoAluno.AlunoCodigo);
                notasFechamentoFinaisNoCache = await repositorioCache.ObterObjetoAsync<List<NotaConceitoBimestreComponenteDto>>(nomeChaveCache);

                if (notasFechamentoFinaisNoCache.NaoEhNulo())
                {
                    foreach (var fechamentoNota in fechamentoAluno.FechamentoNotas)
                    {
                        AtualizarNotasFinaisParaCache(notasFechamentoFinaisNoCache,
                            fechamentoNota,
                            bimestre,
                            fechamentoAluno.AlunoCodigo,
                            turma.CodigoTurma);
                    }
                    await mediator.Send(new SalvarCachePorValorObjetoCommand(ObterChaveNotaConceitoFechamentoTurmaTodosBimestresEFinal(turma.CodigoTurma,fechamentoAluno.AlunoCodigo), notasFechamentoFinaisNoCache));
                }
            }
        }

        private void AtualizarNotasFinaisParaCache(List<NotaConceitoBimestreComponenteDto> notasFinais, 
                                                   FechamentoNota fechamentoNota,
                                                   int bimestre,
                                                   string codigoAluno, 
                                                   string codigoTurma)
        {
            var notaFinalAluno = notasFinais.FirstOrDefault(c => c.AlunoCodigo == codigoAluno && 
                                                                 c.ComponenteCurricularCodigo == fechamentoNota.DisciplinaId &&
                                                                 c.Bimestre == bimestre);

            if (notaFinalAluno.EhNulo())
            {
                notasFinais.Add(new NotaConceitoBimestreComponenteDto
                {
                    AlunoCodigo = codigoAluno,
                    Nota = fechamentoNota.Nota,
                    ConceitoId = fechamentoNota.ConceitoId,
                    ComponenteCurricularCodigo = fechamentoNota.DisciplinaId,
                    TurmaCodigo =  codigoTurma,
                    Bimestre = bimestre
                });
            }
            else
            {
                notaFinalAluno.Nota = fechamentoNota.Nota;
                notaFinalAluno.ConceitoId = fechamentoNota.ConceitoId;
            }
        }

        private static string ObterChaveNotaConceitoFechamentoTurmaTodosBimestresEFinal(string codigoTurma, string codigoAluno)
        {
            return string.Format(NomeChaveCache.NOTA_CONCEITO_FECHAMENTO_TURMA_ALUNO_BIMESTRES_E_FINAL, codigoTurma,codigoAluno);
        }

        private static void ConsolidacaoNotasAlunos(int bimestre, List<ConsolidacaoNotaAlunoDto> consolidacaoNotasAlunos, Turma turma, string AlunoCodigo, FechamentoNota fechamentoNota, bool inativo)
        {
            consolidacaoNotasAlunos.Add(new ConsolidacaoNotaAlunoDto()
            {
                AlunoCodigo = AlunoCodigo,
                TurmaId = turma.Id,
                Bimestre = bimestre > 0 ? bimestre : null,
                AnoLetivo = turma.AnoLetivo,
                Nota = fechamentoNota.Nota,
                ConceitoId = fechamentoNota.ConceitoId,
                ComponenteCurricularId = fechamentoNota.DisciplinaId,
                Inativo = inativo
            });
        }

        private async Task<IEnumerable<FechamentoAluno>> AtualizaSinteseAlunos(long fechamentoTurmaDisciplinaId, DateTime dataReferencia, DisciplinaDto disciplina, int anoLetivo)
        {
            var fechamentoAlunos = await repositorioFechamentoAlunoConsulta.ObterPorFechamentoTurmaDisciplina(fechamentoTurmaDisciplinaId);

            var alunosCodigos = fechamentoAlunos.Select(c => c.AlunoCodigo).Distinct().ToArray();
            var disciplinasIds = fechamentoAlunos.Select(c => c.FechamentoNotas.Select(a => a.DisciplinaId.ToString()).Distinct().ToArray()).FirstOrDefault();
            var frequencias = await mediator.Send(new ObterPorAlunosDisciplinasDataQuery(alunosCodigos, disciplinasIds, dataReferencia));

            foreach (var fechamentoAluno in fechamentoAlunos)
            {
                foreach (var fechamentoNota in fechamentoAluno.FechamentoNotas)
                {
                    var frequencia = frequencias.FirstOrDefault(c => c.CodigoAluno == fechamentoAluno.AlunoCodigo &&
                        c.DisciplinaId == fechamentoNota.DisciplinaId.ToString());

                    var percentualFrequencia = frequencia.EhNulo() ? 0 : frequencia.PercentualFrequencia;
                    var sinteseDto = await mediator.Send(new ObterSinteseAlunoQuery(percentualFrequencia, disciplina, anoLetivo));

                    fechamentoNota.SinteseId = (long)sinteseDto.Id;
                }
            }

            return fechamentoAlunos;
        }

        private async Task<bool> ExigeAprovacao(Turma turma, Usuario usuarioLogado)
        {
            return turma.AnoLetivo < DateTime.Today.Year
                && !usuarioLogado.EhGestorEscolar()
                && await ParametroAprovacaoAtivo(turma.AnoLetivo);
        }

        private async Task<bool> ParametroAprovacaoAtivo(int anoLetivo)
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.AprovacaoAlteracaoNotaFechamento, anoLetivo));
            if (parametro.EhNulo())
                throw new NegocioException($"Não foi possível localizar o parametro 'AprovacaoAlteracaoNotafechamento' para o ano {anoLetivo}");

            return parametro.Ativo;
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
                if (fechamentoTurma.EhNulo())
                    fechamentoTurma = new FechamentoTurma(turma, periodoEscolar);

                fechamentoTurmaDisciplina.FechamentoTurma = fechamentoTurma;
            }
        }

        private async Task<IEnumerable<FechamentoAluno>> CarregarFechamentoAlunoENota(long fechamentoTurmaDisciplinaId, IEnumerable<FechamentoNotaDto> fechamentoNotasDto, Usuario usuarioLogado, ParametrosSistema parametroAlteracaoNotaFechamento, TipoNota? tipoNotaOuConceito)
        {
            var fechamentoAlunos = new List<FechamentoAluno>();
            int indiceFechamentoAntigo = -1;

            if (fechamentoTurmaDisciplinaId > 0)
            {
                fechamentoAlunos = (await repositorioFechamentoAlunoConsulta.ObterPorFechamentoTurmaDisciplina(fechamentoTurmaDisciplinaId))
                    .Where(c => fechamentoNotasDto.Any(a => a.CodigoAluno == c.AlunoCodigo)).ToList();
            }

            foreach (var agrupamentoNotasAluno in fechamentoNotasDto.GroupBy(g => g.CodigoAluno))
            {
                var fechamentoAluno = fechamentoAlunos.FirstOrDefault(c => c.AlunoCodigo == agrupamentoNotasAluno.Key);
                if (fechamentoAluno.EhNulo())
                {
                    fechamentoAluno = new FechamentoAluno() { AlunoCodigo = agrupamentoNotasAluno.Key, FechamentoTurmaDisciplinaId = fechamentoTurmaDisciplinaId };
                    indiceFechamentoAntigo = -1;
                }
                else
                    indiceFechamentoAntigo = fechamentoAlunos.IndexOf(fechamentoAluno);

                foreach (var fechamentoNotaDto in agrupamentoNotasAluno)
                {
                    var notaFechamento = fechamentoAluno.FechamentoNotas.FirstOrDefault(x => x.DisciplinaId == fechamentoNotaDto.DisciplinaId);
                    
                    if (notaFechamento.NaoEhNulo())
                    {
                        if (EnviarWfAprovacao(usuarioLogado) && parametroAlteracaoNotaFechamento.Ativo)
                        {
                            fechamentoNotaDto.Id = notaFechamento.Id;
                            fechamentoNotaDto.NotaAnterior = notaFechamento.Nota;
                            fechamentoNotaDto.ConceitoIdAnterior = notaFechamento.ConceitoId;
                            notasEnvioWfAprovacao.Add(fechamentoNotaDto);
                        }
                        else
                        {
                            if(tipoNotaOuConceito.HasValue)
                                await SalvarHistoricoNotaFechamento(notaFechamento.Id, tipoNotaOuConceito.Value,
                                    notaFechamento.Nota,
                                    fechamentoNotaDto.Nota, notaFechamento.ConceitoId,
                                    fechamentoNotaDto.ConceitoId, usuarioLogado.CodigoRf, usuarioLogado.Nome);
                            
                            notaFechamento.Nota = fechamentoNotaDto.Nota;
                            notaFechamento.ConceitoId = fechamentoNotaDto.ConceitoId;
                            notaFechamento.SinteseId = fechamentoNotaDto.SinteseId;
                        }
                    }
                    else
                        fechamentoAluno.AdicionarNota(MapearParaEntidade(fechamentoNotaDto));
                }
                
                if(indiceFechamentoAntigo >= 0 && fechamentoAlunos.Any())
                    fechamentoAlunos.RemoveAt(indiceFechamentoAntigo);
                
                fechamentoAlunos.Add(fechamentoAluno);
            }

            return fechamentoAlunos;
        }
        
        private async Task SalvarHistoricoNotaFechamento(long fechamentoNotaId, TipoNota tipoNota, double? notaAnterior, double? notaAtual, long? conceitoIdAnterior, long? conceitoIdAtual, string criadoRf, string criadoPor)
        {
            if (tipoNota == TipoNota.Nota)
            {
                if (notaAtual != notaAnterior)
                    await mediator.Send(new SalvarHistoricoNotaFechamentoCommand(notaAnterior, notaAtual,fechamentoNotaId,criadoRF:criadoRf, criadoPor:criadoPor));
            }
            else
            {
                if (conceitoIdAtual != conceitoIdAnterior)
                    await mediator.Send(new SalvarHistoricoConceitoFechamentoCommand(conceitoIdAnterior,conceitoIdAtual, fechamentoNotaId,criadoRF:criadoRf, criadoPor:criadoPor));
            }
        }

        private async Task CarregarTurma(string turmaCodigo)
        {
            turmaFechamento = await repositorioTurma.ObterTurmaComUeEDrePorCodigo(turmaCodigo);
            if (turmaFechamento.EhNulo())
                throw new NegocioException($"Turma com código [{turmaCodigo}] não localizada!");
        }

        private async Task EnviarNotasWfAprovacao(Usuario usuarioLogado)
        {
            if (notasEnvioWfAprovacao.Any())
                await mediator.Send(new EnviarNotasFechamentoParaAprovacaoCommand(notasEnvioWfAprovacao, usuarioLogado));
        }

        private bool EnviarWfAprovacao(Usuario usuarioLogado)
        {
            if (turmaFechamento.AnoLetivo != DateTime.Today.Year && !usuarioLogado.EhGestorEscolar())
                return true;

            return false;
        }

        private FechamentoNota MapearParaEntidade(FechamentoNotaDto fechamentoNotaDto)
            => fechamentoNotaDto.EhNulo() ? null :
              new FechamentoNota()
              {
                  DisciplinaId = fechamentoNotaDto.DisciplinaId,
                  Nota = fechamentoNotaDto.Nota,
                  ConceitoId = fechamentoNotaDto.ConceitoId,
                  SinteseId = fechamentoNotaDto.SinteseId
              };

        private FechamentoNotaDto MapearParaEntidade(FechamentoNota fechamentoNota)
            => fechamentoNota.EhNulo() ? null :
              new FechamentoNotaDto()
              {
                  Id = fechamentoNota.Id,
                  DisciplinaId = fechamentoNota.DisciplinaId,
                  Nota = fechamentoNota.Nota,
                  ConceitoId = fechamentoNota.ConceitoId,
                  SinteseId = fechamentoNota.SinteseId
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
            var periodoFechamento = await servicoPeriodoFechamento.ObterPorTipoCalendarioSme(tipoCalendarioId, Aplicacao.SGP);
            var periodoFechamentoBimestre = periodoFechamento?.FechamentosBimestres.FirstOrDefault(x => x.Bimestre == bimestre);

            if (periodoFechamento.EhNulo() || periodoFechamentoBimestre.EhNulo())
            {
                var hoje = DateTime.Today;
                var tipodeEventoReabertura = ObterTipoEventoFechamentoBimestre();

                if (await repositorioEvento.TemEventoNosDiasETipo(hoje, hoje, (TipoEvento)tipodeEventoReabertura.Codigo, tipoCalendarioId, ue.CodigoUe, ue.Dre.CodigoDre))
                {
                    var fechamentoReabertura = await repositorioFechamentoReabertura.ObterReaberturaFechamentoBimestrePorDataReferencia(bimestre, hoje, tipoCalendarioId, ue.Dre.CodigoDre, ue.CodigoUe);
                    if (fechamentoReabertura.EhNulo())
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
            if (tipoEvento.EhNulo())
                throw new NegocioException($"Não foi possível localizar o tipo de evento {TipoEvento.FechamentoBimestre.ObterAtributo<DisplayAttribute>().Name}.");
            return tipoEvento;
        }

        private async Task VerificaSeProfessorPodePersistirTurma(string codigoRf, string turmaId, DateTime dataAula, string disciplinaId, Usuario usuario = null)
        {
            if (usuario.EhNulo())
                usuario = await servicoUsuario.ObterUsuarioLogado();

            var podePersistir = await servicoUsuario.PodePersistirTurmaDisciplina(codigoRf, turmaId, disciplinaId, dataAula);

            if (!usuario.EhProfessorCj() && !podePersistir)
                throw new NegocioException(MensagemNegocioComuns.Voce_nao_pode_fazer_alteracoes_ou_inclusoes_nesta_turma_componente_e_data);
        }
    }
}
