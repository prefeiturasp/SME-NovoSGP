using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoConselhoClasse : IServicoConselhoClasse
    {
        private readonly IConsultasPeriodoFechamento consultasPeriodoFechamento;
        private readonly IRepositorioConselhoClasse repositorioConselhoClasse;
        private readonly IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno;
        private readonly IRepositorioFechamentoTurma repositorioFechamentoTurma;
        private readonly IRepositorioConselhoClasseNota repositorioConselhoClasseNota;
        private readonly IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina;
        private readonly IRepositorioUe repositorioUe;
        private readonly IRepositorioDre repositorioDre;
        private readonly IConsultasConselhoClasse consultasConselhoClasse;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;
        private readonly IConsultasConselhoClasseNota consultasConselhoClasseNota;
        private readonly IRepositorioConselhoClasseConsolidado repositorioConselhoClasseConsolidado;

        public ServicoConselhoClasse(IRepositorioConselhoClasse repositorioConselhoClasse,
                                     IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno,
                                     IRepositorioFechamentoTurma repositorioFechamentoTurma,
                                     IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina,
                                     IRepositorioUe repositorioUe,
                                     IRepositorioDre repositorioDre,
                                     IConsultasPeriodoFechamento consultasPeriodoFechamento,
                                     IConsultasConselhoClasse consultasConselhoClasse,
                                     IRepositorioConselhoClasseNota repositorioConselhoClasseNota,
                                     IUnitOfWork unitOfWork,
                                     IMediator mediator,
                                     IConsultasConselhoClasseNota consultasConselhoClasseNota,
                                     IRepositorioConselhoClasseConsolidado repositorioConselhoClasseConsolidado)

        {
            this.repositorioConselhoClasse = repositorioConselhoClasse ?? throw new ArgumentNullException(nameof(repositorioConselhoClasse));
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
            this.repositorioConselhoClasseNota = repositorioConselhoClasseNota ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseNota));
            this.consultasConselhoClasse = consultasConselhoClasse ?? throw new ArgumentNullException(nameof(consultasConselhoClasse));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.consultasConselhoClasseNota = consultasConselhoClasseNota ?? throw new ArgumentNullException(nameof(consultasConselhoClasseNota));
            this.repositorioConselhoClasseConsolidado = repositorioConselhoClasseConsolidado ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseConsolidado));
        }

        public async Task<ConselhoClasseNotaRetornoDto> SalvarConselhoClasseAlunoNotaAsync(ConselhoClasseNotaDto conselhoClasseNotaDto, string alunoCodigo, long conselhoClasseId, long fechamentoTurmaId, string codigoTurma, int bimestre)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(codigoTurma));
            if (turma == null) throw new NegocioException("Turma não encontrada");

            var ehAnoAnterior = turma.AnoLetivo != DateTime.Now.Year;
            var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaPorIdAlunoCodigoQuery(fechamentoTurmaId, alunoCodigo, ehAnoAnterior));
            var fechamentoTurmaDisciplina = new FechamentoTurmaDisciplina();
            var periodoEscolar = new PeriodoEscolar();

            if (fechamentoTurma == null)
            {
                if (!ehAnoAnterior) throw new NegocioException("Não existe fechamento de turma para o conselho de classe");

                var ue = repositorioUe.ObterPorId(turma.UeId);
                ue.AdicionarDre(repositorioDre.ObterPorId(ue.DreId));
                turma.AdicionarUe(ue);

                periodoEscolar = await mediator.Send(new ObterPeriodoEscolarPorTurmaBimestreQuery(turma, bimestre));
                if (periodoEscolar == null && bimestre > 0) throw new NegocioException("Período escolar não encontrado");

                fechamentoTurma = new FechamentoTurma()
                {
                    TurmaId = turma.Id,
                    Migrado = false,
                    PeriodoEscolarId = periodoEscolar?.Id,
                    Turma = turma,
                    PeriodoEscolar = periodoEscolar
                };

                fechamentoTurmaDisciplina = new FechamentoTurmaDisciplina()
                {
                    DisciplinaId = conselhoClasseNotaDto.CodigoComponenteCurricular
                };

            }
            else
            {
                if (fechamentoTurma.PeriodoEscolarId != null)
                    periodoEscolar = await mediator.Send(new ObterPeriodoEscolarePorIdQuery(fechamentoTurma.PeriodoEscolarId.Value));
            }

            await GravarFechamentoTurma(fechamentoTurma, fechamentoTurmaDisciplina, turma, periodoEscolar);

            return await GravarConselhoClasse(fechamentoTurma, conselhoClasseId, alunoCodigo, turma, conselhoClasseNotaDto, periodoEscolar?.Bimestre);
        }

        private async Task<ConselhoClasseNotaRetornoDto> GravarConselhoClasse(FechamentoTurma fechamentoTurma, long conselhoClasseId, string alunoCodigo, Turma turma, ConselhoClasseNotaDto conselhoClasseNotaDto, int? bimestre)
        {
            var conselhoClasseNota = new ConselhoClasseNota();
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            ConselhoClasseNotaRetornoDto conselhoClasseNotaRetorno = null;
            conselhoClasseNotaRetorno = conselhoClasseId == 0 ?
                await InserirConselhoClasseNota(fechamentoTurma, alunoCodigo, turma, conselhoClasseNotaDto, bimestre, usuarioLogado) :
                await AlterarConselhoClasse(conselhoClasseId, fechamentoTurma.Id, alunoCodigo, turma, conselhoClasseNotaDto, bimestre, usuarioLogado) ;

            // TODO Verificar se o fechamentoTurma.Turma carregou UE
            if (await VerificaNotasTodosComponentesCurriculares(alunoCodigo, fechamentoTurma.Turma, fechamentoTurma.PeriodoEscolarId))
                await VerificaRecomendacoesAluno(conselhoClasseNotaRetorno.ConselhoClasseAlunoId);

            await mediator.Send(new PublicaFilaAtualizacaoSituacaoConselhoClasseCommand(conselhoClasseNotaRetorno.ConselhoClasseId, usuarioLogado));

            return conselhoClasseNotaRetorno;
        }

        private async Task<ConselhoClasseNotaRetornoDto> AlterarConselhoClasse(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, Turma turma, ConselhoClasseNotaDto conselhoClasseNotaDto, int? bimestre, Usuario usuarioLogado)
        {
            var conselhoClasseAluno = await repositorioConselhoClasseAluno.ObterPorConselhoClasseAlunoCodigoAsync(conselhoClasseId, alunoCodigo);
            AuditoriaDto auditoria = null;
            long conselhoClasseAlunoId = 0;
            bool enviarAprovacao = false;

            unitOfWork.IniciarTransacao();
            try
            {
                conselhoClasseAlunoId = conselhoClasseAluno != null ?
                    conselhoClasseAluno.Id : 
                    await SalvarConselhoClasseAlunoResumido(conselhoClasseId, alunoCodigo);

                await mediator.Send(new InserirTurmasComplementaresCommand(turma.Id, conselhoClasseAlunoId, alunoCodigo));

                var conselhoClasseNota = await repositorioConselhoClasseNota.ObterPorConselhoClasseAlunoComponenteCurricularAsync(conselhoClasseAlunoId, conselhoClasseNotaDto.CodigoComponenteCurricular);

                double? notaAnterior = null;
                long? conceitoIdAnterior = null;

                if (conselhoClasseNota == null)
                {
                    conselhoClasseNota = ObterConselhoClasseNota(conselhoClasseNotaDto, conselhoClasseAlunoId);
                }
                else
                {
                    notaAnterior = conselhoClasseNota.Nota;
                    conceitoIdAnterior = conselhoClasseNota.ConceitoId;

                    conselhoClasseNota.Justificativa = conselhoClasseNotaDto.Justificativa;
                    if (conselhoClasseNotaDto.Nota.HasValue)
                    {
                        // Gera histórico de alteração
                        if (conselhoClasseNota.Nota != null && conselhoClasseNota.Nota != conselhoClasseNotaDto.Nota.Value)
                            await mediator.Send(new SalvarHistoricoNotaConselhoClasseCommand(conselhoClasseNota.Id, conselhoClasseNota.Nota.Value, conselhoClasseNotaDto.Nota.Value));

                        conselhoClasseNota.Nota = conselhoClasseNotaDto.Nota.Value;
                    }
                    else conselhoClasseNota.Nota = null;

                    if (conselhoClasseNotaDto.Conceito.HasValue)
                    {
                        // Gera histórico de alteração
                        if (conselhoClasseNota.ConceitoId != conselhoClasseNotaDto.Conceito.Value)
                            await mediator.Send(new SalvarHistoricoConceitoConselhoClasseCommand(conselhoClasseNota.Id, conselhoClasseNota.ConceitoId, conselhoClasseNotaDto.Conceito.Value));

                        conselhoClasseNota.ConceitoId = conselhoClasseNotaDto.Conceito.Value;
                    }
                }

                if (turma.AnoLetivo == 2020)
                    ValidarNotasFechamentoConselhoClasse2020(conselhoClasseNota);

                enviarAprovacao = await EnviarParaAprovacao(turma, usuarioLogado);
                if (enviarAprovacao)
                    await GerarWFAprovacao(conselhoClasseNota, turma, bimestre, usuarioLogado, alunoCodigo, notaAnterior, conceitoIdAnterior);
                else
                    await repositorioConselhoClasseNota.SalvarAsync(conselhoClasseNota);
                unitOfWork.PersistirTransacao();

                auditoria = (AuditoriaDto)conselhoClasseNota;
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw e;
            }

            var consolidacaoTurma = new ConsolidacaoTurmaDto(turma.Id, bimestre ?? 0);
            var mensagemParaPublicar = JsonConvert.SerializeObject(consolidacaoTurma);

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarTurmaConselhoClasseSync, mensagemParaPublicar, Guid.NewGuid(), null));

            var conselhoClasseNotaRetorno = new ConselhoClasseNotaRetornoDto()
            {
                ConselhoClasseId = conselhoClasseId,
                FechamentoTurmaId = fechamentoTurmaId,
                Auditoria = auditoria,
                ConselhoClasseAlunoId = conselhoClasseAlunoId,
                EmAprovacao = enviarAprovacao
            };

            return conselhoClasseNotaRetorno;
        }

        private async Task<ConselhoClasseNotaRetornoDto> InserirConselhoClasseNota(FechamentoTurma fechamentoTurma, string alunoCodigo, Turma turma, ConselhoClasseNotaDto conselhoClasseNotaDto, int? bimestre, Usuario usuarioLogado)
        {
            AuditoriaDto auditoria = null;
            long conselhoClasseId = 0;
            var conselhoClasse = new ConselhoClasse();
            long conselhoClasseAlunoId = 0;
            conselhoClasse.FechamentoTurmaId = fechamentoTurma.Id;
            bool enviarAprovacao = false;

            unitOfWork.IniciarTransacao();
            try
            {
                await repositorioConselhoClasse.SalvarAsync(conselhoClasse);

                conselhoClasseId = conselhoClasse.Id;

                conselhoClasseAlunoId = await SalvarConselhoClasseAlunoResumido(conselhoClasse.Id, alunoCodigo);

                await mediator.Send(new InserirTurmasComplementaresCommand(turma.Id, conselhoClasseAlunoId, alunoCodigo));

                var conselhoClasseNota = ObterConselhoClasseNota(conselhoClasseNotaDto, conselhoClasseAlunoId);

                if (fechamentoTurma.Turma.AnoLetivo == 2020)
                    ValidarNotasFechamentoConselhoClasse2020(conselhoClasseNota);
                enviarAprovacao = await EnviarParaAprovacao(fechamentoTurma.Turma, usuarioLogado);
                if (enviarAprovacao)
                    await GerarWFAprovacao(conselhoClasseNota, turma, bimestre, usuarioLogado, alunoCodigo, null, null);
                else
                    await repositorioConselhoClasseNota.SalvarAsync(conselhoClasseNota);
                unitOfWork.PersistirTransacao();

                auditoria = (AuditoriaDto)conselhoClasseNota;
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw e;
            }

            var conselhoClasseNotaRetorno = new ConselhoClasseNotaRetornoDto()
            {
                ConselhoClasseId = conselhoClasseId,
                FechamentoTurmaId = fechamentoTurma.Id,
                Auditoria = auditoria,
                ConselhoClasseAlunoId = conselhoClasseAlunoId,
                EmAprovacao = enviarAprovacao
            };
            return conselhoClasseNotaRetorno;
        }

        private async Task<bool> EnviarParaAprovacao(Turma turma, Usuario usuarioLogado)
        {
            return turma.AnoLetivo < DateTime.Today.Year
                && !usuarioLogado.EhGestorEscolar()
                && await ParametroAprovacaoAtivo(turma.AnoLetivo);
        }

        private async Task<bool> ParametroAprovacaoAtivo(int anoLetivo)
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.AprovacaoAlteracaoNotaConselho, anoLetivo));
            if (parametro == null)
                throw new NegocioException($"Não foi possível localizar o parametro 'AprovacaoAlteracaoNotaConselho' para o ano {anoLetivo}");

            return parametro.Ativo;
        }

        private async Task GerarWFAprovacao(ConselhoClasseNota conselhoClasseNota, Turma turma, int? bimestre, Usuario usuarioLogado, string alunoCodigo, double? notaAnterior, long? conceitoIdAnterior)
        {
            double? notaAtual = conselhoClasseNota.Nota;
            long? conceitoIdAtual = conselhoClasseNota.ConceitoId;

            if(conselhoClasseNota.Id == 0)
            {
                conselhoClasseNota.Nota = null;
                conselhoClasseNota.ConceitoId = null;

                await repositorioConselhoClasseNota.SalvarAsync(conselhoClasseNota);
            }

            await mediator.Send(new GerarWFAprovacaoNotaConselhoClasseCommand(conselhoClasseNota.Id,
                                                                              conselhoClasseNota.ComponenteCurricularCodigo,
                                                                              notaAtual,
                                                                              conceitoIdAtual,
                                                                              turma,
                                                                              bimestre,
                                                                              usuarioLogado,
                                                                              alunoCodigo,
                                                                              notaAnterior,
                                                                              conceitoIdAnterior));
        }

        private async Task GravarFechamentoTurma(FechamentoTurma fechamentoTurma, FechamentoTurmaDisciplina fechamentoTurmaDisciplina, Turma turma, PeriodoEscolar periodoEscolar)
        {
            if (fechamentoTurma.PeriodoEscolarId.HasValue)
            {
                // Fechamento Bimestral
                if (!await consultasPeriodoFechamento.TurmaEmPeriodoDeFechamento(fechamentoTurma.Turma, DateTime.Today, fechamentoTurma.PeriodoEscolar.Bimestre))
                    throw new NegocioException($"Turma {fechamentoTurma.Turma.Nome} não esta em período de fechamento para o {fechamentoTurma.PeriodoEscolar.Bimestre}º Bimestre!");
            }
            else
            {
                // Fechamento Final
                if (fechamentoTurma.Turma.AnoLetivo != 2020 && !fechamentoTurma.Turma.Historica)
                {
                    var validacaoConselhoFinal = await consultasConselhoClasse.ValidaConselhoClasseUltimoBimestre(fechamentoTurma.Turma);
                    if (!validacaoConselhoFinal.Item2 && fechamentoTurma.Turma.AnoLetivo == DateTime.Today.Year)
                        throw new NegocioException($"Para salvar a nota final você precisa registrar o conselho de classe do {validacaoConselhoFinal.Item1}º bimestre");
                }
            }
            var consolidacaoTurma = new ConsolidacaoTurmaDto(turma.Id, fechamentoTurma.PeriodoEscolarId != null ? periodoEscolar.Bimestre : 0);
            var mensagemParaPublicar = JsonConvert.SerializeObject(consolidacaoTurma);

            try
            {
                unitOfWork.IniciarTransacao();
                if (fechamentoTurmaDisciplina.DisciplinaId > 0)
                {
                    await repositorioFechamentoTurma.SalvarAsync(fechamentoTurma);
                    fechamentoTurmaDisciplina.FechamentoTurmaId = fechamentoTurma.Id;
                    await repositorioFechamentoTurmaDisciplina.SalvarAsync(fechamentoTurmaDisciplina);
                }
                unitOfWork.PersistirTransacao();

                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarTurmaFechamentoSync, mensagemParaPublicar, Guid.NewGuid(), null));
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw e;
            }
        }

        private async Task VerificaRecomendacoesAluno(long conselhoClasseAlunoId)
        {
            var conselhoClasseAluno = await repositorioConselhoClasseAluno.ObterPorIdAsync(conselhoClasseAlunoId);

            if (string.IsNullOrEmpty(conselhoClasseAluno.RecomendacoesAluno) || string.IsNullOrEmpty(conselhoClasseAluno.RecomendacoesFamilia))
            {
                var recomendacoes = await mediator.Send(new ObterTextoRecomendacoesAlunoFamiliaQuery());

                conselhoClasseAluno.RecomendacoesAluno = string.IsNullOrEmpty(conselhoClasseAluno.RecomendacoesAluno) ? recomendacoes.recomendacoesAluno : conselhoClasseAluno.RecomendacoesAluno;
                conselhoClasseAluno.RecomendacoesFamilia = string.IsNullOrEmpty(conselhoClasseAluno.RecomendacoesFamilia) ? recomendacoes.recomendacoesFamilia : conselhoClasseAluno.RecomendacoesFamilia;

                await repositorioConselhoClasseAluno.SalvarAsync(conselhoClasseAluno);
            }
        }

        private async Task<long> SalvarConselhoClasseAlunoResumido(long conselhoClasseId, string alunoCodigo)
        {
            var conselhoClasseAluno = new ConselhoClasseAluno()
            {
                AlunoCodigo = alunoCodigo,
                ConselhoClasseId = conselhoClasseId
            };

            return await repositorioConselhoClasseAluno.SalvarAsync(conselhoClasseAluno);
        }

        private ConselhoClasseNota ObterConselhoClasseNota(ConselhoClasseNotaDto conselhoClasseNotaDto, long conselhoClasseAlunoId)
        {
            var conselhoClasseNota = new ConselhoClasseNota()
            {
                ConselhoClasseAlunoId = conselhoClasseAlunoId,
                ComponenteCurricularCodigo = conselhoClasseNotaDto.CodigoComponenteCurricular,
                Justificativa = conselhoClasseNotaDto.Justificativa,
            };
            if (conselhoClasseNotaDto.Nota.HasValue)
                conselhoClasseNota.Nota = conselhoClasseNotaDto.Nota.Value;
            if (conselhoClasseNotaDto.Conceito.HasValue)
                conselhoClasseNota.ConceitoId = conselhoClasseNotaDto.Conceito.Value;
            return conselhoClasseNota;
        }

        public async Task<AuditoriaDto> GerarConselhoClasse(ConselhoClasse conselhoClasse, FechamentoTurma fechamentoTurma)
        {
            var conselhoClasseExistente = await repositorioConselhoClasse
                .ObterPorTurmaEPeriodoAsync(fechamentoTurma.TurmaId, fechamentoTurma.PeriodoEscolarId);

            if (conselhoClasseExistente != null)
                throw new NegocioException($"Já existe um conselho de classe gerado para a turma {fechamentoTurma.Turma.Nome}!");

            if (fechamentoTurma.PeriodoEscolarId.HasValue)
            {
                // Fechamento Bimestral
                if (!await consultasPeriodoFechamento.TurmaEmPeriodoDeFechamento(fechamentoTurma.Turma, DateTime.Today, fechamentoTurma.PeriodoEscolar.Bimestre))
                    throw new NegocioException($"Turma {fechamentoTurma.Turma.Nome} não esta em período de fechamento para o {fechamentoTurma.PeriodoEscolar.Bimestre}º Bimestre!");
            }
            else
            {
                // Fechamento Final
                if (fechamentoTurma.Turma.AnoLetivo != 2020)
                {
                    var validacaoConselhoFinal = await consultasConselhoClasse.ValidaConselhoClasseUltimoBimestre(fechamentoTurma.Turma);
                    if (!validacaoConselhoFinal.Item2)
                        throw new NegocioException($"Para acessar este aba você precisa registrar o conselho de classe do {validacaoConselhoFinal.Item1}º bimestre");
                }
            }

            await repositorioConselhoClasse.SalvarAsync(conselhoClasse);
            return (AuditoriaDto)conselhoClasse;
        }

        public async Task<ConselhoClasseAluno> SalvarConselhoClasseAluno(ConselhoClasseAluno conselhoClasseAluno)
        {
            var fechamentoTurma = await mediator
                .Send(new ObterFechamentoTurmaPorIdAlunoCodigoQuery(conselhoClasseAluno.ConselhoClasse.FechamentoTurmaId, conselhoClasseAluno.AlunoCodigo));

            if (!await VerificaNotasTodosComponentesCurriculares(conselhoClasseAluno.AlunoCodigo, fechamentoTurma.Turma, fechamentoTurma.PeriodoEscolarId))
                throw new NegocioException("É necessário que todos os componentes tenham nota/conceito informados!");

            // Se não existir conselho de classe para o fechamento gera
            if (conselhoClasseAluno.ConselhoClasse.Id == 0)
            {
                await GerarConselhoClasse(conselhoClasseAluno.ConselhoClasse, fechamentoTurma);
                conselhoClasseAluno.ConselhoClasseId = conselhoClasseAluno.ConselhoClasse.Id;
            }
            else
                await repositorioConselhoClasse.SalvarAsync(conselhoClasseAluno.ConselhoClasse);

            var conselhoClasseAlunoId = await repositorioConselhoClasseAluno.SalvarAsync(conselhoClasseAluno);

            await mediator.Send(new InserirTurmasComplementaresCommand(fechamentoTurma.TurmaId, conselhoClasseAlunoId, conselhoClasseAluno.AlunoCodigo));

            return conselhoClasseAluno;
        }

        public async Task<bool> VerificaNotasTodosComponentesCurriculares(string alunoCodigo, Turma turma, long? periodoEscolarId)
        {
            int bimestre;
            long[] conselhosClassesIds;
            string[] turmasCodigos;

            if (turma.DeveVerificarRegraRegulares())
            {
                turmasCodigos = await mediator
                    .Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(turma.AnoLetivo, alunoCodigo, turma.ObterTiposRegularesDiferentes()));

                turmasCodigos = turmasCodigos
                    .Concat(new string[] { turma.CodigoTurma }).ToArray();
            }
            else turmasCodigos = new string[] { turma.CodigoTurma };


            if (periodoEscolarId.HasValue)
            {
                var periodoEscolar = await mediator
                    .Send(new ObterPeriodoEscolarePorIdQuery(periodoEscolarId.Value));

                if (periodoEscolar == null)
                    throw new NegocioException("Não foi possível localizar o período escolar");

                bimestre = periodoEscolar.Bimestre;

                conselhosClassesIds = await mediator
                    .Send(new ObterConselhoClasseIdsPorTurmaEPeriodoQuery(turmasCodigos, periodoEscolar?.Id));
            }
            else
            {
                bimestre = 0;
                conselhosClassesIds = new long[0];
            }

            var notasParaVerificar = new List<NotaConceitoBimestreComponenteDto>();
            if (conselhosClassesIds != null)
            {
                foreach (var conselhosClassesId in conselhosClassesIds)
                {
                    var notasParaAdicionar = await mediator.Send(new ObterConselhoClasseNotasAlunoQuery(conselhosClassesId, alunoCodigo));

                    notasParaVerificar.AddRange(notasParaAdicionar);
                }
            }

            if (periodoEscolarId.HasValue)
                notasParaVerificar.AddRange(await mediator.Send(new ObterNotasFechamentosPorTurmasCodigosBimestreQuery(turmasCodigos, alunoCodigo, bimestre)));
            else
            {
                var todasAsNotas = await mediator.Send(new ObterNotasFinaisBimestresAlunoQuery(turmasCodigos, alunoCodigo));

                if (todasAsNotas != null && todasAsNotas.Any())
                    notasParaVerificar.AddRange(todasAsNotas.Where(a => a.Bimestre == null));
            }

            var componentesCurriculares = await ObterComponentesTurmas(turmasCodigos, turma.EnsinoEspecial, turma.TurnoParaComponentesCurriculares);
            var disciplinasDaTurma = await mediator
                .Send(new ObterComponentesCurricularesPorIdsQuery(componentesCurriculares.Select(x => x.CodigoComponenteCurricular).Distinct().ToArray()));

            // Checa se todas as disciplinas da turma receberam nota
            var disciplinasLancamNota = disciplinasDaTurma.Where(c => c.LancaNota && c.GrupoMatrizNome != null);
            foreach (var componenteCurricular in disciplinasLancamNota)
            {
                if (!notasParaVerificar.Any(c => c.ComponenteCurricularCodigo == componenteCurricular.CodigoComponenteCurricular))
                    return false;
            }

            return true;
        }

        private async Task<IEnumerable<DisciplinaDto>> ObterComponentesTurmas(string[] turmasCodigo, bool ehEnsinoEspecial, int turnoParaComponentesCurriculares)
        {
            var componentesTurma = new List<DisciplinaDto>();
            Usuario usuarioAtual = await mediator.Send(new ObterUsuarioLogadoQuery());

            var componentesCurriculares = await mediator.Send(new ObterComponentesCurricularesPorTurmasCodigoQuery(turmasCodigo, usuarioAtual.PerfilAtual, usuarioAtual.Login, ehEnsinoEspecial, turnoParaComponentesCurriculares));
            if (componentesCurriculares != null && componentesCurriculares.Any())
                componentesTurma.AddRange(componentesCurriculares);
            else throw new NegocioException("Não localizado disciplinas para a turma no EOL!");

            return componentesTurma;
        }

        public async Task<ParecerConclusivoDto> GerarParecerConclusivoAlunoAsync(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo)
        {
            var solicitanteId = await mediator.Send(new ObterUsuarioLogadoIdQuery());
            var conselhoClasseAluno = await ObterConselhoClasseAluno(conselhoClasseId, fechamentoTurmaId, alunoCodigo);

            return await mediator.Send(new GerarParecerConclusivoAlunoCommand(conselhoClasseAluno, solicitanteId));
        }

        private async Task<ConselhoClasseAluno> ObterConselhoClasseAluno(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo)
        {
            ConselhoClasseAluno conselhoClasseAluno = await repositorioConselhoClasseAluno.ObterPorConselhoClasseAlunoCodigoAsync(conselhoClasseId, alunoCodigo);
            if (conselhoClasseAluno == null)
            {
                ConselhoClasse conselhoClasse = null;
                if (conselhoClasseId == 0)
                {
                    conselhoClasse = new ConselhoClasse() { FechamentoTurmaId = fechamentoTurmaId };
                    await repositorioConselhoClasse.SalvarAsync(conselhoClasse);
                }
                else
                    conselhoClasse = repositorioConselhoClasse.ObterPorId(conselhoClasseId);

                conselhoClasseAluno = new ConselhoClasseAluno() { AlunoCodigo = alunoCodigo, ConselhoClasse = conselhoClasse, ConselhoClasseId = conselhoClasse.Id };
                await repositorioConselhoClasseAluno.SalvarAsync(conselhoClasseAluno);
            }
            conselhoClasseAluno.ConselhoClasse.FechamentoTurma = await ObterFechamentoTurma(fechamentoTurmaId, alunoCodigo);

            return conselhoClasseAluno;
        }

        private async Task<FechamentoTurma> ObterFechamentoTurma(long fechamentoTurmaId, string alunoCodigo)
        {
            return await mediator.Send(new ObterFechamentoTurmaPorIdAlunoCodigoQuery(fechamentoTurmaId, alunoCodigo));
        }

        private void ValidarNotasFechamentoConselhoClasse2020(ConselhoClasseNota conselhoClasseNota)
        {
            if (conselhoClasseNota.ConceitoId.HasValue && conselhoClasseNota.ConceitoId.Value == 3)
                throw new NegocioException("Não é possível atribuir conceito NS (Não Satisfatório) pois em 2020 não há retenção dos estudantes conforme o Art 5º da LEI Nº 17.437 DE 12 DE AGOSTO DE 2020.");
            else if (conselhoClasseNota.Nota < 5)
                throw new NegocioException("Não é possível atribuir uma nota menor que 5 pois em 2020 não há retenção dos estudantes conforme o Art 5º da LEI Nº 17.437 DE 12 DE AGOSTO DE 2020.");
        }

        public async Task<RetornoConsolidado> ConsolidaConselhoClasse(int dreId)
        {
            int contador = 0;
            int erros = 0;
            var retorno = new Dictionary<string, int>();

            var listaConselhoAlunoReprocessar = await repositorioConselhoClasse.ObterAlunosReprocessamentoConsolidacaoConselho(dreId);

            var listaErros = new List<objConsolidacaoConselhoAluno>();
            foreach (var conselhoAluno in listaConselhoAlunoReprocessar)
            {
                try
                {
                    SituacaoConselhoClasse statusNovo = SituacaoConselhoClasse.NaoIniciado;

                    var consolidadoTurmaAluno = repositorioConselhoClasseConsolidado.ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoAsync(conselhoAluno.turmaId, conselhoAluno.bimestre, conselhoAluno.alunoCodigo).Result;

                    if (consolidadoTurmaAluno == null)
                    {
                        consolidadoTurmaAluno = new ConselhoClasseConsolidadoTurmaAluno();
                        consolidadoTurmaAluno.AlunoCodigo = conselhoAluno.alunoCodigo;
                        consolidadoTurmaAluno.Bimestre = conselhoAluno.bimestre;
                        consolidadoTurmaAluno.TurmaId = conselhoAluno.turmaId;
                        consolidadoTurmaAluno.Status = statusNovo;
                    }

                    var componentesDoAluno = mediator.Send(new ObterComponentesParaFechamentoAcompanhamentoCCAlunoQuery(conselhoAluno.alunoCodigo, conselhoAluno.bimestre, conselhoAluno.turmaId)).Result;
                    if (componentesDoAluno != null && componentesDoAluno.Any())
                    {
                        var turma = await mediator.Send(new ObterTurmaPorIdQuery(conselhoAluno.turmaId));

                        if (conselhoAluno.bimestre == 0)
                        {
                            var fechamento = await mediator.Send(new ObterFechamentoPorTurmaPeriodoQuery() { TurmaId = conselhoAluno.turmaId });
                            var conselhoClasse = await mediator.Send(new ObterConselhoClassePorFechamentoIdQuery(fechamento.Id));
                            var conselhoClasseAluno = await mediator.Send(new ObterConselhoClasseAlunoPorAlunoCodigoConselhoIdQuery(conselhoClasse.Id, conselhoAluno.alunoCodigo));
                            consolidadoTurmaAluno.ParecerConclusivoId = conselhoClasseAluno != null ? conselhoClasseAluno.ConselhoClasseParecerId : null;
                        }


                        var turmasCodigos = new string[] { };
                        if (turma.DeveVerificarRegraRegulares())
                        {
                            List<TipoTurma> turmasCodigosParaConsulta = new List<TipoTurma>() { turma.TipoTurma };
                            turmasCodigosParaConsulta.AddRange(turma.ObterTiposRegularesDiferentes());
                            turmasCodigos = await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(turma.AnoLetivo, conselhoAluno.alunoCodigo, turmasCodigosParaConsulta));
                        }
                        if (turmasCodigos.Length == 0)
                        {
                            turmasCodigos = new string[1] { turma.CodigoTurma };
                        }

                        var componentesComNotaFechamentoOuConselho = await mediator.Send(new ObterComponentesComNotaDeFechamentoOuConselhoQuery(turma.AnoLetivo, conselhoAluno.turmaId, conselhoAluno.bimestre, conselhoAluno.alunoCodigo));
                        var componentesDaTurmaEol = await mediator.Send(new ObterComponentesCurricularesEOLPorTurmasCodigoQuery(turmasCodigos));

                        var possuiComponentesSemNotaConceito = componentesDaTurmaEol.Where(x => x.LancaNota == true).Select(x => x.Codigo).ToArray().Except(componentesComNotaFechamentoOuConselho.Select(x => x.Codigo).ToArray()).Any();

                        if (possuiComponentesSemNotaConceito)
                            statusNovo = SituacaoConselhoClasse.EmAndamento;
                        else
                            statusNovo = SituacaoConselhoClasse.Concluido;
                    }

                    if (consolidadoTurmaAluno.ParecerConclusivoId != null)
                        statusNovo = SituacaoConselhoClasse.Concluido;

                    consolidadoTurmaAluno.Status = statusNovo;

                    consolidadoTurmaAluno.DataAtualizacao = DateTime.Now;

                    var rr = repositorioConselhoClasseConsolidado.SalvarAsync(consolidadoTurmaAluno).Result;
                    contador = contador + 1;
                }

                catch (Exception ex)
                {
                    var objErro = new objConsolidacaoConselhoAluno
                    {
                        alunoCodigo = conselhoAluno.alunoCodigo,
                        bimestre = conselhoAluno.bimestre,
                        turmaId = conselhoAluno.turmaId,
                        erro = ex.Message
                    };
                    listaErros.Add(objErro);
                    erros = erros + 1;
                }
            }
            retorno.Add("TotalAlterados", contador);
            retorno.Add("TotalErros", erros);
            var objRetorno = new RetornoConsolidado
            {
                listaObjErros = listaErros,
                TotaldeAlterados = retorno
            };
            return objRetorno;
        }

    }
}