using System;

namespace SME.SGP.Dominio
{
    public class Evento : EntidadeBase
    {
        public DateTime? DataFim { get; set; }
        public DateTime DataInicio { get; set; }
        public string Descricao { get; set; }
        public string DreId { get; set; }
        public bool EhLetivo { get; set; }
        public FeriadoCalendario FeriadoCalendario { get; set; }
        public long? FeriadoId { get; set; }
        public string Nome { get; set; }
        public TipoCalendario TipoCalendario { get; set; }
        public long TipoCalendarioId { get; set; }
        public long TipoEventoId { get; set; }
        public string UeId { get; set; }
    }
}