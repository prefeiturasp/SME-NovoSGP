using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class RetornoConsultaListagemTurmaComponenteDto
    {
        public int Id { get; set; }
        public long TurmaCodigo { get; set; }
        public string Modalidade { get; set; }
        public string NomeTurma { get; set; }
        public string Ano { get; set; }
        public string NomeComponenteCurricular { get; set; }
        public long ComponenteCurricularCodigo { get; set; }
        public int Turno { get; set; }
    }
}
