using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizarPendenciaFechamentoPorAulaCommandHandler : IRequestHandler<AtualizarPendenciaFechamentoPorAulaCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IServicoPendenciaFechamento servicoPendenciaFechamento;
        private readonly IDictionary<TipoPendencia, Func<ParametroAtualizacaoPendencia, Task>> dicionarioAtualizacaoPorTipo;

        public AtualizarPendenciaFechamentoPorAulaCommandHandler(IMediator mediator, IServicoPendenciaFechamento servicoPendenciaFechamento)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoPendenciaFechamento = servicoPendenciaFechamento ?? throw new ArgumentNullException(nameof(servicoPendenciaFechamento));
            dicionarioAtualizacaoPorTipo = ObterDicionarioAtualizacaoPorTipo();
        }

        public async Task<bool> Handle(AtualizarPendenciaFechamentoPorAulaCommand request, CancellationToken cancellationToken)
        {
            var pendenciasFechamentos = await mediator.Send(new ObterPendenciaFechamentoPorAulaQuery(request.IdAula, request.TipoPendencia));

            if (pendenciasFechamentos.Any())
            {
                foreach (var pendencia in pendenciasFechamentos)
                {
                    var fechamentoTurmaDisciplina = await mediator.Send(new ObterFechamentoTurmaDisciplinaPorIdQuery(pendencia.FechamentoTurmaDisciplinaId));

                    if (fechamentoTurmaDisciplina != null)
                    {
                        var turma = await mediator.Send(new ObterTurmaPorIdQuery(fechamentoTurmaDisciplina.FechamentoTurma.TurmaId));
                        var periodoEscolar = await ObterPeriodoEscolar(turma, fechamentoTurmaDisciplina.FechamentoTurma);
                        var parametro = new ParametroAtualizacaoPendencia(fechamentoTurmaDisciplina, turma, periodoEscolar, pendencia.Pendencia);

                        if (dicionarioAtualizacaoPorTipo.ContainsKey(request.TipoPendencia))
                        {
                            await dicionarioAtualizacaoPorTipo[request.TipoPendencia](parametro);
                        }
                    }
                }
            }

            return true;
        }

        private IDictionary<TipoPendencia, Func<ParametroAtualizacaoPendencia, Task>> ObterDicionarioAtualizacaoPorTipo()
        {
            return new Dictionary<TipoPendencia, Func<ParametroAtualizacaoPendencia, Task>>() 
            {
                { TipoPendencia.AulasSemFrequenciaNaDataDoFechamento, AtualizarPendenciaDeFrequencia },
                { TipoPendencia.AulasSemPlanoAulaNaDataDoFechamento, AtualizarPendenciaDePlanoAula },
                { TipoPendencia.AvaliacaoSemNotaParaNenhumAluno, AtualizarPendenciaAvaliacoes }
            };
        }

        private async Task AtualizarPendenciaDeFrequencia(ParametroAtualizacaoPendencia parametro)
        {
            await this.servicoPendenciaFechamento.ValidarAulasSemFrequenciaRegistrada(
                                                                parametro.FechamentoId,
                                                                parametro.Turma.CodigoTurma,
                                                                parametro.Turma.Nome,
                                                                parametro.DisciplinaId,
                                                                parametro.InicioPeriodo,
                                                                parametro.FimPeriodo,
                                                                parametro.Bimestre,
                                                                parametro.Turma.Id,
                                                                parametro.Pendencia);
        }

        private async Task AtualizarPendenciaDePlanoAula(ParametroAtualizacaoPendencia parametro)
        {
            await servicoPendenciaFechamento.ValidarAulasSemPlanoAulaNaDataDoFechamento(
                                                                parametro.FechamentoId,
                                                                parametro.Turma,
                                                                parametro.DisciplinaId,
                                                                parametro.InicioPeriodo,
                                                                parametro.FimPeriodo,
                                                                parametro.Bimestre,
                                                                parametro.Turma.Id,
                                                                parametro.Pendencia);
        }

        private async Task AtualizarPendenciaAvaliacoes(ParametroAtualizacaoPendencia parametro)
        {
            await servicoPendenciaFechamento.ValidarAvaliacoesSemNotasParaNenhumAluno(
                                                                parametro.FechamentoId,
                                                                parametro.Turma.CodigoTurma,
                                                                parametro.DisciplinaId,
                                                                parametro.InicioPeriodo,
                                                                parametro.FimPeriodo,
                                                                parametro.Bimestre,
                                                                parametro.Turma.Id,
                                                                parametro.Pendencia);
        }

        private async Task<PeriodoEscolar> ObterPeriodoEscolar(Turma turma, FechamentoTurma fechamentoTurma)
        {
            var tipoCalendario = await mediator.Send(
                        new ObterTipoCalendarioIdPorAnoLetivoEModalidadeQuery(
                            turma.ModalidadeCodigo == Modalidade.EJA
                                ? ModalidadeTipoCalendario.EJA
                                : ModalidadeTipoCalendario.FundamentalMedio, turma.AnoLetivo, turma.Semestre));
            var ue = turma.Ue;
            var bimestre = !fechamentoTurma.PeriodoEscolarId.HasValue && turma.ModalidadeTipoCalendario != ModalidadeTipoCalendario.EJA ? 4
                : !fechamentoTurma.PeriodoEscolarId.HasValue && turma.ModalidadeTipoCalendario.Equals(ModalidadeTipoCalendario.EJA) ? 2
                : await mediator.Send(new ObterBimestreDoPeriodoEscolarQuery(fechamentoTurma.PeriodoEscolarId.GetValueOrDefault()));

            return await ObterPeriodoEscolarFechamentoReabertura(tipoCalendario, ue, bimestre);
        }

        private async Task<PeriodoEscolar> ObterPeriodoEscolarFechamentoReabertura(long tipoCalendarioId, Ue ue, int bimestre)
        {
            var periodoFechamento = await mediator.Send(new ObterPeriodoFechamentoPorCalendarioIdQuery(tipoCalendarioId));
            var periodoFechamentoBimestre = periodoFechamento?.FechamentosBimestres.FirstOrDefault(x => x.Bimestre == bimestre);

            if (periodoFechamento == null || periodoFechamentoBimestre == null)
            {
                var hoje = DateTime.Today;
                var tipodeEventoReabertura = await mediator.Send(new ObterEventoTipoIdPorCodigoQuery(TipoEvento.FechamentoBimestre));

                if (await mediator.Send(new ExisteEventoNaDataPorTipoDreUEQuery(hoje, tipoCalendarioId, (TipoEvento)tipodeEventoReabertura, ue.CodigoUe, ue.Dre.CodigoDre)))
                {
                    return (await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioQuery(tipoCalendarioId))).FirstOrDefault(a => a.Bimestre == bimestre);
                }
            }

            return periodoFechamentoBimestre?.PeriodoEscolar;
        }

        private class ParametroAtualizacaoPendencia
        {
            public ParametroAtualizacaoPendencia(FechamentoTurmaDisciplina fechamentoTurmaDisciplina,
                                                 Turma turma,
                                                 PeriodoEscolar periodoEscolar,
                                                 Pendencia pendencia)
            {
                FechamentoId = fechamentoTurmaDisciplina.Id;
                Turma = turma;
                DisciplinaId = fechamentoTurmaDisciplina.DisciplinaId;
                InicioPeriodo = periodoEscolar.PeriodoInicio;
                FimPeriodo = periodoEscolar.PeriodoFim;
                Bimestre = periodoEscolar.Bimestre;
                Pendencia = pendencia;
            }

            public long FechamentoId { get; set; }
            public Turma Turma { get; set; }
            public long DisciplinaId { get; set; }
            public DateTime InicioPeriodo { get; set; }
            public DateTime FimPeriodo { get; set; }
            public int Bimestre { get; set; }
            public Pendencia Pendencia { get; set; }
        }
    }
}
