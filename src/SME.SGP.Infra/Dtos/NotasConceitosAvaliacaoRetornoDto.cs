using System;

namespace SME.SGP.Infra
{
    public class NotasConceitosAvaliacaoRetornoDto
    {
        public DateTime Data { get; set; }
        public string Descricao { get; set; }
        public long Id { get; set; }
        public string Nome { get; set; }
        public bool EhInterdisciplinar { get; set; }
        public bool EhCJ { get; set; }
        public string[] Disciplinas { get; set; }
    }
}