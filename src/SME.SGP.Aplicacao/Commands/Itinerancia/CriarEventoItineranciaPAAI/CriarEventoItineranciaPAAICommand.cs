using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class CriarEventoItineranciaPAAICommand : IRequest<bool>
    {
        public CriarEventoItineranciaPAAICommand(long itineranciaId, string dreCodigo, string ueCodigo, DateTime dataEvento, DateTime dataItinerancia, IEnumerable<ItineranciaObjetivoDto> objetivos)
        {
            ItineranciaId = itineranciaId;
            DreCodigo = dreCodigo;
            UeCodigo = ueCodigo;
            DataItinerancia = dataItinerancia;
            Objetivos = objetivos;
            DataEvento = dataEvento;
        }

        public long ItineranciaId { get; }
        public string DreCodigo { get; }
        public string UeCodigo { get; }
        public DateTime DataItinerancia { get; }
        public DateTime DataEvento { get; }
        public IEnumerable<ItineranciaObjetivoDto> Objetivos { get; }
    }
}
