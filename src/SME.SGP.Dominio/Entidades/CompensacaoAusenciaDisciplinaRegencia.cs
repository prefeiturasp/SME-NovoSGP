using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class CompensacaoAusenciaDisciplinaRegencia : EntidadeBase
    {
        public bool Excluido { get; set; }
        public long CompensacaoAusenciaId { get; set; }
        public CompensacaoAusencia CompensacaoAusencia { get; set; }
        public string DisciplinaId { get; set; }

        public void Excluir()
            => Excluido = true;
    }
}
