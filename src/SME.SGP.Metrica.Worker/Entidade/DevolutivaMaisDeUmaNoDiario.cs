using System;

namespace SME.SGP.Metrica.Worker.Entidade
{
    public class DevolutivaMaisDeUmaNoDiario
    {
        public DevolutivaMaisDeUmaNoDiario()
        {
            Data = DateTime.Now.Date;
        }

        public long DevolutivaId { get; set; }
        public int Quantidade { get; set; }
        public DateTime Data { get; set; }
    }
}
