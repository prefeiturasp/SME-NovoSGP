using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDashboardQuantidadeDeAlunosSemRegistroPorPeriodoUseCase : AbstractUseCase, IObterDashboardQuantidadeDeAlunosSemRegistroPorPeriodoUseCase
    {
        public ObterDashboardQuantidadeDeAlunosSemRegistroPorPeriodoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<GraficoBaseDto>> Executar(FiltroDasboardRegistroIndividualDTO filtro)
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.PeriodoDeDiasSemRegistroIndividual, filtro.AnoLetivo));
            var dataInicial = DateTime.Today.AddDays(-(int.Parse(parametro.Valor)));
            if (filtro.UeId > 0)
                return await mediator.Send(new ObterQuantidadeDeAunosSemRegistroPorPeriodoUeQuery(filtro.AnoLetivo, filtro.Modalidade, dataInicial, filtro.UeId));
            else
                return await mediator.Send(new ObterQuantidadeDeAunosSemRegistroPorPeriodoAnoQuery(filtro.AnoLetivo, filtro.Modalidade, dataInicial, filtro.DreId));
        }
    }
}
