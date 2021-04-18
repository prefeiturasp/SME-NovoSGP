using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos
{
    public class AEEAlunosMatriculadosDto
    {
        public int Ordem { get; set; }
        public string Descricao { get; set; }
        public string LegendaSRM { get; set; }
        public string LegendaPAEE { get; set; }
        public int QuantidadeSRM { get; set; }
        public int QuantidadePAEE { get; set; }
    }
}
