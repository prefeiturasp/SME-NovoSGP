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
        public int AlunoNumero { get; set; }
        public string AlunoCodigo { get; set; }
        public string AlunoNome { get; set; }
        public SituacaoPlanoAEE Situacao { get; set; }
        public string ParecerCoordenacao { get; set; }
        public string ParecerPAAI { get; set; }
        public long? ResponsavelId { get; set; }

        public List<PlanoAEEQuestao> Questoes { get; set; }

        public void EncerrarPlanoAEE() {
            Situacao = SituacaoPlanoAEE.ParecerCP;
        }
        public bool PodeDevolverPlanoAEE()
            => Situacao == SituacaoPlanoAEE.ParecerCP
            || Situacao == SituacaoPlanoAEE.ParecerPAAI
            || Situacao == SituacaoPlanoAEE.AtribuicaoPAAI;
    }
}
