using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class PlanoAEEReestrutucacaoPersistenciaDto
    {
        public long? ReestruturacaoId { get; set; }
        public long VersaoId { get; set; }
        public int Semestre { get; set; }
        public string Descricao { get; set; }
    }
}
