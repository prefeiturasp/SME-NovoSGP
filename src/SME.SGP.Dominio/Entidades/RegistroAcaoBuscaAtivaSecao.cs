using Dapper.Contrib.Extensions;
using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class RegistroAcaoBuscaAtivaSecao : EntidadeBase
    {
        public RegistroAcaoBuscaAtivaSecao()
        {
            Questoes = new List<QuestaoRegistroAcaoBuscaAtiva>();
        }

        [Computed]
        public RegistroAcaoBuscaAtiva RegistroAcaoBuscaAtiva { get; set; }
        public long RegistroAcaoBuscaAtivaId { get; set; }

        [Computed]
        public SecaoRegistroAcaoBuscaAtiva SecaoRegistroAcaoBuscaAtiva { get; set; }
        public long SecaoRegistroAcaoBuscaAtivaId { get; set; }

        public bool Concluido { get; set; }
        public bool Excluido { get; set; }

        [Computed]
        public List<QuestaoRegistroAcaoBuscaAtiva> Questoes { get; set; }
    }
}
