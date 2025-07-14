using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class OpcaoResposta : EntidadeBase
    {
        public OpcaoResposta()
        {
            QuestoesComplementares = new List<OpcaoQuestaoComplementar>();
        }
        public Questao Questao { get; set; }
        public long QuestaoId { get; set; }
        public int Ordem { get; set; }
        public string Nome { get; set; }

        public string Observacao { get; set; }
        public bool Excluido { get; set; }
        public List<OpcaoQuestaoComplementar> QuestoesComplementares { get; set; }
    }
}
