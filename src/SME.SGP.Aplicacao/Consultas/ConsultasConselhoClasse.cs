using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasConselhoClasse : IConsultasConselhoClasse
    {
        private readonly IRepositorioConselhoClasse repositorioConselhoClasse;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;
        private readonly IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno;
        private readonly IRepositorioFechamentoTurma repositorioFechamentoTurma;
        private readonly IConsultasTurma consultasTurma;
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private readonly IConsultasPeriodoFechamento consultasPeriodoFechamento;
        private readonly IConsultasFechamentoTurma consultasFechamentoTurma;
        private readonly IServicoDeNotasConceitos servicoDeNotasConceitos;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IMediator mediator;

        public ConsultasConselhoClasse(IRepositorioConselhoClasse repositorioConselhoClasse,
                                       IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                                       IRepositorioParametrosSistema repositorioParametrosSistema,
                                       IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno,
                                       IRepositorioTipoCalendario repositorioTipoCalendario,
                                       IRepositorioFechamentoTurma repositorioFechamentoTurma,
                                       IConsultasTurma consultasTurma,
                                       IConsultasPeriodoEscolar consultasPeriodoEscolar,
                                       IConsultasPeriodoFechamento consultasPeriodoFechamento,
                                       IConsultasFechamentoTurma consultasFechamentoTurma,
                                       IServicoDeNotasConceitos servicoDeNotasConceitos,
                                       IMediator mediator)
        {
            this.repositorioConselhoClasse = repositorioConselhoClasse ?? throw new ArgumentNullException(nameof(repositorioConselhoClasse));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
            this.consultasTurma = consultasTurma ?? throw new ArgumentNullException(nameof(consultasTurma));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
            this.consultasFechamentoTurma = consultasFechamentoTurma ?? throw new ArgumentNullException(nameof(consultasFechamentoTurma));
            this.servicoDeNotasConceitos = servicoDeNotasConceitos ?? throw new ArgumentNullException(nameof(servicoDeNotasConceitos));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ConselhoClasseAlunoResumoDto> ObterConselhoClasseTurma(string turmaCodigo, string alunoCodigo, int bimestre = 0, bool ehFinal = false, bool consideraHistorico = false)
        {
            var turma = await ObterTurma(turmaCodigo);

            if (turma.EhTurmaEdFisicaOuItinerario())
            {
                var tipos = new List<TipoTurma>() {
                        TipoTurma.Regular
                    };
                var codigosTurmasRelacionadas = await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(turma.AnoLetivo, alunoCodigo, tipos));
                turma = await ObterTurma(codigosTurmasRelacionadas.FirstOrDefault());
            }

            if (bimestre == 0 && !ehFinal)
            {
                bimestre = await ObterBimestreAtual(turma);
                if (bimestre == 0)
                    bimestre = 1;
            }
            var fechamentoTurma = await consultasFechamentoTurma.ObterPorTurmaCodigoBimestreAsync(turma.CodigoTurma, bimestre);

            if (bimestre == 0 && !consideraHistorico && turma.AnoLetivo != 2020)
            {
                var retornoConselhoBimestre = await mediator.Send(new ObterUltimoBimestreTurmaQuery(turma));
                if (!retornoConselhoBimestre.possuiConselho)
                    throw new NegocioException($"Para acessar esta aba você precisa registrar o conselho de classe do {retornoConselhoBimestre.bimestre}º bimestre");
            }

            if (fechamentoTurma == null && !turma.EhAnoAnterior())
                throw new NegocioException("Fechamento da turma não localizado " + (!ehFinal && bimestre > 0 ? $"para o bimestre {bimestre}" : ""));

            var conselhoClasse = fechamentoTurma != null ? await repositorioConselhoClasse.ObterPorFechamentoId(fechamentoTurma.Id) : null;

            var periodoEscolarId = fechamentoTurma?.PeriodoEscolarId;

            PeriodoEscolar periodoEscolar;

            if (periodoEscolarId == null)
            {
                var tipoCalendario = await repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(turma.AnoLetivo, turma.ModalidadeTipoCalendario, turma.Semestre);
                if (tipoCalendario == null) throw new NegocioException("Tipo de calendário não encontrado");

                periodoEscolar = await repositorioPeriodoEscolar.ObterPorTipoCalendarioEBimestreAsync(tipoCalendario.Id, bimestre);

                periodoEscolarId = periodoEscolar?.Id;
            }
            else
            {
                periodoEscolar = await repositorioPeriodoEscolar.ObterPorIdAsync(periodoEscolarId.Value);
            }

            var bimestreFechamento = !ehFinal ? bimestre : (await ObterPeriodoUltimoBimestre(turma)).Bimestre;

            var periodoFechamentoVigente = await consultasPeriodoFechamento.TurmaEmPeriodoDeFechamentoVigente(turma, DateTimeExtension.HorarioBrasilia().Date, bimestreFechamento);

            var tipoNota = await ObterTipoNota(turma, periodoFechamentoVigente, consideraHistorico);

            var mediaAprovacao = double.Parse(await repositorioParametrosSistema
                .ObterValorPorTipoEAno(TipoParametroSistema.MediaBimestre));

            var conselhoClasseAluno = conselhoClasse != null ? await repositorioConselhoClasseAluno.ObterPorConselhoClasseAlunoCodigoAsync(conselhoClasse.Id, alunoCodigo) : null;

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
                AnoLetivo = turma.AnoLetivo
            };
        }

        private async Task<TipoNota> ObterTipoNota(Turma turma, PeriodoFechamentoVigenteDto periodoFechamentoVigente, bool consideraHistorico = false)
        {
            var dataReferencia = periodoFechamentoVigente != null ?
                periodoFechamentoVigente.PeriodoFechamentoFim :
                (await ObterPeriodoUltimoBimestre(turma)).PeriodoFim;

            var tipoNota = await mediator.Send(new ObterNotaTipoPorAnoModalidadeDataReferenciaQuery(turma.Ano, turma.ModalidadeCodigo, dataReferencia));
            if (tipoNota == null)
                throw new NegocioException("Não foi possível identificar o tipo de nota da turma");

            return tipoNota.TipoNota;
        }

        private async Task<PeriodoEscolar> ObterPeriodoUltimoBimestre(Turma turma)
        {
            var periodoEscolarUltimoBimestre = await consultasPeriodoEscolar.ObterUltimoPeriodoAsync(turma.AnoLetivo, turma.ModalidadeTipoCalendario, turma.Semestre);
            if (periodoEscolarUltimoBimestre == null)
                throw new NegocioException("Não foi possível localizar o período escolar do ultimo bimestre da turma");

            return periodoEscolarUltimoBimestre;
        }

        private async Task<Turma> ObterTurma(string turmaCodigo)
        {
            var turma = await consultasTurma.ObterComUeDrePorCodigo(turmaCodigo);
            if (turma == null)
                throw new NegocioException("Turma não localizada");

            return turma;
        }

        private async Task<int> ObterBimestreAtual(Turma turma)
        {
            var bimestre = await mediator.Send(new ObterBimestreAtualComAberturaPorTurmaQuery(turma, DateTime.Today));
            return bimestre;
        }

        public ConselhoClasse ObterPorId(long conselhoClasseId)
            => repositorioConselhoClasse.ObterPorId(conselhoClasseId);

        public async Task<(int, bool)> ValidaConselhoClasseUltimoBimestre(Turma turma)
        {
            return await mediator
                .Send(new ObterUltimoBimestreTurmaQuery(turma));
        }
    }
}