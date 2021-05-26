using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoConsolidadoPorTurmaBimestreUseCase : AbstractUseCase, IObterFechamentoConsolidadoPorTurmaBimestreUseCase
    {
        public ObterFechamentoConsolidadoPorTurmaBimestreUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<StatusTotalFechamentoDto>> Executar(FiltroFechamentoConsolidadoTurmaBimestreDto filtro)
        {
            var listaFechamentosConsolidado = await mediator.Send(new ObterFechamentoConsolidadoPorTurmaBimestreQuery(filtro.TurmaId, filtro.Bimestre));

            if (listaFechamentosConsolidado == null || !listaFechamentosConsolidado.Any())
                return Enumerable.Empty<StatusTotalFechamentoDto>();

            var statusAgrupados = listaFechamentosConsolidado.GroupBy(g => g.Status);

            return MapearRetornoStatusAgrupado(statusAgrupados);
        }

        private IEnumerable<StatusTotalFechamentoDto> MapearRetornoStatusAgrupado(IEnumerable<IGrouping<SituacaoFechamento, FechamentoConsolidadoComponenteTurma>> statusAgrupados)
        {
            var lstStatus = new List<StatusTotalFechamentoDto>();

            foreach (var status in statusAgrupados)
            {
                lstStatus.Add(new StatusTotalFechamentoDto()
                {
                    Status = status.Key == SituacaoFechamento.EmProcessamento ? (int)SituacaoFechamento.NaoIniciado : (int)status.Key,
                    Descricao = status.Key == SituacaoFechamento.EmProcessamento ? SituacaoFechamento.NaoIniciado.Name() : status.Key.Name(),
                    Quantidade = status.Count()
                });
            }

           var lstTodosStatus = Enum.GetValues(typeof(SituacaoFechamento)).Cast<SituacaoFechamento>();

            var statusNaoEncontrados = lstTodosStatus.Where(ls => !lstStatus.Select(s => (SituacaoFechamento)s.Status).Contains(ls));

            if (statusNaoEncontrados != null && statusNaoEncontrados.Any()) 
            {
                foreach (var status in statusNaoEncontrados.Where(s => s != SituacaoFechamento.EmProcessamento))
                {
                    lstStatus.Add(new StatusTotalFechamentoDto()
                    {
                        Status = status == SituacaoFechamento.EmProcessamento ? (int)SituacaoFechamento.NaoIniciado : (int)status,
                        Descricao = status.Name(),
                        Quantidade = 0
                    });
                }
            }

            return lstStatus.OrderBy(o => (int)o.Status);
        }
    }
}
