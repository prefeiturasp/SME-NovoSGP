using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
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

        public async Task<IEnumerable<StatusTotalConselhoClasseDto>> Executar(FiltroConselhoClasseConsolidadoTurmaBimestreDto filtro)
        {
            var listaConselhosClasseConsolidado = await mediator.Send(new ObterConselhoClasseConsolidadoPorTurmaBimestreQuery(filtro.TurmaId, filtro.Bimestre));

            if (listaConselhosClasseConsolidado == null || !listaConselhosClasseConsolidado.Any())
                return Enumerable.Empty<StatusTotalConselhoClasseDto>();

            var statusAgrupados = listaConselhosClasseConsolidado.GroupBy(g => g.Status);

            return MapearRetornoStatusAgrupado(statusAgrupados);
        }

        private IEnumerable<StatusTotalConselhoClasseDto> MapearRetornoStatusAgrupado(IEnumerable<IGrouping<StatusConselhoClasse, ConselhoClasseConsolidadoTurmaAluno>> statusAgrupados)
        {
            var lstStatus = new List<StatusTotalConselhoClasseDto>();

            foreach (var status in statusAgrupados)
            {
                lstStatus.Add(new StatusTotalConselhoClasseDto()
                {
                    Status = status.Key,
                    Descricao = status.Key.Description(),
                    Quantidade = status.Count()
                });
            }

            var lstTodosStatus = Enum.GetValues(typeof(StatusConselhoClasse)).Cast<StatusConselhoClasse>();

            var statusNaoEncontrados = lstTodosStatus.Where(ls => !lstStatus.Select(s => s.Status).Contains(ls));

            if (statusNaoEncontrados != null && statusNaoEncontrados.Any())
            {
                foreach (var status in statusNaoEncontrados)
                {
                    lstStatus.Add(new StatusTotalConselhoClasseDto()
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
