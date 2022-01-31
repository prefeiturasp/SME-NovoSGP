using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDatasDiarioBordoPorPeriodoUseCase : AbstractUseCase, IObterDatasDiarioBordoPorPeriodoUseCase
    {
        public ObterDatasDiarioBordoPorPeriodoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<DiarioBordoPorPeriodoDto>> Executar(string turmaCodigo, DateTime dataInicio, DateTime dataFim, long componenteCurricularId)
        {
            var componentePai = await mediator.Send(new ObterCodigoComponentePaiQuery(componenteCurricularId));

            var diariosBordo = await mediator.Send(new ObterDatasDiarioBordoPorPeriodoQuery(turmaCodigo, dataInicio, dataFim, componentePai));

            return diariosBordo.OrderByDescending(a => a.DataAula).Distinct();
        }
    }
}
