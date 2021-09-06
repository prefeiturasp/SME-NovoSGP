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
            int[] situacoesFechamento = new int[] { filtro.SituacaoFechamento };

            if (filtro.SituacaoFechamento == (int)SituacaoFechamento.NaoIniciado)            
                situacoesFechamento = new int[] { (int)SituacaoFechamento.NaoIniciado, (int)SituacaoFechamento.EmProcessamento };                     

            var listaFechamentosConsolidado = await mediator.Send(new ObterFechamentoConsolidadoPorTurmaBimestreQuery(filtro.TurmaId, filtro.Bimestre, situacoesFechamento));

            if (listaFechamentosConsolidado == null || !listaFechamentosConsolidado.Any())
                return Enumerable.Empty<StatusTotalFechamentoDto>();

            var statusAgrupados = listaFechamentosConsolidado.GroupBy(g => g.Status);

            return MapearRetornoStatusAgrupado(statusAgrupados);
        }

        private IEnumerable<StatusTotalFechamentoDto> MapearRetornoStatusAgrupado(IEnumerable<IGrouping<SituacaoFechamento, FechamentoConsolidadoComponenteTurma>> statusAgrupados)
        {
            var lstStatus = new List<StatusTotalFechamentoDto>();

            if (statusAgrupados.Any(sa => sa.Key == SituacaoFechamento.NaoIniciado || 
                                          sa.Key == SituacaoFechamento.EmProcessamento))
            {
                var qtdStatusNaoIniciado = statusAgrupados.Where(sa => sa.Key == SituacaoFechamento.NaoIniciado || 
                                                                       sa.Key == SituacaoFechamento.EmProcessamento)
                                                          .SelectMany(s => s).Count();

                lstStatus.Add(new StatusTotalFechamentoDto()
                {
                    Status = (int)SituacaoFechamento.NaoIniciado,
                    Descricao = SituacaoFechamento.NaoIniciado.Name(),
                    Quantidade = qtdStatusNaoIniciado
                });

            }
            else
            {
                lstStatus.Add(new StatusTotalFechamentoDto()
                {
                    Status = (int)SituacaoFechamento.NaoIniciado,
                    Descricao = SituacaoFechamento.NaoIniciado.Name(),
                    Quantidade = 0
                });
            }

            foreach (var status in statusAgrupados.Where(sa => sa.Key != SituacaoFechamento.NaoIniciado && 
                                                               sa.Key != SituacaoFechamento.EmProcessamento))
            {
                lstStatus.Add(new StatusTotalFechamentoDto()
                {
                    Status = (int)status.Key,
                    Descricao = status.Key.Name(),
                    Quantidade = status.Count()
                });
            }

            var lstTodosStatus = Enum.GetValues(typeof(SituacaoFechamento)).Cast<SituacaoFechamento>();

            var statusNaoEncontrados = lstTodosStatus.Where(ls => !lstStatus.Select(s => (SituacaoFechamento)s.Status).Contains(ls));

            if (statusNaoEncontrados != null && statusNaoEncontrados.Any())
            {
                foreach (var status in statusNaoEncontrados.Where(s => s != SituacaoFechamento.NaoIniciado && 
                                                                       s != SituacaoFechamento.EmProcessamento))
                {
                    lstStatus.Add(new StatusTotalFechamentoDto()
                    {
                        Status = (int)status,
                        Descricao = status.Name(),
                        Quantidade = 0
                    });
                }
            }

            return lstStatus.OrderBy(o => (int)o.Status);
        }
    }
}
