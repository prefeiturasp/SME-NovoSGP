using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class CompensacaoAusencia : EntidadeBase
    {
        public CompensacaoAusencia()
        {
            Alunos = new List<CompensacaoAusenciaAluno>();
        }

        public int AnoLetivo { get; set; }
        public bool Excluido { get; set; }
        public bool Migrado { get; set; }
        public int Bimestre { get; set; }
        public string DisciplinaId { get; set; }
        public long TurmaId { get; set; }
        public Turma Turma { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool PermiteCompensacaoForaPeriodo { get; set; }

        public IEnumerable<CompensacaoAusenciaAluno> Alunos { get; set; }

        public void Excluir()
            => Excluido = true;
    }
}
