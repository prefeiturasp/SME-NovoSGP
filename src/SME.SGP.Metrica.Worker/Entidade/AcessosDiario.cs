using System;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Metrica.Worker.Entidade
{
    [ExcludeFromCodeCoverage]
    public class AcessosDiario: EntidadeElasticBase
    {
        public AcessosDiario(DateTime data, int quantidade): base(data.ToString("yyyyMMdd"))
        {
            Data = data.Date.ToUniversalTime();
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
