using System;

namespace SME.SGP.Metrica.Worker.Entidade
{
    public class AulasSemAtribuicaoSubstituicaoMensal : EntidadeElasticBase
    {
        public AulasSemAtribuicaoSubstituicaoMensal(DateTime data, int quantidade) : base(data.ToString("yyyyMM"))
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
