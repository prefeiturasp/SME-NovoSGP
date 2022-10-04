﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static SME.SGP.Aplicacao.GerarNotificacaoAlteracaoLimiteDiasUseCase;

namespace SME.SGP.Aplicacao
{
    public class SalvarFechamentoCommandHandler : IRequestHandler<SalvarFechamentoCommand, AuditoriaPersistenciaDto>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;
        private readonly IRepositorioFechamentoNota repositorioFechamentoNota;
        private readonly IRepositorioFechamentoTurma repositorioFechamentoTurma;
        private readonly IRepositorioFechamentoAluno repositorioFechamentoAluno;
        private readonly IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina;

        public SalvarFechamentoCommandHandler(IUnitOfWork unitOfWork, IMediator mediator, IRepositorioFechamentoNota repositorioFechamentoNota, 
            IRepositorioFechamentoTurma repositorioFechamentoTurma, IRepositorioFechamentoAluno repositorioFechamentoAluno,
            IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new ArgumentNullException(nameof(repositorioFechamentoNota));
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
            this.repositorioFechamentoAluno = repositorioFechamentoAluno ?? throw new ArgumentNullException(nameof(repositorioFechamentoAluno));
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
        }
        
        public async Task<AuditoriaPersistenciaDto> Handle(SalvarFechamentoCommand request, CancellationToken cancellationToken)
        {
            var fechamentoTurma = request.FechamentoFinalTurmaDisciplina;
            var notasEmAprovacao = new List<FechamentoNotaDto>();

            var consolidacaoNotasAlunos = new List<ConsolidacaoNotaAlunoDto>();

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery(), cancellationToken);
            var turma = await ObterTurma(fechamentoTurma.TurmaId);

            var fechamentoTurmaDisciplina = await MapearParaEntidade(fechamentoTurma.Id, fechamentoTurma, turma);

            var emAprovacao = await ExigeAprovacao(turma, usuarioLogado);
            var tipoNota = await mediator.Send(new ObterTipoNotaPorTurmaIdQuery(turma.Id, turma.TipoTurma), cancellationToken);
            
            if (fechamentoTurma.Justificativa != null)
            {
                var tamanhoJustificativa = fechamentoTurma.Justificativa.Length;
                var limite = int.Parse(FechamentoTurmaDisciplinaEnum.TamanhoCampoJustificativa.Description());

                if (tamanhoJustificativa > limite)
                    throw new NegocioException("Justificativa não pode ter mais que " + limite + " caracteres");
            }

            var tipoCalendario = await mediator.Send(
                new ObterTipoCalendarioIdPorAnoLetivoEModalidadeQuery(
                    turma.ModalidadeCodigo == Modalidade.EJA
                        ? ModalidadeTipoCalendario.EJA
                        : ModalidadeTipoCalendario.FundamentalMedio, turma.AnoLetivo, turma.Semestre),
                cancellationToken);

            var ue = turma.Ue;

            var bimestre = fechamentoTurma.EhFinal && turma.ModalidadeTipoCalendario != ModalidadeTipoCalendario.EJA ? 4
                : fechamentoTurma.EhFinal && turma.ModalidadeTipoCalendario.Equals(ModalidadeTipoCalendario.EJA) ? 2
                : fechamentoTurma.Bimestre;

            var periodos = await ObterPeriodoEscolarFechamentoReabertura(tipoCalendario, ue, bimestre);
            var periodoEscolar = periodos.periodoEscolar;

            if (periodoEscolar == null)
                throw new NegocioException($"Não localizado período de fechamento em aberto para turma informada no {fechamentoTurma.Bimestre}º Bimestre");

            if (!fechamentoTurma.EhFinal)
            {
                await CarregaFechamentoTurma(fechamentoTurmaDisciplina, turma, periodoEscolar);

                // Valida Permissão do Professor na Turma/Disciplina            
                if (!turma.EhTurmaEdFisicaOuItinerario() && !usuarioLogado.EhGestorEscolar() && !usuarioLogado.EhPerfilSME() && !usuarioLogado.EhPerfilDRE())
                    await VerificaSeProfessorPodePersistirTurma(usuarioLogado.CodigoRf, fechamentoTurma.TurmaId, periodoEscolar.PeriodoFim, periodos.periodoFechamento, fechamentoTurma.DisciplinaId.ToString(), usuarioLogado);
            }

