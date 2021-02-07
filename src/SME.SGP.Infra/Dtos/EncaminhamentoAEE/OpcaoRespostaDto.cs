using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class OpcaoRespostaDto
    {
        public List<QuestaoDto> QuestoesComplementares { get; set; }
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string Nome { get; set; }
        public string Observacao { get; set; }
    }
}
