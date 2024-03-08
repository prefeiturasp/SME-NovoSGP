using MediatR;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ObterDashboardItineranciaVisitasPAAIQuery : IRequest<DashboardItineranciaVisitaPaais>
    {
        public long UeId { get; set; }
        public int Ano { get; set; }
        public int Mes { get; set; }
        public long DreId { get; set; }

        public ObterDashboardItineranciaVisitasPAAIQuery(int ano, long dreId, long ueId, int mes)
        {
            Ano = ano;
            DreId = dreId;
            UeId = ueId;
            Mes = mes;
        }
    }
}
