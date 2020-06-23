using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos.Relatorios
{
    public class FiltroRelatorioBoletimDto
    {
        public string DreCodigo { get; set; }

        public string UeCodigo { get; set; }

        public long? Semestre { get; set; }

        public string TurmaCodigo { get; set; }

        public int? AnoLetivo { get; set; }

        public Modalidade? Modalidade { get; set; }

        public string[] AlunosCodigo { get; set; }

        public Usuario Usuario { get; set; }
    }
}
