using System;

namespace SME.SGP.Infra
{
    public class AlunoFrequenciaTurmaEvasaoDto 
    {
        public string Dre { get; set; }
        public string Ue { get; set; }
        public string Turma { get; set; }
        public string Aluno { get; set; }
        public double PercentualFrequencia { get; set; }
    }
}
