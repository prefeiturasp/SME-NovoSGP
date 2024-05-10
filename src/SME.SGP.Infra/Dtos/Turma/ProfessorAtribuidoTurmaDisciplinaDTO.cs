using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class ProfessorAtribuidoTurmaDisciplinaDTO
    {
        public string CodigoTurma { get; set; }
        public string AnoLetivo { get; set; }
        public string NomeTurma { get; set; }
        public DateTime DataInicioAtribuicao { get; set; }
        public DateTime DataFimAtribuicao { get; set; }
        public DateTime DataFimTurma { get; set; }
        public int AnoAtribuicao { get; set; }
        public string CodigoRf { get; set; }
        public string DisciplinaId { get; set; }
        public string NomeProfessor { get; set; }
    }
}
