using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterItineranciasVisitasPAAIQuery : IRequest<IEnumerable<ItineranciaVisitaDto>>
    {
        public long UeId { get; set; }
        public int Ano { get; set; }
        public int Mes { get; set; }
        public long DreId { get; set; }

        public ObterItineranciasVisitasPAAIQuery(int ano, long dreId, long ueId, int mes)
        {
            Ano = ano;
            DreId = dreId;
            UeId = ueId;
            Mes = mes;
        }
    }
}
