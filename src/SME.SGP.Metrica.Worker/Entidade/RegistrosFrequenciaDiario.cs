using System;

namespace SME.SGP.Metrica.Worker.Entidade
{
    public class RegistrosFrequenciaDiario
    {
        public RegistrosFrequenciaDiario(DateTime data, int quantidade)
        {
            Data = data.ToUniversalTime();
            Quantidade = quantidade;
            Ano = data.Year;
            Mes = data.Month;
        }

        public DateTime Data { get; set; }
        public int Ano { get; set; }
        public int Mes { get; set; }
        public int Quantidade { get; set; }
    }
}
