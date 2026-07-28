using Dapper.Contrib.Extensions;
using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class QuestaoRegistroAcaoBuscaAtiva : EntidadeBase
    {
        public QuestaoRegistroAcaoBuscaAtiva()
        {
            Respostas = new List<RespostaRegistroAcaoBuscaAtiva>();
        }

        [Computed]
        public RegistroAcaoBuscaAtivaSecao RegistroAcaoBuscaAtivaSecao { get; set; }
        public long RegistroAcaoBuscaAtivaSecaoId { get; set; }

        [Computed]
        public Questao Questao { get; set; }
        public long QuestaoId { get; set; }

        public bool Excluido { get; set; }

        [Computed]
        public List<RespostaRegistroAcaoBuscaAtiva> Respostas { get; set; }
    }
}
