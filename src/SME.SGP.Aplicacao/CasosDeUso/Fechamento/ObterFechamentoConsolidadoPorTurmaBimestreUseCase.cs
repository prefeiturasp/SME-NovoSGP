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
                throw new NegocioException("Fechamento consolidado não encontrado!");

            var statusAgrupados = listaFechamentosConsolidado.GroupBy(g => g.Status);

            return MapearRetornoStatusAgrupado(statusAgrupados);
        }

        private IEnumerable<StatusTotalFechamentoDto> MapearRetornoStatusAgrupado(IEnumerable<IGrouping<StatusFechamento, FechamentoConsolidadoComponenteTurma>> statusAgrupados)
        {
            var lstStatus = new List<StatusTotalFechamentoDto>();

            foreach (var status in statusAgrupados)
            {
                lstStatus.Add(new StatusTotalFechamentoDto()
                {
                    Status = status.Key,
                    Descricao = status.Key.Description(),
                    Quantidade = status.Count()
                });
            }

           var lstTodosStatus = Enum.GetValues(typeof(StatusFechamento)).Cast<StatusFechamento>();

            var statusNaoEncontrados = lstTodosStatus.Where(ls => !lstStatus.Select(s => s.Status).Contains(ls));

            if (statusNaoEncontrados != null && statusNaoEncontrados.Any()) 
            {
                foreach (var status in statusNaoEncontrados)
                {
                    lstStatus.Add(new StatusTotalFechamentoDto()
                    {
                        Status = status,
                        Descricao = status.Description(),
                        Quantidade = 0
                    });
                }
            }

            return lstStatus.OrderBy(o => (int)o.Status);
        }
    }
}
