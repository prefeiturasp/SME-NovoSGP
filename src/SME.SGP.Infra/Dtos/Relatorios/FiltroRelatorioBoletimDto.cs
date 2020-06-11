using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos.Relatorios
{
    public class FiltroRelatorioBoletimDto
    {
        public string CodigoUe { get; set; }

        public long? CodCicloEnsino { get; set; }

        public string CodigoTurma { get; set; }

        public Modalidade? Modalidade { get; set; }

        public int? SemestreEJA { get; set; }

        public string[] CodigosAluno { get; set; }
    }
}
