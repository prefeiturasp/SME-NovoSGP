using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class OpcaoRespostaSimplesDto
    {
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string Nome { get; set; }
        public string Observacao { get; set; }
        public long QuestaoId { get; set; }
    }
}
