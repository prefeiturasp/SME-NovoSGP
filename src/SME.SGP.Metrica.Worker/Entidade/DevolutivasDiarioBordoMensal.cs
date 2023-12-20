using System;

namespace SME.SGP.Metrica.Worker.Entidade
{
    public class DevolutivasDiarioBordoMensal : EntidadeElasticBase
    {
        public DevolutivasDiarioBordoMensal(DateTime data, int quantidade) : base(data.ToString("yyyyMM"))
        {
            Quantidade = quantidade;
            Ano = data.Year;
            Mes = data.Month;
        }
        public int Ano { get; set; }
        public int Mes { get; set; }
        public int Quantidade { get; set; }
    }
}
