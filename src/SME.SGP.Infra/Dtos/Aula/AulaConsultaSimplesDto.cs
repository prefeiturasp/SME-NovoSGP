using System;

namespace SME.SGP.Infra
{
    public class AulaConsultaSimplesDto
    {
        public long Id { get; set; }
        public string UeId { get; set; }
        public string TurmaId { get; set; }
        public string DisciplinaId { get; set; }
        public int Quantidade { get; set; }
        public long TipoCalendarioId { get; set; }
        public DateTime DataAula { get; set; }
    }
}
