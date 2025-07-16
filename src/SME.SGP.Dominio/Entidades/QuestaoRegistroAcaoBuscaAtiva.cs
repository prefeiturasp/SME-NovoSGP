using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class QuestaoRegistroAcaoBuscaAtiva : EntidadeBase
    {
        public QuestaoRegistroAcaoBuscaAtiva()
        {
            Respostas = new List<RespostaRegistroAcaoBuscaAtiva>();
        }

        public RegistroAcaoBuscaAtivaSecao RegistroAcaoBuscaAtivaSecao { get; set; }
        public long RegistroAcaoBuscaAtivaSecaoId { get; set; }

        public Questao Questao { get; set; }
        public long QuestaoId { get; set; }

        public bool Excluido { get; set; }

        public List<RespostaRegistroAcaoBuscaAtiva> Respostas { get; set; }
    }
}
