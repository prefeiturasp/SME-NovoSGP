using System;

namespace SME.SGP.Infra
{
    public class FechamentoReaberturaRetornoDto
    {
        public int[] Bimestres { get; set; }
        public int BimestresQuantidadeTotal { get; set; }
        public DateTime DataFim { get; set; }
        public DateTime DataInicio { get; set; }
        public string Descricao { get; set; }
        public string DreCodigo { get; set; }
        public long Id { get; set; }
        public string UeCodigo { get; set; }
    }
}