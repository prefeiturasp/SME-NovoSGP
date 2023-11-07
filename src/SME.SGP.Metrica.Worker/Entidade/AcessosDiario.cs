using System;

namespace SME.SGP.Metrica.Worker.Entidade
{
    public class AcessosDiario
    {
        public AcessosDiario(DateTime data, int quantidade)
        {
            Data = data;
            Quantidade = quantidade;
        }

        public DateTime Data { get; set; }
        public int Quantidade { get; set; }
    }
}
