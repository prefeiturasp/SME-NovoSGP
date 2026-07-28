using Dapper.Contrib.Extensions;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class OpcaoResposta : EntidadeBase
    {
        public OpcaoResposta()
        {
            QuestoesComplementares = new List<OpcaoQuestaoComplementar>();
        }
        [Computed]
        public Questao Questao { get; set; }
        public long QuestaoId { get; set; }
        public int Ordem { get; set; }
        public string Nome { get; set; }

        public string Observacao { get; set; }
        public bool Excluido { get; set; }
        [Computed]
        public List<OpcaoQuestaoComplementar> QuestoesComplementares { get; set; }
    }
}
