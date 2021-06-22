using MediatR;
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

        public Task<IEnumerable<GraficoBaseDto>> Executar(FiltroDasboardRegistroIndividualDTO filtro)
        {
            /** TODO Alterar para subtrair do parametro */
            var dataInicial = DateTime.Today.AddDays(-15);
            if (filtro.UeId > 0)
                return mediator.Send(new ObterQuantidadeDeAunosSemRegistroPorPeriodoUeQuery(filtro.AnoLetivo, filtro.Modalidade, dataInicial, filtro.UeId));
            else
                return mediator.Send(new ObterQuantidadeDeAunosSemRegistroPorPeriodoAnoQuery(filtro.AnoLetivo, filtro.Modalidade, dataInicial, filtro.DreId));
        }
    }
}
