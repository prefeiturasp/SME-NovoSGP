using MediatR;
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
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra.Utilitarios;
using static SME.SGP.Aplicacao.GerarNotificacaoAlteracaoLimiteDiasUseCase;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Aplicacao.Queries;

namespace SME.SGP.Aplicacao
{
    public class SalvarFechamentoCommandHandler : IRequestHandler<SalvarFechamentoCommand, AuditoriaPersistenciaFechamentoNotaConceitoTurmaDto>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;
        private readonly IRepositorioFechamentoNota repositorioFechamentoNota;
        private readonly IRepositorioFechamentoTurma repositorioFechamentoTurma;
        private readonly IRepositorioFechamentoAluno repositorioFechamentoAluno;
        private readonly IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina;

        private const int BIMESTRE_2 = 2;
        private const int BIMESTRE_4 = 4;

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
        
        public async Task<AuditoriaPersistenciaFechamentoNotaConceitoTurmaDto> Handle(SalvarFechamentoCommand request, CancellationToken cancellationToken)
        {
            var fechamentoTurma = request.FechamentoFinalTurmaDisciplina;
            var notasEmAprovacao = new List<FechamentoNotaDto>();

            var consolidacaoNotasAlunos = new List<ConsolidacaoNotaAlunoDto>();

            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance, cancellationToken);
            var turma = await ObterTurma(fechamentoTurma.TurmaId);

            var fechamentoTurmaDisciplina = await MapearParaEntidade(fechamentoTurma.Id, fechamentoTurma, turma);

            var emAprovacao = await ExigeAprovacao(turma, usuarioLogado);
            var tipoNota = await mediator.Send(new ObterNotaTipoValorPorTurmaIdQuery(turma), cancellationToken);
            
            if (fechamentoTurma.Justificativa.NaoEhNulo())
            {
                var tamanhoJustificativa = await mediator.Send(new ObterTamanhoCaracteresJustificativaNotaQuery(fechamentoTurma.Justificativa));
                var limite = int.Parse(FechamentoTurmaDisciplinaEnum.TamanhoCampoJustificativa.Description());

                if (tamanhoJustificativa > limite)
                    throw new NegocioException("Justificativa não pode ter mais que " + limite + " caracteres");
            }

            var tipoCalendario = await mediator.Send(
                new ObterTipoCalendarioIdPorAnoLetivoEModalidadeQuery(
                    turma.ModalidadeCodigo.ObterModalidadeTipoCalendario(), turma.AnoLetivo, turma.Semestre),
                cancellationToken);

            var ue = turma.Ue;
            var bimestre = fechamentoTurma.Bimestre;

            if (fechamentoTurma.EhFinal)
                bimestre = turma.ModalidadeTipoCalendario.EhEjaOuCelp() ? BIMESTRE_2 : BIMESTRE_4;

            var periodos = await ObterPeriodoEscolarFechamentoReabertura(tipoCalendario, ue, bimestre);
            var periodoEscolar = periodos.periodoEscolar;

            if (periodoEscolar.EhNulo())
                throw new NegocioException($"Não localizado período de fechamento em aberto para turma informada no {fechamentoTurma.Bimestre}º Bimestre");

            if (!fechamentoTurma.EhFinal)
            {
                await CarregaFechamentoTurma(fechamentoTurmaDisciplina, turma, periodoEscolar);

                // Valida Permissão do Professor na Turma/Disciplina            
                if (!turma.EhTurmaEdFisicaOuItinerario() && !usuarioLogado.EhGestorEscolar() && !usuarioLogado.EhPerfilSME() && !usuarioLogado.EhPerfilDRE())
                    await VerificaSeProfessorPodePersistirTurma(fechamentoTurma.TurmaId, periodoEscolar.PeriodoFim, fechamentoTurma.DisciplinaId.ToString(), usuarioLogado);
            }

