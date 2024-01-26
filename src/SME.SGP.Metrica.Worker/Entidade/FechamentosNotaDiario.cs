using System;

namespace SME.SGP.Metrica.Worker.Entidade
{
    public class FechamentosNotaDiario : EntidadeElasticBase
    {
        public FechamentosNotaDiario(DateTime data, int quantidade, int bimestre): base($"{data.ToString("yyyyMMdd")}-{bimestre}")
        {
            Data = data.Date.ToUniversalTime();
            Quantidade = quantidade;
            Ano = data.Year;
            Mes = data.Month;
            Bimestre = bimestre;
        }

        public DateTime Data { get; set; }
        public int Ano { get; set; }
        public int Mes { get; set; }
        public int Quantidade { get; set; }
        public int Bimestre { get; set; }
    }
}
