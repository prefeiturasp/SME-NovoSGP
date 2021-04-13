﻿using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
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
        private readonly IRepositorioConselhoClasseParecerConclusivo repositorioParecer;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IRepositorioConselhoClasseNota repositorioConselhoClasseNota;
        private readonly IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina;
        private readonly IRepositorioUe repositorioUe;
        private readonly IRepositorioDre repositorioDre;
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private readonly IConsultasConselhoClasse consultasConselhoClasse;
        private readonly IConsultasDisciplina consultasDisciplina;
        private readonly IUnitOfWork unitOfWork;
        private readonly IServicoCalculoParecerConclusivo servicoCalculoParecerConclusivo;
        private readonly IMediator mediator;
        private readonly IConsultasConselhoClasseNota consultasConselhoClasseNota;



        public ServicoConselhoClasse(IRepositorioConselhoClasse repositorioConselhoClasse,
                                     IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno,
                                     IRepositorioFechamentoTurma repositorioFechamentoTurma,
                                     IRepositorioConselhoClasseParecerConclusivo repositorioParecer,
                                     IRepositorioTipoCalendario repositorioTipoCalendario,
                                     IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina,
                                     IRepositorioUe repositorioUe,
                                     IRepositorioDre repositorioDre,
                                     IConsultasPeriodoEscolar consultasPeriodoEscolar,
                                     IConsultasPeriodoFechamento consultasPeriodoFechamento,
                                     IConsultasConselhoClasse consultasConselhoClasse,
                                     IConsultasDisciplina consultasDisciplina,
                                     IRepositorioConselhoClasseNota repositorioConselhoClasseNota,
                                     IUnitOfWork unitOfWork,
                                     IServicoCalculoParecerConclusivo servicoCalculoParecerConclusivo,
                                     IMediator mediator,
                                     IConsultasConselhoClasseNota consultasConselhoClasseNota)

        {
            this.repositorioConselhoClasse = repositorioConselhoClasse ?? throw new ArgumentNullException(nameof(repositorioConselhoClasse));
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
            this.repositorioParecer = repositorioParecer ?? throw new ArgumentNullException(nameof(repositorioParecer));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
            this.repositorioConselhoClasseNota = repositorioConselhoClasseNota ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseNota));
            this.consultasConselhoClasse = consultasConselhoClasse ?? throw new ArgumentNullException(nameof(consultasConselhoClasse));
            this.consultasDisciplina = consultasDisciplina ?? throw new ArgumentNullException(nameof(consultasDisciplina));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.servicoCalculoParecerConclusivo = servicoCalculoParecerConclusivo ?? throw new ArgumentNullException(nameof(servicoCalculoParecerConclusivo));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.consultasConselhoClasseNota = consultasConselhoClasseNota ?? throw new ArgumentNullException(nameof(consultasConselhoClasseNota));
        }

        public async Task<ConselhoClasseNotaRetornoDto> SalvarConselhoClasseAlunoNotaAsync(ConselhoClasseNotaDto conselhoClasseNotaDto, string alunoCodigo, long conselhoClasseId, long fechamentoTurmaId, string codigoTurma, int bimestre)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(codigoTurma));
            if (turma == null) throw new NegocioException("Turma não encontrada");

            var ehAnoAnterior = turma.AnoLetivo != DateTime.Now.Year;
            var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaPorIdAlunoCodigoQuery(fechamentoTurmaId, alunoCodigo, ehAnoAnterior));
            var fechamentoTurmaDisciplina = new FechamentoTurmaDisciplina();

            if (fechamentoTurma == null)
            {
                if (!ehAnoAnterior) throw new NegocioException("Não existe fechamento de turma para o conselho de classe");

                var ue = repositorioUe.ObterPorId(turma.UeId);
                ue.AdicionarDre(repositorioDre.ObterPorId(ue.DreId));
                turma.AdicionarUe(ue);

                var periodoEscolar = await mediator.Send(new ObterPeriodoEscolarPorTurmaBimestreQuery(turma, bimestre));
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



            try
            {
                unitOfWork.IniciarTransacao();
                if (fechamentoTurmaDisciplina.DisciplinaId > 0)
                {
                    await repositorioFechamentoTurma.SalvarAsync(fechamentoTurma);
                    fechamentoTurmaDisciplina.FechamentoTurmaId = fechamentoTurma.Id;
                    await repositorioFechamentoTurmaDisciplina.SalvarAsync(fechamentoTurmaDisciplina);
                }
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
                        if (!validacaoConselhoFinal.Item2 && fechamentoTurma.Turma.AnoLetivo == DateTime.Today.Year)
                            throw new NegocioException($"Para acessar este aba você precisa registrar o conselho de classe do {validacaoConselhoFinal.Item1}º bimestre");
                    }
                }
                unitOfWork.PersistirTransacao();
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw e;
            }

            long conselhoClasseAlunoId = 0;
            var conselhoClasseNota = new ConselhoClasseNota();

            try
            {
                if (conselhoClasseId == 0)
                {

                    var conselhoClasse = new ConselhoClasse();
                    conselhoClasse.FechamentoTurmaId = fechamentoTurma.Id;

                    unitOfWork.IniciarTransacao();
                    await repositorioConselhoClasse.SalvarAsync(conselhoClasse);

                    conselhoClasseId = conselhoClasse.Id;

                    conselhoClasseAlunoId = await SalvarConselhoClasseAlunoResumido(conselhoClasse.Id, alunoCodigo);

                    await mediator.Send(new InserirTurmasComplementaresCommand(turma.Id, conselhoClasseAlunoId, alunoCodigo));

                    conselhoClasseNota = ObterConselhoClasseNota(conselhoClasseNotaDto, conselhoClasseAlunoId);

                    if (fechamentoTurma.Turma.AnoLetivo == 2020)
                        ValidarNotasFechamentoConselhoClasse2020(conselhoClasseNota);

                    await repositorioConselhoClasseNota.SalvarAsync(conselhoClasseNota);
                    unitOfWork.PersistirTransacao();
                }
                else
                {
                    var conselhoClasseAluno = await repositorioConselhoClasseAluno.ObterPorConselhoClasseAlunoCodigoAsync(conselhoClasseId, alunoCodigo);
                    unitOfWork.IniciarTransacao();

                    conselhoClasseAlunoId = conselhoClasseAluno != null ? conselhoClasseAluno.Id : await SalvarConselhoClasseAlunoResumido(conselhoClasseId, alunoCodigo);

                    await mediator.Send(new InserirTurmasComplementaresCommand(turma.Id, conselhoClasseAlunoId, alunoCodigo));

                    conselhoClasseNota = await repositorioConselhoClasseNota.ObterPorConselhoClasseAlunoComponenteCurricularAsync(conselhoClasseAlunoId, conselhoClasseNotaDto.CodigoComponenteCurricular);

                    if (conselhoClasseNota == null)
                    {
                        conselhoClasseNota = ObterConselhoClasseNota(conselhoClasseNotaDto, conselhoClasseAlunoId);
                    }
                    else
                    {
                        conselhoClasseNota.Justificativa = conselhoClasseNotaDto.Justificativa;
                        if (conselhoClasseNotaDto.Nota.HasValue)
                        {
                            // Gera histórico de alteração
                            if (conselhoClasseNota.Nota != conselhoClasseNotaDto.Nota.Value)
                                await mediator.Send(new SalvarHistoricoNotaConselhoClasseCommand(conselhoClasseNota.Id, conselhoClasseNota.Nota.Value, conselhoClasseNotaDto.Nota.Value));

                            conselhoClasseNota.Nota = conselhoClasseNotaDto.Nota.Value;
                        }
                        else conselhoClasseNota.Nota = null;

                        if (conselhoClasseNotaDto.Conceito.HasValue)
                        {
                            // Gera histórico de alteração
                            if (conselhoClasseNota.ConceitoId != conselhoClasseNotaDto.Conceito.Value)
                                await mediator.Send(new SalvarHistoricoConceitoConselhoClasseCommand(conselhoClasseNota.Id, conselhoClasseNota.ConceitoId.Value, conselhoClasseNotaDto.Conceito.Value));

                            conselhoClasseNota.ConceitoId = conselhoClasseNotaDto.Conceito.Value;
                        }
                    }

                    if (fechamentoTurma.Turma.AnoLetivo == 2020)
                        ValidarNotasFechamentoConselhoClasse2020(conselhoClasseNota);

                    await repositorioConselhoClasseNota.SalvarAsync(conselhoClasseNota);

                    unitOfWork.PersistirTransacao();
                }
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw e;
            }

            // TODO Verificar se o fechamentoTurma.Turma carregou UE
            if (await VerificaNotasTodosComponentesCurriculares(alunoCodigo, fechamentoTurma.Turma, fechamentoTurma.PeriodoEscolarId))
                await VerificaRecomendacoesAluno(conselhoClasseAlunoId);

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            await mediator.Send(new PublicaFilaAtualizacaoSituacaoConselhoClasseCommand(conselhoClasseId, usuarioLogado));

            var auditoria = (AuditoriaDto)conselhoClasseNota;
            var conselhoClasseNotaRetorno = new ConselhoClasseNotaRetornoDto()
            {
                ConselhoClasseId = conselhoClasseId,
                FechamentoTurmaId = fechamentoTurma.Id,
                Auditoria = auditoria
            };
            return conselhoClasseNotaRetorno;
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
            var conselhoClasseExistente = await repositorioConselhoClasse.ObterPorTurmaEPeriodoAsync(fechamentoTurma.TurmaId, fechamentoTurma.PeriodoEscolarId);
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

        public async Task<AuditoriaConselhoClasseAlunoDto> SalvarConselhoClasseAluno(ConselhoClasseAluno conselhoClasseAluno)
        {
            var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaPorIdAlunoCodigoQuery(conselhoClasseAluno.ConselhoClasse.FechamentoTurmaId, conselhoClasseAluno.AlunoCodigo));

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

            return (AuditoriaConselhoClasseAlunoDto)conselhoClasseAluno;
        }

        public async Task<bool> VerificaNotasTodosComponentesCurriculares(string alunoCodigo, Turma turma, long? periodoEscolarId)
        {
            int bimestre;
            long[] conselhosClassesIds;


            string[] turmasCodigos;
            if (turma.DeveVerificarRegraRegulares())
            {
                turmasCodigos = await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(turma.AnoLetivo, alunoCodigo, turma.ObterTiposRegularesDiferentes()));
                turmasCodigos = turmasCodigos.Concat(new string[] { turma.CodigoTurma }).ToArray();
            }
            else turmasCodigos = new string[] { turma.CodigoTurma };


            if (periodoEscolarId.HasValue)
            {
                var periodoEscolar = await mediator.Send(new ObterPeriodoEscolarePorIdQuery(periodoEscolarId.Value));
                if (periodoEscolar == null)
                    throw new NegocioException("Não foi possível localizar o período escolar");

                bimestre = periodoEscolar.Bimestre;
                conselhosClassesIds = await mediator.Send(new ObterConselhoClasseIdsPorTurmaEPeriodoQuery(turmasCodigos, periodoEscolar?.Id));
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
                    var notasParaAdicionar = await consultasConselhoClasseNota.ObterNotasAlunoAsync(conselhosClassesId, alunoCodigo);
                    notasParaVerificar.AddRange(notasParaAdicionar);
                }
            }

            //var notasAluno = await repositorioConselhoClasseNota.ObterNotasAlunoPorTurmasAsync(alunoCodigo, turmasCodigos, periodoEscolarId);
            if (periodoEscolarId.HasValue)
                notasParaVerificar.AddRange(await mediator.Send(new ObterNotasFechamentosPorTurmasCodigosBimestreQuery(turmasCodigos, alunoCodigo, bimestre)));
            else
            {
                var todasAsNotas = await consultasConselhoClasseNota.ObterNotasFinaisBimestresAlunoAsync(alunoCodigo, turmasCodigos);
                if (todasAsNotas != null && todasAsNotas.Any())
                    notasParaVerificar.AddRange(todasAsNotas.Where(a => a.Bimestre == null));
            }


            var componentesCurriculares = await ObterComponentesTurmas(turmasCodigos, turma.EnsinoEspecial, turma.TurnoParaComponentesCurriculares);

            var disciplinasDaTurma = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(componentesCurriculares.Select(x => x.CodigoComponenteCurricular).Distinct().ToArray()));

            // Checa se todas as disciplinas da turma receberam nota
            foreach (var componenteCurricular in disciplinasDaTurma.Where(c => c.LancaNota && c.GrupoMatrizNome != null))
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
            var conselhoClasseAluno = await ObterConselhoClasseAluno(conselhoClasseId, fechamentoTurmaId, alunoCodigo);
            var turma = conselhoClasseAluno.ConselhoClasse.FechamentoTurma.Turma;

            // Se não possui notas de fechamento nem de conselho retorna um Dto vazio
            if (!await VerificaNotasTodosComponentesCurriculares(alunoCodigo, turma, null))
                return new ParecerConclusivoDto();

            var pareceresDaTurma = await ObterPareceresDaTurma(turma.Id);

            var parecerConclusivo = await servicoCalculoParecerConclusivo.Calcular(alunoCodigo, turma.CodigoTurma, pareceresDaTurma);
            conselhoClasseAluno.ConselhoClasseParecerId = parecerConclusivo.Id;
            await repositorioConselhoClasseAluno.SalvarAsync(conselhoClasseAluno);

            return new ParecerConclusivoDto()
            {
                Id = parecerConclusivo.Id,
                Nome = parecerConclusivo.Nome
            };
        }

        private async Task<IEnumerable<ConselhoClasseParecerConclusivo>> ObterPareceresDaTurma(long turmaId)
        {
            var pareceresConclusivos = await repositorioParecer.ObterListaPorTurmaIdAsync(turmaId, DateTime.Today);
            if (pareceresConclusivos == null || !pareceresConclusivos.Any())
                throw new NegocioException("Não foram encontrados pareceres conclusivos para a turma!");

            return pareceresConclusivos;
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
            else
            if (conselhoClasseNota.Nota < 5)
                throw new NegocioException("Não é possível atribuir uma nota menor que 5 pois em 2020 não há retenção dos estudantes conforme o Art 5º da LEI Nº 17.437 DE 12 DE AGOSTO DE 2020.");

        }
    }
}