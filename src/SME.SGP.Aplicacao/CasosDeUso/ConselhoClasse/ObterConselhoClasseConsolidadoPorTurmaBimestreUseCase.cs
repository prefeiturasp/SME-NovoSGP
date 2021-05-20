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

            return lstStatus;
        }
    }
}