            var parametroAlteracaoNotaFechamento = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.AprovacaoAlteracaoNotaFechamento, turma.AnoLetivo), cancellationToken);

            IEnumerable<FechamentoAluno> fechamentoAlunos;

            var disciplinasEol = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(new[] { fechamentoTurmaDisciplina.DisciplinaId }), cancellationToken);

            var disciplina = disciplinasEol is null
                ? throw new NegocioException("Não foi possível localizar o componente curricular no EOL.")
                : disciplinasEol.FirstOrDefault();

            // reprocessar do fechamento de componente sem nota deve atualizar a sintise de frequencia
            if (fechamentoTurma.ComponenteSemNota && fechamentoTurmaDisciplina.Id > 0) 
                fechamentoAlunos = await AtualizaSinteseAlunos(fechamentoTurmaDisciplina.Id, periodoEscolar.PeriodoFim, disciplina, turma.AnoLetivo, turma.CodigoTurma);
            else
                fechamentoAlunos = await CarregarFechamentoAlunoENota(fechamentoTurmaDisciplina.Id, fechamentoTurma.NotaConceitoAlunos, usuarioLogado, parametroAlteracaoNotaFechamento, turma.AnoLetivo);

            var alunos = (await mediator.Send(new ObterAlunosPorTurmaQuery(turma.CodigoTurma), cancellationToken)).ToList();
            var parametroDiasAlteracao = await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.QuantidadeDiasAlteracaoNotaFinal, turma.AnoLetivo), cancellationToken);

            var diasAlteracao = DateTime.Today.DayOfYear - fechamentoTurmaDisciplina.CriadoEm.Date.DayOfYear;
            var acimaDiasPermitidosAlteracao = parametroDiasAlteracao.NaoEhNulo() && diasAlteracao > int.Parse(parametroDiasAlteracao);
            var alunosComNotaAlterada = "";

            if (fechamentoAlunos.Any())
                VerificaSeEhNotaValida(fechamentoAlunos);

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
                                    a.CodigoAluno,
                                    a.DisciplinaId
                                })
                                .FirstOrDefault(x => x.CodigoAluno == fechamentoAluno.AlunoCodigo && x.DisciplinaId == fechamentoNota.DisciplinaId);                            
                            
                            var semFechamentoNota = (fechamentoNota.Id == 0);

                            //-> Caso não estiver em aprovação ou estiver em aprovação e não houver qualquer lançamento de nota de fechamento,
                            //   deve gerar o registro do fechamento da nota inicial.
                            if (!emAprovacao || (emAprovacao && semFechamentoNota))
                            {
                                double? notaAnterior = null;
                                long? conceitoIdAnterior = null;
                                
                                if (!emAprovacao)
                                {
                                    if (!semFechamentoNota)
                                    {
                                        notaAnterior = fechamentoNota.Nota;
                                        conceitoIdAnterior = fechamentoNota.ConceitoId;
                                    }

                                    if (notaConceitoAprovacaoAluno.NaoEhNulo())
                                    {
                                        fechamentoNota.Nota = notaConceitoAprovacaoAluno.Nota;
                                        fechamentoNota.ConceitoId = notaConceitoAprovacaoAluno.ConceitoId;
                                    }
                                }

                                fechamentoNota.FechamentoAlunoId = fechamentoAluno.Id;
                                fechamentoNota.FechamentoAluno = fechamentoAluno;

                                if (emAprovacao && semFechamentoNota)
                                {
                                    fechamentoNota.Nota = null;
                                    fechamentoNota.ConceitoId = null;
                                }

                                await repositorioFechamentoNota.SalvarAsync(fechamentoNota);
                                
                                if (!emAprovacao)
                                    await SalvarHistoricoNotaFechamentoNovo(fechamentoNota, tipoNota.TipoNota, usuarioLogado.CodigoRf, usuarioLogado.Nome, notaAnterior, conceitoIdAnterior);

                                var alunoInativo = alunos.FirstOrDefault(t => t.CodigoAluno == fechamentoAluno.AlunoCodigo)?.Inativo ?? false;
                                ConsolidacaoNotasAlunos(periodoEscolar.Bimestre, consolidacaoNotasAlunos, turma, fechamentoAluno.AlunoCodigo, fechamentoNota, alunoInativo, fechamentoTurma.EhFinal);

                                await AtualizaCacheFechamentoFinal(fechamentoTurma.EhFinal, fechamentoNota, fechamentoAluno, turma.CodigoTurma);
                            }
                            
                            if (!emAprovacao)
                                continue;

                            if (notaConceitoAprovacaoAluno.NaoEhNulo())
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

                    if (fechamentoTurma.Id <= 0 || !acimaDiasPermitidosAlteracao || notaAlunoAlterada.EhNulo() ||
                        alunosComNotaAlterada.Contains(fechamentoAluno.AlunoCodigo))
                    {
                        continue;
                    }

                    var aluno = alunos.FirstOrDefault(a => a.CodigoAluno == fechamentoAluno.AlunoCodigo);

                    if (aluno.NaoEhNulo())
                        alunosComNotaAlterada += $"<li>{aluno.CodigoAluno} - {aluno.NomeAluno}</li>";
                }

                await EnviarNotasAprovacao(notasEmAprovacao, usuarioLogado);
                unitOfWork.PersistirTransacao();

                await RemoverCache(string.Format(NomeChaveCache.FECHAMENTO_NOTA_TURMA_PERIODO_COMPONENTE, turma.Id, periodoEscolar.Id, fechamentoTurma.DisciplinaId), cancellationToken);
                await RemoverCache(string.Format(NomeChaveCache.FECHAMENTO_NOTA_TURMA_BIMESTRE, turma.CodigoTurma, bimestre), cancellationToken);

                var alunosDaTurma = (await mediator.Send(new ObterAlunosPorTurmaQuery(turma.CodigoTurma), cancellationToken)).ToList();

                foreach (var consolidacaoNotaAlunoDto in consolidacaoNotasAlunos)
                {
                    var dadosAluno = alunosDaTurma.FirstOrDefault(f => f.CodigoAluno.Equals(consolidacaoNotaAlunoDto.AlunoCodigo));
                    consolidacaoNotaAlunoDto.Inativo = dadosAluno.EhNulo() || dadosAluno.Inativo;

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

                    await mediator.Send(new IncluirFilaGeracaoPendenciasFechamentoCommand(fechamentoDto, fechamentoTurma.ComponenteSemNota, disciplina.RegistraFrequencia));
                }
                if (!emAprovacao)
                    await ExcluirPendenciaAusenciaFechamento(fechamentoTurmaDisciplina.DisciplinaId, fechamentoTurmaDisciplina.FechamentoTurma.TurmaId, periodoEscolar, usuarioLogado, fechamentoTurma.EhFinal);

                var auditoriaFechamentoNotaConceitoTurma = (AuditoriaPersistenciaFechamentoNotaConceitoTurmaDto)fechamentoTurmaDisciplina;
                auditoriaFechamentoNotaConceitoTurma.EmAprovacao = notasEmAprovacao.Any();

                if (emAprovacao)
                    auditoriaFechamentoNotaConceitoTurma.MensagemConsistencia = $"{tipoNota.TipoNota.Name()} registrados com sucesso. Em até 24 horas será enviado para aprovação e será considerado válido após a aprovação do último nível.";         
                else
                    auditoriaFechamentoNotaConceitoTurma.MensagemConsistencia = $" {tipoNota.TipoNota.Name()} registrados com sucesso.";
                
                auditoriaFechamentoNotaConceitoTurma.AuditoriaAlteracao = AuditoriaUtil.MontarTextoAuditoriaAlteracao(fechamentoTurmaDisciplina, tipoNota.EhNota());
                auditoriaFechamentoNotaConceitoTurma.AuditoriaInclusao = AuditoriaUtil.MontarTextoAuditoriaInclusao(fechamentoTurmaDisciplina, tipoNota.EhNota());
                auditoriaFechamentoNotaConceitoTurma.Situacao = fechamentoTurmaDisciplina.Situacao;
                auditoriaFechamentoNotaConceitoTurma.SituacaoNome = fechamentoTurmaDisciplina.Situacao.Name();
                auditoriaFechamentoNotaConceitoTurma.DataFechamento = fechamentoTurmaDisciplina.AlteradoEm ?? fechamentoTurmaDisciplina.CriadoEm;

                await InserirOuAtualizarCache(fechamentoTurma, emAprovacao);

                return auditoriaFechamentoNotaConceitoTurma;
            }
            catch (Exception e)
            {
                await LogarErro("Erro ao persistir notas de fechamento", e, LogNivel.Critico);

                unitOfWork.Rollback();
                throw;
            }
        }

        private void VerificaSeEhNotaValida(IEnumerable<FechamentoAluno> fechamentoAlunos)
        {
            if (fechamentoAlunos.Any(f=> f.FechamentoNotas.Any(f=> f.Nota.NaoEhNulo() && f.Nota > 10)))
                throw new NegocioException(MensagensNegocioLancamentoNota.NOTA_NUMERICA_DEVE_SER_MENOR_OU_IGUAL_A_10);
        }
        private async Task SalvarHistoricoNotaFechamentoNovo(FechamentoNota fechamentoNota, TipoNota tipoNota,string criadoRf, string criadoPor, double? notaAnterior, long? conceitoIdAnterior)
        {
            if (tipoNota == TipoNota.Nota)
            {
                if (fechamentoNota.Nota.GetValueOrDefault().CompareTo(notaAnterior) != 0)
                    await mediator.Send(new SalvarHistoricoNotaFechamentoCommand(notaAnterior, fechamentoNota.Nota, fechamentoNota.Id, criadoRF:criadoRf, criadoPor:criadoPor));
            }
            else if (fechamentoNota.ConceitoId.GetValueOrDefault().CompareTo(conceitoIdAnterior) != 0)
                await mediator.Send(new SalvarHistoricoConceitoFechamentoCommand(conceitoIdAnterior, fechamentoNota.ConceitoId,fechamentoNota.Id, criadoRF:criadoRf, criadoPor:criadoPor));
        }
        
        private async Task InserirOuAtualizarCache(FechamentoFinalTurmaDisciplinaDto fechamentoFinalTurmaDisciplina, bool emAprovacao)
        {
            var componenteCurricularId = fechamentoFinalTurmaDisciplina.EhRegencia ? fechamentoFinalTurmaDisciplina.DisciplinaId :
                fechamentoFinalTurmaDisciplina.NotaConceitoAlunos.First().DisciplinaId;

            var fechamentosNotasConceitos = fechamentoFinalTurmaDisciplina.NotaConceitoAlunos.Select(notaConceitoAluno => new FechamentoNotaConceitoDto
            {
                DiscplinaId = notaConceitoAluno.DisciplinaId,
                CodigoAluno = notaConceitoAluno.CodigoAluno, 
                Nota = notaConceitoAluno.Nota, 
                ConceitoId = notaConceitoAluno.ConceitoId
            }).ToList();

            await mediator.Send(new InserirOuAtualizarCacheFechamentoNotaConceitoCommand(componenteCurricularId,
                fechamentoFinalTurmaDisciplina.TurmaId,
                fechamentosNotasConceitos, emAprovacao, fechamentoFinalTurmaDisciplina.Bimestre));
        }

        private void ConsolidacaoNotasAlunos(int bimestre, List<ConsolidacaoNotaAlunoDto> consolidacaoNotasAlunos, Turma turma, string AlunoCodigo, FechamentoNota fechamentoNota, bool inativo, bool ehFinal)
        {
            consolidacaoNotasAlunos.Add(new ConsolidacaoNotaAlunoDto()
            {
                AlunoCodigo = AlunoCodigo,
                TurmaId = turma.Id,
                Bimestre = ObterBimestre(bimestre, ehFinal),
                AnoLetivo = turma.AnoLetivo,
                Nota = fechamentoNota.Nota,
                ConceitoId = fechamentoNota.ConceitoId,
                ComponenteCurricularId = fechamentoNota.DisciplinaId,
                Inativo = inativo
            });
        }

        private static int? ObterBimestre(int? bimestre, bool ehFinal)
        {
            if (!ehFinal || (bimestre.HasValue && bimestre.Value > 0))
                return bimestre;

            return null;
        }

        private async Task<Turma> ObterTurma(string turmaId)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(turmaId));
            if (turma.EhNulo())
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
            if (parametro.EhNulo())
                throw new NegocioException($"Não foi possível localizar o parametro 'AprovacaoAlteracaoNotafechamento' para o ano {anoLetivo}");

            return parametro.Ativo;
        }

        private async Task<FechamentoTurmaDisciplina> MapearParaEntidade(long id, FechamentoFinalTurmaDisciplinaDto fechamentoDto, Turma turma)
        {
            FechamentoTurmaDisciplina fechamentoTurmaDisciplina = null;

            if (fechamentoDto.EhFinal)
            {
                //colocar verificar regencia
                var disciplinaId = fechamentoDto.EhRegencia ? fechamentoDto.DisciplinaId : fechamentoDto.NotaConceitoAlunos.First().DisciplinaId;

                var fechamentoFinalTurma = await mediator.Send(new ObterFechamentoTurmaPorTurmaIdQuery(turma.Id));

                if (fechamentoFinalTurma.EhNulo())
                    fechamentoFinalTurma = new FechamentoTurma(0, turma.Id);
                else
                    //verificar retorno e disciplina regencia
                    fechamentoTurmaDisciplina = await mediator.Send(new ObterFechamentoTurmaDisciplinaQuery(fechamentoDto.TurmaId, disciplinaId));

                if (fechamentoTurmaDisciplina.EhNulo())
                    fechamentoTurmaDisciplina = new FechamentoTurmaDisciplina { DisciplinaId = disciplinaId, Situacao = SituacaoFechamento.ProcessadoComSucesso };

                fechamentoTurmaDisciplina.FechamentoTurma = fechamentoFinalTurma;

                foreach (var agrupamentoAluno in fechamentoDto.NotaConceitoAlunos.GroupBy(a => a.CodigoAluno))
                {
                    var fechamentoAluno = await mediator.Send(new ObterFechamentoAlunoPorTurmaIdQuery(fechamentoTurmaDisciplina.DisciplinaId, agrupamentoAluno.Key));

                    if (fechamentoAluno.EhNulo())
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

                if (fechamentoTurmaDisciplina.EhNulo())
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

            if (periodoFechamento.EhNulo() || periodoFechamentoBimestre.EhNulo())
            {
                var hoje = DateTime.Today;
                var tipodeEventoReabertura = await mediator.Send(new ObterEventoTipoIdPorCodigoQuery(TipoEvento.FechamentoBimestre));

                if (await mediator.Send(new ExisteEventoNaDataPorTipoDreUEQuery(hoje, tipoCalendarioId, (TipoEvento)tipodeEventoReabertura, ue.CodigoUe, ue.Dre.CodigoDre)))
                {
                    var fechamentoReabertura = await mediator.Send(new ObterTurmaEmPeriodoFechamentoReaberturaQuery(bimestre, hoje, tipoCalendarioId, ue.Dre.CodigoDre, ue.CodigoUe));

                    if (fechamentoReabertura.EhNulo())
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

                if (fechamentoTurma.EhNulo())
                    fechamentoTurma = new FechamentoTurma(turma, periodoEscolar);

                fechamentoTurmaDisciplina.FechamentoTurma = fechamentoTurma;
            }
        }

        private async Task VerificaSeProfessorPodePersistirTurma(string turmaId, DateTime dataAula, string disciplinaId, Usuario usuario = null)
        {
            if (usuario.EhNulo())
                usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            var podePersistir = await mediator.Send(new ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery(Int64.Parse(disciplinaId), turmaId, dataAula, usuario));

            if (!usuario.EhProfessorCj() && !podePersistir)
                throw new NegocioException(MensagemNegocioComuns.Voce_nao_pode_fazer_alteracoes_ou_inclusoes_nesta_turma_componente_e_data);
        }

        private async Task<IEnumerable<FechamentoAluno>> AtualizaSinteseAlunos(long fechamentoTurmaDisciplinaId, DateTime dataReferencia, DisciplinaDto disciplina, int anoLetivo, string codigoTurma)
        {
            var fechamentoAlunos = await mediator.Send(new ObterFechamentoAlunoPorDisciplinaIdQuery(fechamentoTurmaDisciplinaId));

            foreach (var fechamentoAluno in fechamentoAlunos)
            {
                foreach (var fechamentoNota in fechamentoAluno.FechamentoNotas)
                {
                    var frequencia = await mediator.Send(new ObterFrequenciaPorAlunoDisciplinaDataQuery(fechamentoAluno.AlunoCodigo, fechamentoNota.DisciplinaId.ToString(), dataReferencia, codigoTurma));
                    var percentualFrequencia = frequencia.EhNulo() ? 0 : frequencia.PercentualFrequencia;
                    var sinteseDto = await ObterSinteseAluno(percentualFrequencia, disciplina, anoLetivo);

                    fechamentoNota.SinteseId = (long)sinteseDto.Id;
                }
            }

            return fechamentoAlunos;
        }

        public async Task<SinteseDto> ObterSinteseAluno(double? percentualFrequencia, DisciplinaDto disciplina, int anoLetivo)
        {
            var sintese = SinteseEnum.NaoFrequente;

            if (percentualFrequencia.NaoEhNulo() && percentualFrequencia >= await ObterFrequenciaMedia(disciplina, anoLetivo))
                sintese = SinteseEnum.Frequente;

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
            int indiceFechamentoAntigo = -1;

            if (fechamentoTurmaDisciplinaId > 0)
            {
                fechamentoAlunos = (await mediator.Send(new ObterFechamentoAlunoPorDisciplinaIdQuery(fechamentoTurmaDisciplinaId)))
                    .Where(x => fechamentoNotasDto.Any(a => a.CodigoAluno == x.AlunoCodigo)).ToList();
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
                        if (EnviarWfAprovacao(usuarioLogado, turmaAnoLetivo) && parametroAlteracaoNotaFechamento.Ativo)
                        {
                            fechamentoNotaDto.Id = notaFechamento.Id;
                            fechamentoNotaDto.NotaAnterior = notaFechamento.Nota;
                            fechamentoNotaDto.ConceitoIdAnterior = notaFechamento.ConceitoId;
                        }
                        else
                        {
                            if (!notaFechamento.ConceitoId.HasValue)
                            {
                                if (fechamentoNotaDto.Nota != notaFechamento.Nota)
                                    await mediator.Send(new SalvarHistoricoNotaFechamentoCommand(notaFechamento.Nota, fechamentoNotaDto.Nota, notaFechamento.Id, criadoRF:usuarioLogado.CriadoRF, criadoPor:usuarioLogado.CriadoPor));
                            }
                            else
                            {
                                if (fechamentoNotaDto.ConceitoId != notaFechamento.ConceitoId)
                                    await mediator.Send(new SalvarHistoricoConceitoFechamentoCommand(notaFechamento.ConceitoId, fechamentoNotaDto.ConceitoId, notaFechamento.Id, criadoRF:usuarioLogado.CriadoRF, criadoPor:usuarioLogado.CriadoPor));
                            }

                            notaFechamento.Nota = fechamentoNotaDto.Nota;
                            notaFechamento.ConceitoId = fechamentoNotaDto.ConceitoId;
                            notaFechamento.SinteseId = fechamentoNotaDto.SinteseId;
                        }
                    }
                    else
                        fechamentoAluno.AdicionarNota(MapearParaEntidade(fechamentoNotaDto));
                }

                if (indiceFechamentoAntigo >= 0 && fechamentoAlunos.Any())
                    fechamentoAlunos.RemoveAt(indiceFechamentoAntigo);

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
            else if (!notaDto.SinteseId.HasValue && notaDto.Nota < 5)
                throw new NegocioException("Não é possível atribuir uma nota menor que 5 pois em 2020 não há retenção dos estudantes conforme o Art 5º da LEI Nº 17.437 DE 12 DE AGOSTO DE 2020.");
        }

        private async Task AtualizaCacheFechamentoFinal(bool EhFinal, FechamentoNota fechamentoNota, FechamentoAluno fechamentoAluno, string codigoTurma)
        {
            if (EhFinal)
                await mediator.Send(new AtualizarCacheFechamentoNotaCommand(
                                fechamentoNota,
                                fechamentoAluno.AlunoCodigo,
                                codigoTurma,
                                fechamentoAluno.FechamentoTurmaDisciplinaId));
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
            => fechamentoNotaDto.EhNulo() ? null :
                new FechamentoNota()
                {
                    DisciplinaId = fechamentoNotaDto.DisciplinaId,
                    Nota = fechamentoNotaDto.Nota,
                    ConceitoId = fechamentoNotaDto.ConceitoId,
                    SinteseId = fechamentoNotaDto.SinteseId
                };

        private async Task RemoverCache(string nomeChave, CancellationToken cancellationToken)
        {
            await mediator.Send(new RemoverChaveCacheCommand(nomeChave), cancellationToken);
        }
    }
}
