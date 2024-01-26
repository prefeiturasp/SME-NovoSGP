using System;

namespace SME.SGP.Metrica.Worker.Entidade
{
    public class ConselhoClasseDuplicado
    {
        public long FechamentoTurmaId { get; set; }
        public int Quantidade { get; set; }
        public DateTime PrimeiroRegistro { get; set; }
        public DateTime UltimoRegistro { get; set; }
        public long UltimoId { get; set; }
    }
}
