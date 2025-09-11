using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
   public class NotificacaoAulaPrevista :EntidadeBase
    {
        public long NotificacaoCodigo { get; set; }
        public string DisciplinaId { get; set; }
        public string TurmaId { get; set; }
        public int Bimestre { get; set; }
        public bool Excluido { get; set; }
    }
}
