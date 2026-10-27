using MediatR;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDashboardItineranciaObjetivosQuery : IRequest<IEnumerable<DashboardItineranciaDto>>
    {
        public long UeId { get; set; }
        public int Ano { get; set; }
        public int Mes { get; set; }
        public long DreId { get; set; }
        public string CodigoRF { get; set; }

        public ObterDashboardItineranciaObjetivosQuery(int ano, long dreId, long ueId, int mes, string codigoRF)
        {
            Ano = ano;
            DreId = dreId;
            UeId = ueId;
            Mes = mes;
            CodigoRF = codigoRF;
        }
    }
}
