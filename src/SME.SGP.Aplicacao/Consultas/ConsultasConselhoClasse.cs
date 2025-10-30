using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao
{
    public class ConsultasConselhoClasse : IConsultasConselhoClasse
    {
        private readonly IRepositorioConselhoClasseConsulta repositorioConselhoClasseConsulta;
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;
        private readonly IRepositorioConselhoClasseAlunoConsulta repositorioConselhoClasseAluno;
        private readonly IRepositorioParametrosSistemaConsulta repositorioParametrosSistema;
        private readonly IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurma;
        private readonly IConsultasTurma consultasTurma;
        private readonly IConsultasPeriodoFechamento consultasPeriodoFechamento;
        private readonly IConsultasFechamentoTurma consultasFechamentoTurma;
        private readonly IRepositorioTipoCalendarioConsulta repositorioTipoCalendario;
        private readonly IMediator mediator;

        public ConsultasConselhoClasse(IRepositorioConselhoClasseConsulta repositorioConselhoClasseConsulta,
                                       IRepositorioConselhoClasseAlunoConsulta repositorioConselhoClasseAluno,
                                       IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar,
                                       IRepositorioParametrosSistemaConsulta repositorioParametrosSistema,
                                       IRepositorioTipoCalendarioConsulta repositorioTipoCalendario,
                                       IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurma,
                                       IConsultasTurma consultasTurma,
                                       IConsultasPeriodoFechamento consultasPeriodoFechamento,
                                       IConsultasFechamentoTurma consultasFechamentoTurma,
                                       IMediator mediator)
        {
            this.repositorioConselhoClasseConsulta = repositorioConselhoClasseConsulta ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseConsulta));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
            this.consultasTurma = consultasTurma ?? throw new ArgumentNullException(nameof(consultasTurma));
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
            this.consultasFechamentoTurma = consultasFechamentoTurma ?? throw new ArgumentNullException(nameof(consultasFechamentoTurma));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ConselhoClasseAlunoResumoDto> ObterConselhoClasseTurma(string turmaCodigo, string alunoCodigo, int bimestre = 0, bool ehFinal = false, bool consideraHistorico = false)
        {
            var turma = await ObterTurma(turmaCodigo);

            var tipoCalendarioTurma = await mediator.Send(new ObterTipoCalendarioIdPorTurmaQuery(turma));
            var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestreTipoCalendarioQuery(bimestre, turma.CodigoTurma, turma.AnoLetivo, turma.Semestre, tipoCalendarioTurma));

            switch (fechamentoTurma)
            {
                case null when !turma.EhAnoAnterior():
                {
                    if (!ehFinal && bimestre > 0)
                        throw new NegocioException(string.Format(MensagemNegocioFechamentoTurma.FECHAMENTO_TURMA_NAO_LOCALIZADO_BIMESTRE, bimestre));
                
                    throw new NegocioException(MensagemNegocioFechamentoTurma.FECHAMENTO_TURMA_NAO_LOCALIZADO);
                }
                case null:
                    throw new NegocioException(MensagemNegocioFechamentoTurma.FECHAMENTO_TURMA_NAO_LOCALIZADO);
            }

            var turmasItinerarioEnsinoMedio = (await mediator.Send(ObterTurmaItinerarioEnsinoMedioQuery.Instance)).ToList();

            if (turma.EhTurmaEdFisicaOuItinerario() || turmasItinerarioEnsinoMedio.Any(a => a.Id == (int)turma.TipoTurma))
            {
                var tiposParaConsulta = new List<int>();
                tiposParaConsulta.AddRange(turma.ObterTiposRegularesDiferentes());

                var turmasRegularesDoAluno = await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(turma.AnoLetivo, alunoCodigo, tiposParaConsulta, semestre: turma.Semestre != 0 ? turma.Semestre : null));
                
                var turmaRegularCodigo = turmasRegularesDoAluno.FirstOrDefault();

                if (turmaRegularCodigo.EhNulo())
                    throw new NegocioException($"Não foi possível obter a turma regular do aluno(a) {alunoCodigo}");

                var turmaRegularId = await mediator.Send(new ObterTurmaIdPorCodigoQuery(turmaRegularCodigo));

                var fechamentoDaTurmaRegular = await mediator.Send(new ObterFechamentoPorTurmaPeriodoQuery
                    { TurmaId = turmaRegularId, PeriodoEscolarId = fechamentoTurma.PeriodoEscolarId ?? 0 });
                
                if (fechamentoDaTurmaRegular.NaoEhNulo())
                    fechamentoTurma = await repositorioFechamentoTurma.ObterCompletoPorIdAsync(fechamentoDaTurmaRegular.Id);

                turma = await ObterTurma(turmaRegularCodigo);
            }

            if (bimestre == 0 && !ehFinal)
            {
                bimestre = await ObterBimestreAtual(turma);

                if (bimestre == 0)
                    bimestre = 1;
            }

            if (bimestre == 0 && !consideraHistorico && !turma.EhAnoAnterior())
            {
                var retornoConselhoBimestre = await mediator.Send(new ObterUltimoBimestreAlunoTurmaQuery(turma, alunoCodigo));
                var alunoPossuiNotasTodosComponentesCurriculares = await mediator.Send(new VerificaNotasTodosComponentesCurricularesQuery(alunoCodigo, turma, retornoConselhoBimestre.bimestre));

                if (!retornoConselhoBimestre.possuiConselho || !alunoPossuiNotasTodosComponentesCurriculares)
                    throw new NegocioException(string.Format(MensagemNegocioConselhoClasse.NAO_PERMITE_ACESSO_ABA_FINAL_SEM_CONCLUIR_CONSELHO_BIMESTRE, retornoConselhoBimestre.bimestre));
            }

            var conselhoClasse = fechamentoTurma.NaoEhNulo() ? await repositorioConselhoClasseConsulta.ObterPorFechamentoId(fechamentoTurma.Id) : null;

            var periodoEscolarId = fechamentoTurma?.PeriodoEscolarId;

            PeriodoEscolar periodoEscolar;

            if (periodoEscolarId.EhNulo())
            {
                var tipoCalendario = await repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(turma.AnoLetivo, turma.ModalidadeTipoCalendario, turma.Semestre);

                if (tipoCalendario.EhNulo()) 
                    throw new NegocioException(MensagemNegocioTipoCalendario.TIPO_CALENDARIO_NAO_ENCONTRADO);

                periodoEscolar = await repositorioPeriodoEscolar.ObterPorTipoCalendarioEBimestreAsync(tipoCalendario.Id, bimestre);
            }
            else
            {
                periodoEscolar = await repositorioPeriodoEscolar.ObterPorIdAsync(periodoEscolarId.Value);
            }

            var bimestreFechamento = !ehFinal ? bimestre : (await ObterPeriodoUltimoBimestre(turma)).Bimestre;

            var periodoFechamentoVigente = await consultasPeriodoFechamento.TurmaEmPeriodoDeFechamentoVigente(turma, DateTimeExtension.HorarioBrasilia().Date, bimestreFechamento);

            var tipoNota = await ObterTipoNota(turma, periodoFechamentoVigente);

            var mediaAprovacao = double.Parse(await repositorioParametrosSistema.ObterValorPorTipoEAno(TipoParametroSistema.MediaBimestre));

            var conselhoClasseAluno = conselhoClasse.NaoEhNulo() ? await repositorioConselhoClasseAluno.ObterPorConselhoClasseAlunoCodigoAsync(conselhoClasse.Id, alunoCodigo) : null;

            bool periodoAberto;

            if (bimestre == 0)
            {
                periodoAberto = await mediator.Send(new ObterTurmaEmPeriodoDeFechamentoQuery(turma, DateTimeExtension.HorarioBrasilia().Date));
            }
            else
                periodoAberto = await mediator.Send(new ObterTurmaEmPeriodoDeFechamentoQuery(turma, DateTimeExtension.HorarioBrasilia().Date, bimestre));

            return new ConselhoClasseAlunoResumoDto()
            {
                FechamentoTurmaId = fechamentoTurma?.Id,
                ConselhoClasseId = conselhoClasse?.Id,
                ConselhoClasseAlunoId = conselhoClasseAluno?.Id,
                Bimestre = bimestre,
                BimestrePeriodoInicio = periodoEscolar?.PeriodoInicio,
                BimestrePeriodoFim = periodoEscolar?.PeriodoFim,
                PeriodoFechamentoInicio = periodoFechamentoVigente?.PeriodoFechamentoInicio,
                PeriodoFechamentoFim = periodoFechamentoVigente?.PeriodoFechamentoFim,
                TipoNota = tipoNota,
                Media = mediaAprovacao,
                AnoLetivo = turma.AnoLetivo,
                PeriodoAberto = periodoAberto
            };
        }

        public async Task<ConselhoClasseAlunoResumoDto> ObterConselhoClasseTurmaFinal(string turmaCodigo, string alunoCodigo, bool consideraHistorico = false)
        {
            var turma = await ObterTurma(turmaCodigo);

            var turmasitinerarioEnsinoMedio = (await mediator.Send(ObterTurmaItinerarioEnsinoMedioQuery.Instance)).ToList();

            if (turma.EhTurmaEdFisicaOuItinerario() || turmasitinerarioEnsinoMedio.Any(a => a.Id == (int)turma.TipoTurma))
            {
                var turmasCodigosParaConsulta = new List<int>();
                turmasCodigosParaConsulta.AddRange(turma.ObterTiposRegularesDiferentes());

                var codigosTurmasRelacionadas = await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(turma.AnoLetivo, alunoCodigo, turmasCodigosParaConsulta, semestre: turma.Semestre != 0 ? turma.Semestre : null));

                turma = await ObterTurma(codigosTurmasRelacionadas.FirstOrDefault());
            }

            var fechamentoTurma = await consultasFechamentoTurma.ObterPorTurmaCodigoBimestreAsync(turma.CodigoTurma);

            var conselhoClasse = fechamentoTurma.NaoEhNulo() ? await repositorioConselhoClasseConsulta.ObterPorFechamentoId(fechamentoTurma.Id) : null;

            var conselhoClasseAluno = conselhoClasse != null ? await repositorioConselhoClasseAluno.ObterPorConselhoClasseAlunoCodigoAsync(conselhoClasse.Id, alunoCodigo) : null;

            var ultimoBimestre = (await ObterPeriodoUltimoBimestre(turma));

            var periodoFechamentoBimestre = await consultasPeriodoFechamento
                .TurmaEmPeriodoDeFechamentoVigente(turma, DateTimeExtension.HorarioBrasilia().Date, ultimoBimestre.Bimestre);

            var tipoNota = await ObterTipoNota(turma, periodoFechamentoBimestre);

            return new ConselhoClasseAlunoResumoDto()
            {
                FechamentoTurmaId = fechamentoTurma?.Id ?? 0,
                ConselhoClasseId = conselhoClasse?.Id ?? 0,
                ConselhoClasseAlunoId = conselhoClasseAluno?.Id ?? 0,
                TipoNota = tipoNota
            };
        }

        private async Task<TipoNota> ObterTipoNota(Turma turma, PeriodoFechamentoVigenteDto periodoFechamentoVigente)
        {
            if (turma.EhCELP())
                return TipoNota.Conceito;

            var dataReferencia = periodoFechamentoVigente?.PeriodoFechamentoFim ?? (await ObterPeriodoUltimoBimestre(turma)).PeriodoFim;

            var tipoNota = await mediator.Send(new ObterNotaTipoPorAnoModalidadeDataReferenciaQuery(turma.Ano, turma.ModalidadeCodigo, dataReferencia));

            if (tipoNota.EhNulo())
                throw new NegocioException(MensagemNegocioTurma.NAO_FOI_POSSIVEL_IDENTIFICAR_TIPO_NOTA_TURMA);

            return tipoNota.TipoNota;
        }

        private async Task<PeriodoEscolar> ObterPeriodoUltimoBimestre(Turma turma)
        {
            var periodoEscolarUltimoBimestre = await mediator.Send(new ObterUltimoPeriodoEscolarPorAnoModalidadeSemestreQuery(turma.AnoLetivo, turma.ModalidadeTipoCalendario, turma.Semestre));
            if (periodoEscolarUltimoBimestre.EhNulo())
                throw new NegocioException(MensagemNegocioPeriodo.NAO_FOI_ENCONTRADO_PERIODO_ULTIMO_BIMESTRE);

            return periodoEscolarUltimoBimestre;
        }

        private async Task<Turma> ObterTurma(string turmaCodigo)
        {
            var turma = await consultasTurma.ObterComUeDrePorCodigo(turmaCodigo);
            if (turma.EhNulo())
                throw new NegocioException(MensagemNegocioTurma.TURMA_NAO_ENCONTRADA);

            return turma;
        }

        private async Task<int> ObterBimestreAtual(Turma turma)
        {
            return await mediator.Send(new ObterBimestreAtualComAberturaPorTurmaQuery(turma, DateTimeExtension.HorarioBrasilia().Date));
        }

        public ConselhoClasse ObterPorId(long conselhoClasseId)
            => repositorioConselhoClasseConsulta.ObterPorId(conselhoClasseId);
    }
}