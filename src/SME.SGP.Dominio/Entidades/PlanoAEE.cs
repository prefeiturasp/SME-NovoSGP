using SME.SGP.Dominio.Enumerados;
using System.Collections.Generic;

namespace SME.SGP.Dominio.Entidades
{
    public class PlanoAEE : EntidadeBase
    {
        public PlanoAEE()
        {
            Questoes = new List<QuestaoEncaminhamentoAEE>();
        }

        public Turma Turma { get; set; }
        public long TurmaId { get; set; }
        public string AlunoNumeroChamada { get; set; }
        public string AlunoCodigo { get; set; }
        public string AlunoNome { get; set; }
        public SituacaoAEE Situacao { get; set; }

        public List<QuestaoEncaminhamentoAEE> Questoes { get; set; }
    }
}
