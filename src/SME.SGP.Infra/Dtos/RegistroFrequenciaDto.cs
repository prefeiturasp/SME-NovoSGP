using System;

namespace SME.SGP.Infra
{
    public class RegistroFrequenciaDto
    {
        public long AulaId { get; set; }
        public string CodigoAluno { get; set; }
        public DateTime DataAula { get; set; }
        public string DisciplinaId { get; set; }
        public bool Migrado { get; set; }
        public int NumeroAula { get; set; }
        public long RegistroFrequenciaId { get; set; }
        public long TipoCalendarioId { get; set; }
        public string TurmaId { get; set; }
    }
}