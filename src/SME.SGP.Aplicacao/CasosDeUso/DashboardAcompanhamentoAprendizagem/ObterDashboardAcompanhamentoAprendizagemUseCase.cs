using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDashboardAcompanhamentoAprendizagemUseCase : AbstractUseCase, IObterDashboardAcompanhamentoAprendizagemUseCase
    {
        public ObterDashboardAcompanhamentoAprendizagemUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<GraficoBaseDto>> Executar(FiltroDashboardAcompanhamentoAprendizagemDto filtro)
        {
            var acompanhamentosPorTurma = await mediator.Send(new ObterDashBoardEncaminhamentoAprendizagemQuery(filtro.AnoLetivo, filtro.DreId, filtro.UeId, filtro.Semestre));

            var dashboard = new List<GraficoBaseDto>();
            foreach (var acompanhamento in acompanhamentosPorTurma.Where(c => c.QuantidadeComAcompanhamento > 0 || c.QuantidadeSemAcompanhamento > 0))
            {
                dashboard.Add(new GraficoBaseDto(acompanhamento.Turma, acompanhamento.QuantidadeComAcompanhamento, DashboardConstants.QuantidadeComAcompanhamento));
                dashboard.Add(new GraficoBaseDto(acompanhamento.Turma, acompanhamento.QuantidadeSemAcompanhamento, DashboardConstants.QuantidadeSemAcompanhamento));
            }

            return dashboard;
        }
    }
}
