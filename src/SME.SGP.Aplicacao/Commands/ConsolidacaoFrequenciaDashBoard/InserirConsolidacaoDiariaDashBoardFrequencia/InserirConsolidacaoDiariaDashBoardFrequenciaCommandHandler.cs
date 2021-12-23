using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirConsolidacaoDiariaDashBoardFrequenciaCommandHandler : IRequestHandler<InserirConsolidacaoDiariaDashBoardFrequenciaCommand, bool>
    {
        private readonly IRepositorioConsolidacaoFrequenciaTurma repositorioConsolidacaoFrequenciaTurma;
        private readonly IMediator mediator;

        public InserirConsolidacaoDiariaDashBoardFrequenciaCommandHandler(IRepositorioConsolidacaoFrequenciaTurma repositorioConsolidacaoFrequenciaTurma, IMediator mediator)
        {
            this.repositorioConsolidacaoFrequenciaTurma = repositorioConsolidacaoFrequenciaTurma ?? throw new ArgumentNullException(nameof(repositorioConsolidacaoFrequenciaTurma));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(InserirConsolidacaoDiariaDashBoardFrequenciaCommand request, CancellationToken cancellationToken)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(request.TurmaId));

            if (turma == null)
                return false;

            var anoLetivo = request.AnoLetivo;
            var mes = request.Mes;
            var primeiroDiaDoMes = new DateTime(anoLetivo, mes, 1);
            var ultimoDiaDoMes = new DateTime(anoLetivo, mes, DateTime.DaysInMonth(anoLetivo, mes));

            for (var data = primeiroDiaDoMes; data <= ultimoDiaDoMes; data = data.AddDays(1))
            {
                var consolidacao = await mediator.Send(new ObterDadosParaConsolidacaoDashBoardFrequenciaPorTurmaQuery(anoLetivo,
                                                                                                                      request.TurmaId,
                                                                                                                      turma.ModalidadeCodigo,
                                                                                                                      TipoPeriodoDashboardFrequencia.Diario,
                                                                                                                      data,
                                                                                                                      mes,
                                                                                                                      null,
                                                                                                                      null));

                if (consolidacao == null)
                    return false;

                if (consolidacao.Presentes == 0 && consolidacao.Ausentes == 0 && consolidacao.Remotos == 0)
                    continue;

                await mediator.Send(new ExcluirConsolidacaoDashBoardFrequenciaPorDataETipoCommand(anoLetivo,
                                                                                              turma.Id,
                                                                                              data,
                                                                                              TipoPeriodoDashboardFrequencia.Diario,
                                                                                              null,
                                                                                              null,
                                                                                              mes));

                await repositorioConsolidacaoFrequenciaTurma.InserirConsolidacaoDashBoard(MapearParaEntidade(turma,
                                                                                                             consolidacao,
                                                                                                             data,
                                                                                                             (int)TipoPeriodoDashboardFrequencia.Diario,
                                                                                                             null,
                                                                                                             null,
                                                                                                             mes));

            }

            return true;
        }

        private ConsolidacaoDashBoardFrequencia MapearParaEntidade(Turma turma, DadosParaConsolidacaoDashBoardFrequenciaDto dados, DateTime dataAula, int tipoPeriodo, DateTime? dataInicio = null, DateTime? dataFim = null, int? mes = null)
        {
            return new ConsolidacaoDashBoardFrequencia()
            {
                AnoLetivo = dataAula.Year,
                TurmaId = turma.Id,
                TurmaNome = turma.NomeComModalidade(),
                TurmaAno = turma.AnoComModalidade(),
                semestre = turma.Semestre,
                DataAula = dataAula,
                DataInicio = dataInicio,
                DataFim = dataFim,
                Mes = mes,
                ModalidadeCodigo = (int)turma.ModalidadeCodigo,
                Tipo = tipoPeriodo,
                DreId = turma.Ue.DreId,
                DreCodigo = turma.Ue.Dre.CodigoDre,
                UeId = turma.UeId,
                DreAbreviacao = AbreviacaoDreFormatado(turma.Ue.Dre.Abreviacao),
                QuantidadePresencas = dados.Presentes,
                QuantidadeRemotos = dados.Remotos,
                QuantidadeAusentes = dados.Ausentes,
                CriadoEm = DateTime.Now
            };
        }
        private static string AbreviacaoDreFormatado(string abreviacaoDre)
           => abreviacaoDre.Replace(DashboardConstants.PrefixoDreParaSerRemovido, string.Empty).Trim();       
    }
}
