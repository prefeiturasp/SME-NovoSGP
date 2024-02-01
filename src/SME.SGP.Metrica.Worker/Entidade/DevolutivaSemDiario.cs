using System;

namespace SME.SGP.Metrica.Worker.Entidade
{
    public class DevolutivaSemDiario
    {
        public DevolutivaSemDiario()
        {
            Data = DateTime.Now.Date;
        }

        public long DevolutivaId { get; set; }
        public DateTime Data { get; set; }
    }
}
