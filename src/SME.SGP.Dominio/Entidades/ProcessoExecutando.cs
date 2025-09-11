using System;

namespace SME.SGP.Dominio
{
    public class ProcessoExecutando
    {
        public long Id { get; set; }
        public TipoProcesso TipoProcesso { get; set; }
        public string TurmaId { get; set; }
        public string DisciplinaId { get; set; }
        public int Bimestre { get; set; }
        public long? AulaId { get; set; }
        public DateTime CriadoEm { get; set; }
    }
}
