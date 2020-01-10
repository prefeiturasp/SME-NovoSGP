using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class CompensacaoAusencia : EntidadeBase
    {
        public bool Excluido { get; set; }
        public int Bimestre { get; set; }
        public string DisciplinaId { get; set; }
        public string TurmaId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
    }
}
