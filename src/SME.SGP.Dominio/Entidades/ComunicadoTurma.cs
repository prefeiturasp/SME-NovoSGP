using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio.Entidades
{
    public class ComunicadoTurma : EntidadeBase
    {
        public string CodigoTurma { get; set; }
        public long ComunicadoId { get; set; }
        public bool Excluido { get; set; }
    }
}
