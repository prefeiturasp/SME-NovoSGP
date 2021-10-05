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
    public class InserirConsolidacaoDashBoardFrequenciaCommandHandler : IRequestHandler<InserirConsolidacaoDashBoardFrequenciaCommand, bool>
    {        
        private readonly IRepositorioConsolidacaoFrequenciaTurma repositorioConsolidacaoFrequenciaTurma;
        private readonly IMediator mediator;

        public InserirConsolidacaoDashBoardFrequenciaCommandHandler(IRepositorioConsolidacaoFrequenciaTurma repositorioConsolidacaoFrequenciaTurma, IMediator mediator)
        {            
            this.repositorioConsolidacaoFrequenciaTurma = repositorioConsolidacaoFrequenciaTurma ?? throw new ArgumentNullException(nameof(repositorioConsolidacaoFrequenciaTurma));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(InserirConsolidacaoDashBoardFrequenciaCommand request, CancellationToken cancellationToken)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(request.TurmaId));

            if (turma == null)
                return false;

            var anoLetivo = request.DataAula.Year;
            int? mes = null;
            var data = request.DataAula;
            DateTime? dataInicioSemena = null;
            DateTime? dataFinalSemena = null; 

            if (request.TipoPeriodo == TipoPeriodoDashboardFrequencia.Semanal)
            {
                while (data.DayOfWeek != DayOfWeek.Monday)
                {
                    data = data.AddDays(-1);
                }

                dataInicioSemena = data;
                dataFinalSemena = data.AddDays(6);
            }else if(request.TipoPeriodo == TipoPeriodoDashboardFrequencia.Mensal)
            {
                mes = request.DataAula.Month;
            }
            
            await mediator.Send(new ExcluirConsolidacaoDashBoardFrequenciaPorDataETipoCommand(anoLetivo,
                                                                                              turma.Id,
                                                                                              request.DataAula,
                                                                                              request.TipoPeriodo,
                                                                                              dataInicioSemena,
                                                                                              dataFinalSemena,
                                                                                              mes));

            var consolidacao = await mediator.Send(new ObterDadosParaConsolidacaoDashBoardFrequenciaPorTurmaQuery(anoLetivo,
                                                                                                                  request.TurmaId,
                                                                                                                  turma.ModalidadeCodigo,
                                                                                                                  request.TipoPeriodo,
                                                                                                                  request.DataAula,
                                                                                                                  mes,
                                                                                                                  dataInicioSemena,
                                                                                                                  dataFinalSemena));

            if (consolidacao == null)
                return false;

            return await repositorioConsolidacaoFrequenciaTurma.InserirConsolidacaoDashBoard(MapearParaEntidade(turma,
                                                                                                                consolidacao,
                                                                                                                request.DataAula,
                                                                                                                (int)request.TipoPeriodo,
                                                                                                                dataInicioSemena,
                                                                                                                dataFinalSemena,
                                                                                                                mes)) != 0;
        }

        private ConsolidacaoDashBoardFrequencia MapearParaEntidade(Turma turma, DadosParaConsolidacaoDashBoardFrequenciaDto dados, DateTime dataAula, int tipoPeriodo, DateTime? dataInicio = null, DateTime? dataFim = null, int? mes = null)
        {
            return new ConsolidacaoDashBoardFrequencia()
            {
                AnoLetivo = dataAula.Year,
                TurmaId = turma.Id,
                TurmaNome = turma.NomeComModalidade(),
                TurmaAno = turma.Ano,
                DataAula = dataAula,
                DataInicio = dataInicio,
                DataFim = dataFim,
                Mes = mes,
                ModalidadeCodigo = (int)turma.ModalidadeCodigo,
                Tipo = tipoPeriodo,
                DreId = turma.Ue.DreId,
                UeId = turma.UeId,
                DreAbreviacao = turma.Ue.Dre.Abreviacao,
                QuantidadePresencas = dados.Presentes,
                QuantidadeRemotos = dados.Remotos,
                QuantidadeAusentes = dados.Ausentes,
                CriadoEm = DateTime.Now
            };
        }
    }
}
