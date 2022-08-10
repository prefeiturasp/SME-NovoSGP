using System;

namespace SME.SGP.Infra
{
    public class FechamentoNotaPorTurmaPeriodoCCDto
    {
        public long Id { get; set; }
        public long DisciplinaId { get; set; }
        public double? Nota { get; set; }
        public long? ConceitoId { get; set; }
        public DateTime CriadoEm { get; set; }
        public string CriadoRF { get; set; }
        public string CriadoPor { get; set; }
        public DateTime? AlteradoEm { get; set; }
        public string AlteradoRF { get; set; }
        public string AlteradoPor { get; set; }
    }
}