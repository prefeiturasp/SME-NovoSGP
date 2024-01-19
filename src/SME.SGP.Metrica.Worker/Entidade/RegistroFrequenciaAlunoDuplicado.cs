using System;

namespace SME.SGP.Metrica.Worker.Entidade
{
    public class RegistroFrequenciaAlunoDuplicado
    {
        public long RegistroFrequenciaId { get; set; }
        public long AulaId { get; set; }
        public int NumeroAula { get; set; }
        public string AlunoCodigo { get; set; }
        public int Quantidade { get; set; }
        public DateTime PrimeiroRegistro { get; set; }
        public DateTime UltimoRegistro { get; set; }
        public long UltimoId { get; set; }
    }
}
