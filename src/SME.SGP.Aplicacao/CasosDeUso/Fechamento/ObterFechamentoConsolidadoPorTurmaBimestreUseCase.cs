using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
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
            foreach (var status in statusAgrupados)
            {
                yield return new StatusTotalFechamentoDto()
                {
                    Descricao = status.Key.Description(),
                    Quantidade = status.Count()
                };
            }
        }
    }
}
