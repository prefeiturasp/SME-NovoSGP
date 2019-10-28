using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class EventoDto
    {
        public DateTime? DataFim { get; set; }
        public DateTime DataInicio { get; set; }
        public string Descricao { get; set; }
        public string DreId { get; set; }
        public long? FeriadoId { get; set; }
        public long Id { get; set; }
        public EventoLetivo Letivo { get; set; }
        public string Nome { get; set; }
        public long TipoCalendarioId { get; set; }
        public long TipoEventoId { get; set; }
        public string UeId { get; set; }
    }
}