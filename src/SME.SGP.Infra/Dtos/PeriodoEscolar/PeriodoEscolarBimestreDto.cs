using System;

namespace SME.SGP.Infra
{
    public class PeriodoEscolarBimestreDto
    {
        public int Id { get; set; }
        public int TipoCalendarioId { get; set; }
        public int Bimestre { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFim { get; set; }
        public bool Migrado { get; set; }
        public bool AulaCj { get; set; }
    }
}
