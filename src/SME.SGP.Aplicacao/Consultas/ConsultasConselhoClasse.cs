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
        private readonly IRepositorioConselhoClasseConsulta repositorioConselhoClasseConsulta;
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;
        private readonly IRepositorioConselhoClasseAlunoConsulta repositorioConselhoClasseAluno;
        private readonly IRepositorioParametrosSistemaConsulta repositorioParametrosSistema;
        private readonly IConsultasTurma consultasTurma;
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private readonly IConsultasPeriodoFechamento consultasPeriodoFechamento;
        private readonly IConsultasFechamentoTurma consultasFechamentoTurma;
        private readonly IRepositorioTipoCalendarioConsulta repositorioTipoCalendario;
        private readonly IMediator mediator;

        public ConsultasConselhoClasse(IRepositorioConselhoClasseConsulta repositorioConselhoClasseConsulta,
                                       IRepositorioConselhoClasseAlunoConsulta repositorioConselhoClasseAluno,
                                       IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar,
                                       IRepositorioParametrosSistemaConsulta repositorioParametrosSistema,
                                       IRepositorioTipoCalendarioConsulta repositorioTipoCalendario,
                                       IConsultasTurma consultasTurma,
                                       IConsultasPeriodoEscolar consultasPeriodoEscolar,
                                       IConsultasPeriodoFechamento consultasPeriodoFechamento,
                                       IConsultasFechamentoTurma consultasFechamentoTurma,
                                       IMediator mediator)
        {
            this.repositorioConselhoClasseConsulta = repositorioConselhoClasseConsulta ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseConsulta));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.consultasTurma = consultasTurma ?? throw new ArgumentNullException(nameof(consultasTurma));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
            this.consultasFechamentoTurma = consultasFechamentoTurma ?? throw new ArgumentNullException(nameof(consultasFechamentoTurma));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ConselhoClasseAlunoResumoDto> ObterConselhoClasseTurma(string turmaCodigo, string alunoCodigo, int bimestre = 0, bool ehFinal = false, bool consideraHistorico = false)
        {
            var turma = await ObterTurma(turmaCodigo);

            if (turma.EhTurmaEdFisicaOuItinerario())
            {
                var tipos = new List<TipoTurma>() {
                        TipoTurma.Regular, TipoTurma.Itinerarios2AAno, TipoTurma.EdFisica
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

            var conselhoClasse = fechamentoTurma != null ? await repositorioConselhoClasseConsulta.ObterPorFechamentoId(fechamentoTurma.Id) : null;

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

            PeriodoFechamentoBimestre periodoFechamentoBimestre = await consultasPeriodoFechamento
                .ObterPeriodoFechamentoTurmaAsync(turma, bimestreFechamento, periodoEscolarId);

            var tipoNota = await ObterTipoNota(turma, periodoFechamentoBimestre, consideraHistorico);

            var mediaAprovacao = double.Parse(await repositorioParametrosSistema.ObterValorPorTipoEAno(TipoParametroSistema.MediaBimestre));

            var conselhoClasseAluno = conselhoClasse != null ? await repositorioConselhoClasseAluno.ObterPorConselhoClasseAlunoCodigoAsync(conselhoClasse.Id, alunoCodigo) : null;

            return new ConselhoClasseAlunoResumoDto()
            {
                FechamentoTurmaId = fechamentoTurma?.Id,
                ConselhoClasseId = conselhoClasse?.Id,
                ConselhoClasseAlunoId = conselhoClasseAluno?.Id,
                Bimestre = bimestre,
                BimestrePeriodoInicio = periodoEscolar?.PeriodoInicio,
                BimestrePeriodoFim = periodoEscolar?.PeriodoFim,
                PeriodoFechamentoInicio = periodoFechamentoBimestre?.InicioDoFechamento,
                PeriodoFechamentoFim = periodoFechamentoBimestre?.FinalDoFechamento,
                TipoNota = tipoNota,
                Media = mediaAprovacao,
                AnoLetivo = turma.AnoLetivo
            };
        }

        private async Task<TipoNota> ObterTipoNota(Turma turma, PeriodoFechamentoBimestre periodoFechamentoBimestre, bool consideraHistorico = false)
        {
            var dataReferencia = periodoFechamentoBimestre != null ?
                periodoFechamentoBimestre.FinalDoFechamento :
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
            => repositorioConselhoClasseConsulta.ObterPorId(conselhoClasseId);

        public async Task<(int, bool)> ValidaConselhoClasseUltimoBimestre(Turma turma)
        {
            return await mediator
                .Send(new ObterUltimoBimestreTurmaQuery(turma));
        }
    }
}