using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroRelatorioResumoPAPDto
    {
        public string Ano { get; set; }
        public int? CicloId { get; set; }
        public string DreId { get; set; }
        public int? Periodo { get; set; }
        public string TurmaId { get; set; }
        public string UeId { get; set; }
        public int AnoLetivo { get; set; }
        public string UsuarioNome { get; set; }
        public string UsuarioRf { get; set; }
    }
}
