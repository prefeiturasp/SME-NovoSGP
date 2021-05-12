using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseConsolidadoPorTurmaBimestreUseCase : AbstractUseCase, IObterConselhoClasseConsolidadoPorTurmaBimestreUseCase
    {
        public ObterConselhoClasseConsolidadoPorTurmaBimestreUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<StatusTotalFechamentoDto>> Executar(FiltroConselhoClasseConsolidadoTurmaBimestreDto filtro)
        {
            var listaConselhosClasseConsolidado = await mediator.Send(new ObterConselhoClasseConsolidadoPorTurmaBimestreQuery(filtro.TurmaId, filtro.Bimestre));

            if (listaConselhosClasseConsolidado == null || !listaConselhosClasseConsolidado.Any())
                throw new NegocioException("Conselhos classe consolidado não encontrado!");

            var statusAgrupados = listaConselhosClasseConsolidado.GroupBy(g => g.Status);

            return MapearRetornoStatusAgrupado(statusAgrupados);
        }

        private IEnumerable<StatusTotalFechamentoDto> MapearRetornoStatusAgrupado(IEnumerable<IGrouping<StatusFechamento, ConselhoClasseConsolidadoTurmaAluno>> statusAgrupados)
        {
            foreach (var status in statusAgrupados)
            {
                yield return new StatusTotalFechamentoDto()
                {
                    Descricao = status.Key.Description(),
                    Quantidade = status.Select(t => t.TurmaId).Distinct().Count()
                };
            }
        }
    }
}
