using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class RegistroAcaoBuscaAtiva : EntidadeBase
    {
        public RegistroAcaoBuscaAtiva()
        {
            Secoes = new List<RegistroAcaoBuscaAtivaSecao>();
        }
        public Turma Turma { get; set; }
        public long TurmaId { get; set; }
        public string AlunoCodigo { get; set; }
        public string AlunoNome { get; set; }
        public bool Excluido { get; set; }
        public List<RegistroAcaoBuscaAtivaSecao> Secoes { get; set; }

        public RegistroAcaoBuscaAtiva Clone()
        {
            return (RegistroAcaoBuscaAtiva)this.MemberwiseClone();
        }

    }
}
