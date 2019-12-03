using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio.Entidades
{
    public class NotasConceitoCicloParametro
    {
        public long Id { get; set; }
        public long CicloId { get; set; }
        public long TipoNotaId { get; set; }
        public int QtdMinimaAvalicoes{ get; set; }
        public int PercentualAlerta { get; set; }
        public bool Ativo { get; set; }
        public DateTime InicioVigencia { get; set; }
        public DateTime FimVigencia { get; set; }
    }
}