            var parametroAlteracaoNotaFechamento = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.AprovacaoAlteracaoNotaFechamento, turma.AnoLetivo), cancellationToken);

            IEnumerable<FechamentoAluno> fechamentoAlunos;

            var disciplinasEol = await mediator.Send(new ObterDisciplinasPorIdsQuery(new[] { fechamentoTurmaDisciplina.DisciplinaId }), cancellationToken);

            var disciplina = disciplinasEol is null
                ? throw new NegocioException("Não foi possível localizar o componente curricular no EOL.")
                : disciplinasEol.FirstOrDefault();

            // reprocessar do fechamento de componente sem nota deve atualizar a sintise de frequencia
            if (fechamentoTurma.ComponenteSemNota && fechamentoTurma.Id > 0)
                fechamentoAlunos = await AtualizaSinteseAlunos(fechamentoTurma.Id, periodoEscolar.PeriodoFim, disciplina, turma.AnoLetivo, turma.CodigoTurma);
            else
                fechamentoAlunos = await CarregarFechamentoAlunoENota(fechamentoTurma.Id, fechamentoTurma.NotaConceitoAlunos, usuarioLogado, parametroAlteracaoNotaFechamento, turma.AnoLetivo);

            var alunos = (await mediator.Send(new ObterAlunosPorTurmaQuery(turma.CodigoTurma), cancellationToken)).ToList();
            var parametroDiasAlteracao = await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.QuantidadeDiasAlteracaoNotaFinal, turma.AnoLetivo), cancellationToken);

            var diasAlteracao = DateTime.Today.DayOfYear - fechamentoTurmaDisciplina.CriadoEm.Date.DayOfYear;
            var acimaDiasPermitidosAlteracao = parametroDiasAlteracao != null && diasAlteracao > int.Parse(parametroDiasAlteracao);
            var alunosComNotaAlterada = "";

            //salvar para todos
            unitOfWork.IniciarTransacao();
            try
            {
                var fechamentoTurmaId = fechamentoTurmaDisciplina.FechamentoTurma.Id > 0
                    ? fechamentoTurmaDisciplina.FechamentoTurma.Id
                    : await repositorioFechamentoTurma.SalvarAsync(fechamentoTurmaDisciplina.FechamentoTurma);

                fechamentoTurmaDisciplina.FechamentoTurmaId = fechamentoTurmaId;

                var fechamentoTurmaDisciplinaId = await repositorioFechamentoTurmaDisciplina.SalvarAsync(fechamentoTurmaDisciplina);

                foreach (var fechamentoAluno in fechamentoAlunos)
                {
                    fechamentoAluno.FechamentoTurmaDisciplinaId = fechamentoTurmaDisciplinaId;
                    await repositorioFechamentoAluno.SalvarAsync(fechamentoAluno);

                    foreach (var fechamentoNota in fechamentoAluno.FechamentoNotas)
                    {
                        try
                        {
                            // Regra de não reprovação em 2020
                            if (turma.AnoLetivo == 2020)
                                ValidarNotasFechamento2020(fechamentoNota);
                            
                            var notaConceitoAprovacaoAluno = fechamentoTurma.NotaConceitoAlunos.Select(a => new 
                                { 
                                    a.Nota,
                                    a.ConceitoId,
                                    a.NotaAnterior, 
                                    a.ConceitoIdAnterior, 
                                    a.CodigoAluno
                                })
                                .FirstOrDefault(x => x.CodigoAluno == fechamentoAluno.AlunoCodigo);                            
                            
                            //-> Caso não estiver em aprovação ou estiver em aprovação e não houver qualquer lançamento de nota de fechamento,
                            //   deve gerar o registro do fechamento da nota inicial.
                            if (!emAprovacao || (emAprovacao && fechamentoNota.Id == 0))
                            {
                                if (fechamentoNota is { Id: > 0 })
                                {
                                    if (tipoNota.TipoNota == TipoNota.Nota)
                                    {
                                        if (fechamentoNota.Nota.HasValue && fechamentoNota.Nota.GetValueOrDefault().CompareTo(notaConceitoAprovacaoAluno?.NotaAnterior) != 0)
                                            await mediator.Send(new SalvarHistoricoNotaFechamentoCommand(notaConceitoAprovacaoAluno?.NotaAnterior, fechamentoNota.Nota, fechamentoNota.Id), cancellationToken);
                                    }
                                    else
                                    if (fechamentoNota.ConceitoId.GetValueOrDefault() != notaConceitoAprovacaoAluno?.ConceitoIdAnterior.GetValueOrDefault())
                                        await mediator.Send(new SalvarHistoricoConceitoFechamentoCommand(notaConceitoAprovacaoAluno?.ConceitoIdAnterior, fechamentoNota.ConceitoId, fechamentoNota.Id), cancellationToken);
                                }

                                if (emAprovacao && fechamentoNota.Id == 0)
                                {
                                    fechamentoNota.Nota = notaConceitoAprovacaoAluno.NotaAnterior;
                                    fechamentoNota.ConceitoId = notaConceitoAprovacaoAluno.ConceitoIdAnterior;
                                }

                                fechamentoNota.FechamentoAlunoId = fechamentoAluno.Id;
                                fechamentoNota.FechamentoAluno = fechamentoAluno;

                                await repositorioFechamentoNota.SalvarAsync(fechamentoNota);

                                ConsolidacaoNotasAlunos(periodoEscolar.Bimestre, consolidacaoNotasAlunos, turma, fechamentoAluno.AlunoCodigo, fechamentoNota);
                            }
                            
                            if (!emAprovacao)
                                continue;

                            AdicionaAprovacaoNota(notasEmAprovacao, fechamentoNota, fechamentoAluno.AlunoCodigo,
                                notaConceitoAprovacaoAluno?.Nota, notaConceitoAprovacaoAluno?.NotaAnterior,
                                notaConceitoAprovacaoAluno?.ConceitoId, notaConceitoAprovacaoAluno?.ConceitoIdAnterior);
                        }
                        catch (NegocioException e)
                        {
                            var mensagem = $"Não foi possível salvar a nota do componente [{fechamentoNota.DisciplinaId}] aluno [{fechamentoAluno.AlunoCodigo}]";
                            await LogarErro(mensagem, e, LogNivel.Negocio);
                        }
                        catch (Exception e)
                        {
                            var mensagem = $"Não foi possível salvar a nota do componente [{fechamentoNota.DisciplinaId}] aluno [{fechamentoAluno.AlunoCodigo}]";
                            await LogarErro(mensagem, e, LogNivel.Critico);
                        }
                    }

                    if (fechamentoTurma.EhFinal || fechamentoTurma.ComponenteSemNota) 
                        continue;
                    
                    var notaAlunoAlterada = fechamentoTurma.NotaConceitoAlunos.FirstOrDefault(n => n.CodigoAluno.Equals(fechamentoAluno.AlunoCodigo));

                    if (fechamentoTurma.Id <= 0 || !acimaDiasPermitidosAlteracao || notaAlunoAlterada == null ||
                        alunosComNotaAlterada.Contains(fechamentoAluno.AlunoCodigo))
                    {
                        continue;
                    }

                    var aluno = alunos.FirstOrDefault(a => a.CodigoAluno == fechamentoAluno.AlunoCodigo);

                    if (aluno != null)
                        alunosComNotaAlterada += $"<li>{aluno.CodigoAluno} - {aluno.NomeAluno}</li>";
                }

                await EnviarNotasAprovacao(notasEmAprovacao, usuarioLogado);
                unitOfWork.PersistirTransacao();

                var alunosDaTurma = (await mediator.Send(new ObterAlunosPorTurmaQuery(turma.CodigoTurma), cancellationToken)).ToList();

                foreach (var consolidacaoNotaAlunoDto in consolidacaoNotasAlunos)
                {
                    var dadosAluno = alunosDaTurma.FirstOrDefault(f => f.CodigoAluno.Equals(consolidacaoNotaAlunoDto.AlunoCodigo));
                    consolidacaoNotaAlunoDto.Inativo = dadosAluno == null || dadosAluno.Inativo;

                    await mediator.Send(new ConsolidacaoNotaAlunoCommand(consolidacaoNotaAlunoDto), cancellationToken);
                }   

                if (alunosComNotaAlterada.Length > 0)
                {
                    var dados = new GerarNotificacaoAlteracaoLimiteDiasParametros
                    {
                        TurmaFechamento = turma,
                        UsuarioLogado = usuarioLogado,
                        Ue = ue,
                        Bimestre = fechamentoTurma.Bimestre,
                        AlunosComNotaAlterada = alunosComNotaAlterada
                    };
                    
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.GerarNotificacaoAlteracaoLimiteDias, dados, Guid.NewGuid()), cancellationToken);
                }

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
                    fechamentoTurmaDisciplina.FechamentoTurma.TurmaId,
                    fechamentoTurma.ComponenteSemNota,
                    disciplina.RegistraFrequencia);

                if (!emAprovacao)
                    await ExcluirPendenciaAusenciaFechamento(fechamentoTurmaDisciplina.DisciplinaId, fechamentoTurmaDisciplina.FechamentoTurma.TurmaId, periodoEscolar, usuarioLogado, fechamentoTurma.EhFinal);

                var auditoria = (AuditoriaPersistenciaDto)fechamentoTurmaDisciplina;
                auditoria.EmAprovacao = notasEmAprovacao.Any();

                if (parametroAlteracaoNotaFechamento.Ativo && turma.AnoLetivo < DateTimeExtension.HorarioBrasilia().Year)
                    auditoria.MensagemConsistencia = $"{tipoNota.TipoNota.Name()} registrados com sucesso. Em até 24 horas será enviado para aprovação e será considerado válido após a aprovação do último nível.";         
                else
                    auditoria.MensagemConsistencia = $" {tipoNota.TipoNota.Name()} registrados com sucesso.";
                
                await InserirOuAtualizarCache(fechamentoTurma, emAprovacao);

                return auditoria;
            }
            catch (Exception e)
            {
                await LogarErro("Erro ao persistir notas de fechamento", e, LogNivel.Critico);

                unitOfWork.Rollback();
                throw e;
            }
        }
        
        private async Task InserirOuAtualizarCache(FechamentoFinalTurmaDisciplinaDto fechamentoFinalTurmaDisciplina, bool emAprovacao)
        {
            var disciplinaId = fechamentoFinalTurmaDisciplina.EhRegencia ? fechamentoFinalTurmaDisciplina.DisciplinaId :
                fechamentoFinalTurmaDisciplina.NotaConceitoAlunos.First().DisciplinaId;

            var fechamentosNotasConceitos = fechamentoFinalTurmaDisciplina.NotaConceitoAlunos.Select(notaConceitoAluno => new FechamentoNotaConceitoDto
            {
                CodigoAluno = notaConceitoAluno.CodigoAluno, 
                Nota = notaConceitoAluno.Nota, 
                ConceitoId = notaConceitoAluno.ConceitoId
            }).ToList();

            await mediator.Send(new InserirOuAtualizarCacheFechamentoNotaConceitoCommand(disciplinaId,
                fechamentoFinalTurmaDisciplina.TurmaId,
                fechamentosNotasConceitos, emAprovacao, fechamentoFinalTurmaDisciplina.Bimestre));
        }        

        private void ConsolidacaoNotasAlunos(int bimestre, List<ConsolidacaoNotaAlunoDto> consolidacaoNotasAlunos, Turma turma, string AlunoCodigo, FechamentoNota fechamentoNota)
        {
            consolidacaoNotasAlunos.Add(new ConsolidacaoNotaAlunoDto()
            {
                AlunoCodigo = AlunoCodigo,
                TurmaId = turma.Id,
                Bimestre = ObterBimestre(bimestre),
                AnoLetivo = turma.AnoLetivo,
                Nota = fechamentoNota.Nota,
                ConceitoId = fechamentoNota.ConceitoId,
                ComponenteCurricularId = fechamentoNota.DisciplinaId
            });
        }

        private static int? ObterBimestre(int? bimestre)
        {
            return bimestre.HasValue ? bimestre.Value > 0 ? bimestre : null : null;
        }

        private Task GerarPendenciasFechamento(long componenteCurricularId, string turmaCodigo, string turmaNome, DateTime periodoEscolarInicio, DateTime periodoEscolarFim, int bimestre, Usuario usuario, long fechamentoTurmaDisciplinaId, string justificativa, string criadoRF, long turmaId, bool componenteSemNota = false, bool registraFrequencia = true)
            => mediator.Send(new IncluirFilaGeracaoPendenciasFechamentoCommand(
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

        private async Task<Turma> ObterTurma(string turmaId)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(turmaId));
            if (turma == null)
                throw new NegocioException("Não foi encontrada a turma informada.");

            return turma;
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
            if (parametro == null)
                throw new NegocioException($"Não foi possível localizar o parametro 'AprovacaoAlteracaoNotafechamento' para o ano {anoLetivo}");

            return parametro.Ativo;
        }

        private async Task<DisciplinaDto> ObterComponenteCurricular(long componenteCurricularId)
        {
            var componentes = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(new long[] { componenteCurricularId }));

            if (!componentes.Any())
                throw new NegocioException($"Componente Curricular do Fechamento ({componenteCurricularId}) não localizado!");

            return componentes.FirstOrDefault();
        }

        private async Task<FechamentoTurmaDisciplina> MapearParaEntidade(long id, FechamentoFinalTurmaDisciplinaDto fechamentoDto, Turma turma)
        {
            FechamentoTurmaDisciplina fechamentoTurmaDisciplina = null;

            if (fechamentoDto.EhFinal)
            {
                //colocar verificar regencia
                var disciplinaId = fechamentoDto.EhRegencia ? fechamentoDto.DisciplinaId : fechamentoDto.NotaConceitoAlunos.First().DisciplinaId;

                var fechamentoFinalTurma = await mediator.Send(new ObterFechamentoTurmaPorTurmaIdQuery(turma.Id));

                if (fechamentoFinalTurma == null)
                    fechamentoFinalTurma = new FechamentoTurma(0, turma.Id);
                else
                    //verificar retorno e disciplina regencia
                    fechamentoTurmaDisciplina = await mediator.Send(new ObterFechamentoTurmaDisciplinaQuery(fechamentoDto.TurmaId, disciplinaId));

                if (fechamentoTurmaDisciplina == null)
                    fechamentoTurmaDisciplina = new FechamentoTurmaDisciplina { DisciplinaId = disciplinaId, Situacao = SituacaoFechamento.ProcessadoComSucesso };

                fechamentoTurmaDisciplina.FechamentoTurma = fechamentoFinalTurma;

                foreach (var agrupamentoAluno in fechamentoDto.NotaConceitoAlunos.GroupBy(a => a.CodigoAluno))
                {
                    var fechamentoAluno = await mediator.Send(new ObterFechamentoAlunoPorTurmaIdQuery(fechamentoTurmaDisciplina.DisciplinaId, agrupamentoAluno.Key));

                    if (fechamentoAluno == null)
                        fechamentoAluno = new FechamentoAluno { AlunoCodigo = agrupamentoAluno.Key };

                    fechamentoTurmaDisciplina.FechamentoAlunos.Add(fechamentoAluno);
                }
            }
            else
            {
                if (id > 0)
                {
                    fechamentoTurmaDisciplina = await mediator.Send(new ObterFechamentoTurmaDisciplinaPorIdQuery(id));
                    fechamentoTurmaDisciplina.FechamentoTurma = await mediator.Send(new ObterFechamentoTurmaPorIdQuery(fechamentoTurmaDisciplina.FechamentoTurmaId));
                }

                if (fechamentoTurmaDisciplina == null)
                {
                    fechamentoTurmaDisciplina = new FechamentoTurmaDisciplina()
                    {
                        DisciplinaId = fechamentoDto.DisciplinaId,
                        Justificativa = fechamentoDto.Justificativa,
                        Situacao = SituacaoFechamento.EmProcessamento
                    };
                }
            }

            return fechamentoTurmaDisciplina;
        }

        private async Task<(PeriodoEscolar periodoEscolar, PeriodoDto periodoFechamento)> ObterPeriodoEscolarFechamentoReabertura(long tipoCalendarioId, Ue ue, int bimestre)
        {
            var periodoFechamento = await mediator.Send(new ObterPeriodoFechamentoPorCalendarioIdQuery(tipoCalendarioId));
            var periodoFechamentoBimestre = periodoFechamento?.FechamentosBimestres.FirstOrDefault(x => x.Bimestre == bimestre);

            if (periodoFechamento == null || periodoFechamentoBimestre == null)
            {
                var hoje = DateTime.Today;
                var tipodeEventoReabertura = await mediator.Send(new ObterEventoTipoIdPorCodigoQuery(TipoEvento.FechamentoBimestre));

                if (await mediator.Send(new ExisteEventoNaDataPorTipoDreUEQuery(hoje, tipoCalendarioId, (TipoEvento)tipodeEventoReabertura, ue.CodigoUe, ue.Dre.CodigoDre)))
                {
                    var fechamentoReabertura = await mediator.Send(new ObterTurmaEmPeriodoFechamentoQuery(bimestre, hoje, tipoCalendarioId, ue.Dre.CodigoDre, ue.CodigoUe));

                    if (fechamentoReabertura == null)
                        throw new NegocioException($"Não localizado período de fechamento em aberto para turma informada no {bimestre}º Bimestre");

                    return ((await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioQuery(tipoCalendarioId))).FirstOrDefault(a => a.Bimestre == bimestre),
                        new PeriodoDto(fechamentoReabertura.Inicio, fechamentoReabertura.Fim));
                }
            }

            return (periodoFechamentoBimestre?.PeriodoEscolar, periodoFechamentoBimestre is null ? null :
                new PeriodoDto(periodoFechamentoBimestre.InicioDoFechamento.Value, periodoFechamentoBimestre.FinalDoFechamento.Value));
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
                var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaPorPeriodoQuery(turma.Id, periodoEscolar.Id));

                if (fechamentoTurma == null)
                    fechamentoTurma = new FechamentoTurma(turma, periodoEscolar);

                fechamentoTurmaDisciplina.FechamentoTurma = fechamentoTurma;
            }
        }

        private async Task VerificaSeProfessorPodePersistirTurma(string codigoRf, string turmaId, DateTime dataAula, PeriodoDto periodoFechamento, string disciplinaId, Usuario usuario = null)
        {
            if (usuario == null)
                usuario = await mediator.Send(new ObterUsuarioLogadoQuery());

            var podePersistir = await mediator.Send(new ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery(Int64.Parse(disciplinaId), turmaId, dataAula, usuario));

            if (!usuario.EhProfessorCj() && !podePersistir)
                throw new NegocioException("Você não pode fazer alterações ou inclusões nesta turma, componente curricular e data.");
        }

        private async Task<IEnumerable<FechamentoAluno>> AtualizaSinteseAlunos(long fechamentoTurmaDisciplinaId, DateTime dataReferencia, DisciplinaDto disciplina, int anoLetivo, string codigoTurma)
        {
            var fechamentoAlunos = await mediator.Send(new ObterFechamentoAlunoPorDisciplinaIdQuery(fechamentoTurmaDisciplinaId));

            foreach (var fechamentoAluno in fechamentoAlunos)
            {
                foreach (var fechamentoNota in fechamentoAluno.FechamentoNotas)
                {
                    var frequencia = await mediator.Send(new ObterFrequenciaPorAlunoDisciplinaDataQuery(fechamentoAluno.AlunoCodigo, fechamentoNota.DisciplinaId.ToString(), dataReferencia, codigoTurma));
                    var percentualFrequencia = frequencia == null ? 100 : frequencia.PercentualFrequencia;
                    var sinteseDto = await ObterSinteseAluno(percentualFrequencia, disciplina, anoLetivo);

                    fechamentoNota.SinteseId = (long)sinteseDto.Id;
                }
            }

            return fechamentoAlunos;
        }

        public async Task<SinteseDto> ObterSinteseAluno(double? percentualFrequencia, DisciplinaDto disciplina, int anoLetivo)
        {
            var sintese = percentualFrequencia == null ?
                SinteseEnum.NaoFrequente :
                percentualFrequencia >= await ObterFrequenciaMedia(disciplina, anoLetivo) ?
                SinteseEnum.Frequente :
                SinteseEnum.NaoFrequente;

            return new SinteseDto()
            {
                Id = sintese,
                Valor = sintese.Name()
            };
        }

        public async Task<double> ObterFrequenciaMedia(DisciplinaDto disciplina, int anoLetivo)
        {
            double mediaFrequencia = 0;

            if (mediaFrequencia == 0)
            {
                if (disciplina.Regencia || !disciplina.LancaNota)
                    mediaFrequencia = double.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.CompensacaoAusenciaPercentualRegenciaClasse, anoLetivo)));
                else
                    mediaFrequencia = double.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.CompensacaoAusenciaPercentualFund2, anoLetivo)));
            }

            return mediaFrequencia;
        }

        private async Task<IEnumerable<FechamentoAluno>> CarregarFechamentoAlunoENota(long fechamentoTurmaDisciplinaId, IEnumerable<FechamentoNotaDto> fechamentoNotasDto, Usuario usuarioLogado, ParametrosSistema parametroAlteracaoNotaFechamento, int turmaAnoLetivo)
        {
            var fechamentoAlunos = new List<FechamentoAluno>();

            if (fechamentoTurmaDisciplinaId > 0)
            {
                fechamentoAlunos = (await mediator.Send(new ObterFechamentoAlunoPorDisciplinaIdQuery(fechamentoTurmaDisciplinaId)))
                    .Where(x => fechamentoNotasDto.Any(a => a.CodigoAluno == x.AlunoCodigo)).ToList();
            }

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
                                await mediator.Send(new SalvarHistoricoNotaFechamentoCommand(notaFechamento.Nota, fechamentoNotaDto.Nota, notaFechamento.Id));
                        }
                        else
                        {
                            if (fechamentoNotaDto.ConceitoId != notaFechamento.ConceitoId)
                                await mediator.Send(new SalvarHistoricoConceitoFechamentoCommand(notaFechamento.ConceitoId, fechamentoNotaDto.ConceitoId, notaFechamento.Id));
                        }

                        if (EnviarWfAprovacao(usuarioLogado, turmaAnoLetivo) && parametroAlteracaoNotaFechamento.Ativo)
                        {
                            fechamentoNotaDto.Id = notaFechamento.Id;
                            fechamentoNotaDto.NotaAnterior = notaFechamento.Nota;
                            fechamentoNotaDto.ConceitoIdAnterior = notaFechamento.ConceitoId;
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

        private static bool EnviarWfAprovacao(Usuario usuarioLogado, int turmaAnoLetivo)
        {
            if (turmaAnoLetivo != DateTime.Today.Year && !usuarioLogado.EhGestorEscolar())
                return true;

            return false;
        }

        private async Task EnviarNotasAprovacao(List<FechamentoNotaDto> notasEmAprovacao, Usuario usuarioLogado)
        {
            if (notasEmAprovacao.Any())
                await mediator.Send(new EnviarNotasFechamentoParaAprovacaoCommand(notasEmAprovacao, usuarioLogado));
        }

        private async Task ExcluirPendenciaAusenciaFechamento(long disciplinaId, long turmaId, PeriodoEscolar periodoEscolar, Usuario usuarioLogado, bool ehFinal)
        {
            if (ehFinal)
                await mediator.Send(new PublicaFilaExcluirPendenciaAusenciaFechamentoCommand(disciplinaId, null, turmaId, usuarioLogado));
            else
                await mediator.Send(new PublicaFilaExcluirPendenciaAusenciaFechamentoCommand(disciplinaId, periodoEscolar.Id, turmaId, usuarioLogado));
        }

        private static void ValidarNotasFechamento2020(FechamentoNota notaDto)
        {
            if (notaDto.ConceitoId.HasValue && notaDto.ConceitoId.Value == 3)
                throw new NegocioException("Não é possível atribuir conceito NS (Não Satisfatório) pois em 2020 não há retenção dos estudantes conforme o Art 5º da LEI Nº 17.437 DE 12 DE AGOSTO DE 2020.");
            else
            if (!notaDto.SinteseId.HasValue && notaDto.Nota < 5)
                throw new NegocioException("Não é possível atribuir uma nota menor que 5 pois em 2020 não há retenção dos estudantes conforme o Art 5º da LEI Nº 17.437 DE 12 DE AGOSTO DE 2020.");
        }

        private static void AdicionaAprovacaoNota(List<FechamentoNotaDto> notasEmAprovacao, FechamentoNota fechamentoNota,
            string alunoCodigo, double? nota,  double? notaAnterior, long? conceitoId, long? conceitoIdAnterior)
        {
            notasEmAprovacao.Add(new FechamentoNotaDto()
            {
                Id = fechamentoNota.Id,
                NotaAnterior = notaAnterior,
                Nota = nota,
                ConceitoIdAnterior = conceitoIdAnterior,
                ConceitoId = conceitoId,
                CodigoAluno = alunoCodigo,
                DisciplinaId = fechamentoNota.DisciplinaId
            });
        }

        private Task LogarErro(string mensagem, Exception e, LogNivel nivel)
            => mediator.Send(new SalvarLogViaRabbitCommand(mensagem, nivel, LogContexto.Fechamento, e.Message));

        private FechamentoNota MapearParaEntidade(FechamentoNotaDto fechamentoNotaDto)
            => fechamentoNotaDto == null ? null :
                new FechamentoNota()
                {
                    DisciplinaId = fechamentoNotaDto.DisciplinaId,
                    Nota = fechamentoNotaDto.Nota,
                    ConceitoId = fechamentoNotaDto.ConceitoId,
                    SinteseId = fechamentoNotaDto.SinteseId
                };
    }
}
