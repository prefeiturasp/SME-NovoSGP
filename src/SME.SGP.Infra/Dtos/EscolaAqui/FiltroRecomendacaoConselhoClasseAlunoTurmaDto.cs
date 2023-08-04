using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroRecomendacaoConselhoClasseAlunoTurmaDto
    {
        public FiltroRecomendacaoConselhoClasseAlunoTurmaDto() {}

        public FiltroRecomendacaoConselhoClasseAlunoTurmaDto(string codigoAluno, string codigoTurma, int anoLetivo, int? modalidade, int semestre)
        {
            CodigoAluno = codigoAluno;
            CodigoTurma = codigoTurma;
            AnoLetivo = anoLetivo;
            Modalidade = modalidade;
            Semestre = semestre;
        }

        public string CodigoAluno { get; set; }
        public string CodigoTurma { get; set; }
        public int AnoLetivo { get; set; }
        public int? Modalidade { get; set; }
        public int Semestre { get; set; }
    }
}
