using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDashboardAcompanhamentoAprendizagemPorDreUseCase : AbstractUseCase, IObterDashboardAcompanhamentoAprendizagemPorDreUseCase
    {
        public ObterDashboardAcompanhamentoAprendizagemPorDreUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<GraficoBaseDto>> Executar(FiltroDashboardAcompanhamentoAprendizagemPorDreDto filtro)
        {
            var acompanhamentos = await mediator.Send(new ObterDashboardAcompanhamentoAprendizagemPorDreQuery(filtro.AnoLetivo, filtro.Semestre));

            var dashboard = new List<GraficoBaseDto>();
            foreach (var acompanhamento in acompanhamentos.Where(c => c.QuantidadeComAcompanhamento > 0 || c.QuantidadeSemAcompanhamento > 0))
            {
                dashboard.Add(new GraficoBaseDto(acompanhamento.Dre, acompanhamento.QuantidadeComAcompanhamento, DashboardConstants.QuantidadeComAcompanhamento));
                dashboard.Add(new GraficoBaseDto(acompanhamento.Dre, acompanhamento.QuantidadeSemAcompanhamento, DashboardConstants.QuantidadeSemAcompanhamento));
            }

            return dashboard;
        }
    }
}
