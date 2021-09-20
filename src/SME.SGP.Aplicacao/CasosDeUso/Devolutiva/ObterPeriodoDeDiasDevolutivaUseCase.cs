using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoDeDiasDevolutivaUseCase : AbstractUseCase, IObterPeriodoDeDiasDevolutivaUseCase
    {
        public ObterPeriodoDeDiasDevolutivaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<string> Executar(FiltroTipoParametroAnoDto filtro)
        {
            var periodoDeDias = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(filtro.TipoParametro, filtro.AnoLetivo));
            return periodoDeDias.Valor;
        }
    }
}
