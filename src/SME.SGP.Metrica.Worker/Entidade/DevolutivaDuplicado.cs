using System;

namespace SME.SGP.Metrica.Worker.Entidade
{
    public class DevolutivaDuplicado
    {
        public DevolutivaDuplicado()
        {
            Data = DateTime.Now.Date;
        }

        public string Descricao { get; set; }
        public long ComponenteCurricularId { get; set; }
        public int Quantidade { get; set; }
        public DateTime PrimeiroRegistro { get; set; }
        public DateTime UltimoRegistro { get; set; }
        public long PrimeiroId { get; set; }
        public long UltimoId { get; set; }
        public DateTime Data { get; set; }
    }
}
