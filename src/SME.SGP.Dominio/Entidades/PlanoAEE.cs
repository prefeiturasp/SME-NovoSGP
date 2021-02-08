using SME.SGP.Dominio.Enumerados;
using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class PlanoAEE : EntidadeBase
    {
        public PlanoAEE()
        {
            Questoes = new List<PlanoAEEQuestao>();
        }

        public Turma Turma { get; set; }
        public long TurmaId { get; set; }
        public string AlunoNumero { get; set; }
        public string AlunoCodigo { get; set; }
        public string AlunoNome { get; set; }
        public SituacaoPlanoAEE Situacao { get; set; }

        public List<PlanoAEEQuestao> Questoes { get; set; }
    }
}
