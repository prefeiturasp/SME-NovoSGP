using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class RegistroFrequenciaFaltanteDto
    {
        public string CodigoTurma { get; set; }
        public string NomeTurma { get; set; }
        public string DisciplinaId { get; set; }
        public string CodigoUe { get; set; }
        public string NomeUe { get; set; }
        public string CodigoDre { get; set; }
        public string NomeDre { get; set; }
        public int QuantidadeAulasSemFrequencia { get; set; }
        public string Aulas { get; set; }
    }
}
