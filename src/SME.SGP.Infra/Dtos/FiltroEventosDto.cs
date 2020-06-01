using System;

namespace SME.SGP.Infra
{
    public class FiltroEventosDto
    {
        public DateTime? DataFim { get; set; }
        public DateTime? DataInicio { get; set; }
        public string DreId { get; set; }
        public bool EhTodasDres { get; set; }
        public bool EhTodasUes { get; set; }
        public string NomeEvento { get; set; }
        public long? TipoCalendarioId { get; set; }
        public long? TipoEventoId { get; set; }
        public string UeId { get; set; }
        public bool ConsideraHistorico { get; set; }
    }
}